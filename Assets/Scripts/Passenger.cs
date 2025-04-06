using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class Passenger : MonoBehaviour
{
    private bool _isInElevator;
    public bool IsInElevator
    {
        get => _isInElevator;
        set => _isInElevator = value;
    }

    private Floor _start;
    private Floor _goal;

    public float _patienceTime;
    [SerializeField] float m_PatienceTreshold;
    [SerializeField] AnimationCurve m_PatienceAnimationCurve;

    [SerializeField] Image m_Sprite;
    [SerializeField] Color m_UpColor;
    [SerializeField] Color m_DownColor;

    [SerializeField] AudioSource m_MvtSwift;

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
        SoundModifier.PlayAdjustPitch(m_MvtSwift);
        StartCoroutine(Anim.FadeIn(0.15f, m_Sprite));
    }

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
            UIManager.Instance.GetElevatorButton(Mathf.Abs(_goal.FloorNumber)).IsOn = true;
            m_Sprite.gameObject.SetActive(false);
            gameObject.transform.localScale = Vector3.one; // reset animation 
        } else if (!_goal.Equals(GameManager.Instance.GetCurrentFloorData()))
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

    public IEnumerator LeaveElevator()
    {
        IsInElevator = false;
        yield return Anim.FadeIn(0.1f, m_Sprite);

        StartCoroutine(Anim.FadeOut(0.2f, m_Sprite));
        yield return Anim.Slide(gameObject, transform.position + Vector3.left, 0.2f, AnimationCurve.EaseInOut(0, 0, 1, 1));

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        UIManager.Instance.GetElevatorButton(Mathf.Abs(_goal.FloorNumber)).IsOn = false;
    }
}
