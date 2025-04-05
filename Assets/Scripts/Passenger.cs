using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Passenger : MonoBehaviour
{
    private bool _isInElevator;

    private Floor _start;
    private Floor _goal;

    public float m_PatienceTime;
    [SerializeField] float m_PatienceDiminution;

    // Start is called before the first frame update
    void Start()
    {
        _start = GetRandomFloor();
        _goal = GetRandomFloor();
        _isInElevator = false;

        SetToStartPoint();
    }

    private Floor GetRandomFloor(Floor except = null)
    {
        List<Floor> f = GameManager.Instance.AllFloors;
        if (except != null)
        {
            f = f.Where((f) => f.Equals(except)).ToList();
        }
        return f[UnityEngine.Random.Range(0, f.Count)];
    }

    private void SetToStartPoint()
    {
        var pointer = UIManager.Instance.GetPointer(Mathf.Abs(_start.FloorNumber));
        pointer.SetNewPassenger(this);
    }

    public void SetInElevator() => _isInElevator = true;

    public bool IsArrivedToDestination(Floor currentFloor)
    {
        return _isInElevator && currentFloor.FloorNumber == _goal.FloorNumber;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_isInElevator)
        {
            transform.position = PlayerController.Instance.GetElevatorPos();
            UIManager.Instance.GetElevatorButton(Mathf.Abs(_goal.FloorNumber)).SetOn(true);
        }
    }

    private void OnDestroy()
    {
        UIManager.Instance.GetElevatorButton(Mathf.Abs(_goal.FloorNumber)).SetOn(false);
    }
}
