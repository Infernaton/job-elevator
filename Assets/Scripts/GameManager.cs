using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Floor> AllFloors;

    public float StartYPos;
    public float FloorSize;

    private Dictionary<float, Floor> _compileFloor = new();
    private Floor _currentFloor;
    public Floor GetCurrentFloorData() => _currentFloor;

    #region Passengers
    private List<Passenger> _passengerList;
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
        #region Floor Detection
        if (_compileFloor.TryGetValue(PlayerController.Instance.GetCurrentHeight(), out var floor))
        {
            if (_currentFloor == null || !_currentFloor.Equals(floor))
            {
                _currentFloor = floor;

                Debug.Log("Arrives to " + floor.Name);
                int index = (int)Mathf.Abs(floor.FloorNumber);
                UIManager.Instance.SelectPointer(index);
            }
        } else
        {
            _currentFloor = null;
        }
        #endregion

        #region Generate Passangers

        #endregion
    }
}
