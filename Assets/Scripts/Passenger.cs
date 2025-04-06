using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Passenger : MonoBehaviour
{
    private bool _isInElevator;
    public bool IsInElevator() => _isInElevator;

    private Floor _start;
    private Floor _goal;

    public float _patienceTime;
    [SerializeField] float m_PatienceTreshold;
    [SerializeField] AnimationCurve m_PatienceAnimationCurve;

    [SerializeField] Image m_Sprite;
    [SerializeField] Color m_UpColor;
    [SerializeField] Color m_DownColor;

    // Use only with a prefab to initiate
    public Passenger Create(Floor start, Floor goal)
    {
        Passenger passenger = Instantiate(this);

        passenger._start = start;
        passenger._goal = goal;
        return passenger;
    }

    void Start()
    {
        if (_start == null) _start = GetRandomFloor();
        if (_goal == null) _goal = GetRandomFloor(_start);
        _isInElevator = false;

        _patienceTime = DifficultyScaler.Instance.CurrentPassengerPatience(GameManager.Instance.GetCurrentTimer());

        m_Sprite.color = _start.FloorNumber > _goal.FloorNumber ? m_DownColor : m_UpColor;

        SetToStartPoint();
    }

    private Floor GetRandomFloor(Floor except = null)
    {
        List<Floor> f = GameManager.Instance.AllFloors;
        if (except != null)
        {
            f = f.Where((f) => !f.Equals(except)).ToList();
        }
        return f[UnityEngine.Random.Range(0, f.Count)];
    }

    private void SetToStartPoint()
    {
        var pointer = UIManager.Instance.GetPointer(Mathf.Abs(_start.FloorNumber));
        pointer.SetNewPassenger(this);
        StartCoroutine(Animation.FadeIn(0.15f, m_Sprite));
    }

    public void SetInElevator() => _isInElevator = true;

    public bool IsArrivedToDestination(Floor currentFloor)
    {
        return _isInElevator && currentFloor.FloorNumber == _goal.FloorNumber;
    }

    void FixedUpdate()
    {
        if (!GameManager.IsInGame()) return;

        if (_isInElevator)
        {
            transform.position = PlayerController.Instance.GetElevatorPos();
            UIManager.Instance.GetElevatorButton(Mathf.Abs(_goal.FloorNumber)).SetOn(true);
            m_Sprite.gameObject.SetActive(false);
            gameObject.transform.localScale = Vector3.one; // reset animation 
        } else
        {
            _patienceTime -= Time.fixedDeltaTime;
        }
    }

    private void Update()
    {
        if (_patienceTime <= m_PatienceTreshold)
        {
            float multiplier = m_PatienceAnimationCurve.Evaluate(Time.time);
            gameObject.transform.localScale = Vector3.one + Vector3.one * multiplier;
        }

        if (!GameManager.IsInGame()) return;

        if (_patienceTime <= 0)
        {
            // GameOver;
            Debug.Log("Game over");
            GameManager.Instance.SetGameOver("When will this elevator arrives ??");
        }
    }

    private void OnDestroy()
    {
        UIManager.Instance.GetElevatorButton(Mathf.Abs(_goal.FloorNumber)).SetOn(false);
    }
}
