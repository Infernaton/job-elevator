using System;
using System.Collections;
using System.Collections.Generic;
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

    private Floor GetRandomFloor()
    {
        return GameManager.Instance.AllFloors[UnityEngine.Random.Range(0, GameManager.Instance.AllFloors.Count)];
    }

    private void SetToStartPoint()
    {
        var pointer = UIManager.Instance.GetPointer(Mathf.Abs(_start.FloorNumber));
        pointer.SetNewPassenger(this);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //m_PatienceTime -= Time.fixedDeltaTime;
    }
}
