using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float m_MovementCancelOffset;
    [SerializeField] float m_MovementReducer;

    [SerializeField] ElevatorButtonDisplay m_ElevatorUI;
    public Vector3 GetElevatorPos() => m_ElevatorUI.transform.position;
    private float _movement;
    private float _lastMovement;

    #region Door Handler
    [SerializeField] private bool _isDoorsOpen;
    public bool IsDoorsOpen
    {
        get => _isDoorsOpen;
        private set
        {
            _isDoorsOpen = value;

            if (_isDoorsOpen && !m_DoorsSound.isPlaying)
            {
                m_DoorsSound.Play();
            }
            else if (!m_DoorsCloseSound.isPlaying)
            {
                m_DoorsCloseSound.Play();
            }
        }
    }
    #endregion

    #region Elevator Height
    [Header("Height Handler")]
    [SerializeField] private float m_MinHeight;
    [SerializeField] private float m_MaxHeight;
    private float _currentHeight;
    public float GetCurrentHeight()
    {
        if (Mathf.Sign(_lastMovement) == 1)
            return Mathf.Ceil(_currentHeight + GetHeightOffset());
        return Mathf.Floor(_currentHeight - GetHeightOffset());
    }
    public void AddHeight(float add) { 
        _currentHeight += add; 

        _currentHeight = Mathf.Max(m_MinHeight, Mathf.Min(_currentHeight, m_MaxHeight));
    }
    #endregion

    [Header("Sound")]
    [SerializeField] private AudioSource m_DoorsSound;
    [SerializeField] private AudioSource m_DoorsCloseSound;

    public static PlayerController Instance = null;
    private void Awake()
    {
        if (Instance == null) // If there is no instance already
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    private float GetHeightOffset()
    {
        var gm = GameManager.Instance;
        return gm.StartYPos / gm.FloorSize;
    }

    void FixedUpdate()
    {
        if (!GameManager.IsInGame()) return;
        if (Math.Abs(_movement) > m_MovementCancelOffset && !IsDoorsOpen)
        {
            _lastMovement = _movement;
            AddHeight(_movement * m_MovementReducer);
        }
    }

    private void Update()
    {
        if (!GameManager.IsInGame()) return;
        var gm = GameManager.Instance;
        float y = gm.StartYPos + _currentHeight * gm.FloorSize;
        m_ElevatorUI.SetUIPosition(new Vector3(0, y, 0));

        m_ElevatorUI.IsOn = IsDoorsOpen;
    }

    public void OnMove(InputAction.CallbackContext value)
    {
        if (!GameManager.IsInGame()) return;

        _movement = value.ReadValue<Vector2>().y;
        // _movement is between -1 and 1 but the Slider in UI
        // can have value between -2 and 2
        UIManager.Instance.UpdateLeverPos(_movement * 2);
    }

    public void SliderOnMove(float value)
    {
        _movement = value/2;
    }

    public void ToggleDoors(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed) return;

        ToggleDoors();
    }

    public void ToggleDoors()
    {
        IsDoorsOpen = !IsDoorsOpen;
    }
}
