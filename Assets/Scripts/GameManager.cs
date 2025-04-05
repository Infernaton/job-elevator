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

                Debug.Log("Arrives to " + floor.Name);
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
