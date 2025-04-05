using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] List<Floor> m_InitFloors;
    [SerializeField] float m_FloorArrivesOffset;

    private Dictionary<float, Floor> _compileFloor = new();
    private Floor _currentFloor;
    public Floor GetCurrentFloorData() => _currentFloor;

    private float _currentHeight;
    public float GetCurrentHeight() => Mathf.Round(_currentHeight);
    public void AddHeight(float add){ _currentHeight += add; }


    public static GameManager Instance = null;

    private void Awake()
    {
        if (Instance == null) // If there is no instance already
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        CompileFloorData();
    }

    private void CompileFloorData()
    {
        foreach (Floor floor in m_InitFloors)
        {
            _compileFloor.Add(floor.FloorNumber, floor);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_compileFloor.TryGetValue(GetCurrentHeight(), out var floor))
        {
            if (_currentFloor == null || !_currentFloor.Equals(floor))
            {
                _currentFloor = floor;
                Debug.Log("Arrives to " + floor.Name);
            }
        } else
        {
            _currentFloor = null;
        }
    }
}
