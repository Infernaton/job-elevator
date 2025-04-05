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
    #endregion

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
            var passenger = _passengerList.Find((item) => item.IsArrivedToDestination(GetCurrentFloorData()));
            if (passenger != null)
            {
                // If so, remove the passengers from the array 
                _passengerList.Remove(passenger);
                Destroy(passenger.gameObject);
                Debug.Log("A passenger as arrived to destination");
            }

            // Get Current Pointer based on Current Floor
            FloorPointer pointer = GetCurrentPointer();
            if (pointer.ArePassengersWaiting())
            {
                // If there's a passenger
                var waitingPassenger = pointer.GetFirstPassengerWaiting();
                //remove it from the floor array
                pointer.RemovePassenger(waitingPassenger);
                //set the _isInElevator bool to true
                waitingPassenger.SetInElevator();
                // => Simulate that the passenger is in the elvator
            }
        }

        //Generate new passengers over time
        if (Time.time - _lastTimeGenPassenger >= _currentGenPassengerCooldown)
        {
            GeneratePassenger();
            _lastTimeGenPassenger = Time.time;
        }
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
