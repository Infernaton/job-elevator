using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Floor Handler
    [Header("Floor related")]
    public List<Floor> AllFloors;
    public float StartYPos;
    public float FloorSize;

    private Dictionary<float, Floor> _compileFloor = new();
    private Floor _currentFloor;
    public Floor GetCurrentFloorData() => _currentFloor;
    public FloorPointer GetCurrentPointer() => UIManager.Instance.GetPointer(Mathf.Abs(_currentFloor.FloorNumber));
    #endregion

    #region Passengers
    [Header("Passenger related")]
    [SerializeField] Passenger m_PassengerPrefab;
    private List<Passenger> _passengerList = new();

    [SerializeField] float m_GenPassengerCooldownInit;
    private float _currentGenPassengerCooldown;
    private float _lastTimeGenPassenger;

    [SerializeField] int m_MaxPickupPassengers;
    #endregion

    private int _score;

    public static GameManager Instance = null;
    private void Awake()
    {
        if (Instance == null) // If there is no instance already
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    #region Start
    void Start()
    {
        CompileFloorData();
        _currentGenPassengerCooldown = m_GenPassengerCooldownInit;
    }

    private void CompileFloorData()
    {
        foreach (Floor floor in AllFloors)
        {
            _compileFloor.Add(floor.FloorNumber, floor);
        }
    }
    #endregion

    // Update is called once per frame
    void FixedUpdate()
    {
        FloorDetection();

        // Don't need to loop throught all arrays, we are in a FixedUpdate, the loop is already here
        // If the door is Open
        if (PlayerController.Instance.GetDoorState())
        {
            // check for all passengers who are _isInElevator == true if the current floor is equal to Passenger._end
            var passengersInElevator = _passengerList.FindAll((item) => item.IsInElevator());
            var passengers = passengersInElevator.FindAll((item) => item.IsArrivedToDestination(GetCurrentFloorData()));
            if (passengers.Count > 0)
            {
                var passenger = passengers[0];
                // If so, remove the passengers from the array 
                _passengerList.Remove(passenger);
                Destroy(passenger.gameObject);
                _score++;
                Debug.Log("A passenger as arrived to destination");
            }

            // Get Current Pointer based on Current Floor
            FloorPointer pointer = GetCurrentPointer();
            if (passengersInElevator.Count < m_MaxPickupPassengers && pointer.ArePassengersWaiting())
            {
                // If there's a passenger
                var waitingPassenger = pointer.GetFirstPassengerWaiting();
                //remove it from the floor array
                pointer.RemovePassenger(waitingPassenger);
                //set the _isInElevator bool to true
                waitingPassenger.SetInElevator();
                // => Simulate that the passenger is in the elevator
            }
        }

        //Generate new passengers over time
        if (Time.time - _lastTimeGenPassenger >= _currentGenPassengerCooldown)
        {
            GeneratePassenger();
            _lastTimeGenPassenger = Time.time;
        }
    }

    private void Update()
    {
        var ui = UIManager.Instance;

        var passengers = _passengerList.FindAll((item) => item.IsInElevator());
        ui.UpdatePassengersCapacityUI(passengers.Count, m_MaxPickupPassengers);

        ui.UpdateTimeUI(Time.time);

        ui.UpdateScoreUI(_score);
    }

    private void FloorDetection()
    {
        if (_compileFloor.TryGetValue(PlayerController.Instance.GetCurrentHeight(), out var floor))
        {
            if (_currentFloor == null || !_currentFloor.Equals(floor))
            {
                _currentFloor = floor;

                //Debug.Log("Arrives to " + floor.Name);
                UIManager.Instance.SelectPointer(Mathf.Abs(floor.FloorNumber));
            }
        }
        else
        {
            _currentFloor = null;
        }
    }

    private void GeneratePassenger()
    {
        var passenger = Instantiate(m_PassengerPrefab);
        _passengerList.Add(passenger);
    }
}
