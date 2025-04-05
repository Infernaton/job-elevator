using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float m_MovementCancelOffset;
    [SerializeField] float m_MovementReducer;

    [SerializeField] ElevatorButtonDisplay m_ElevatorUI;
    public Vector3 GetElevatorPos() => m_ElevatorUI.transform.position;
    private float _movement;
    private float _lastMovement;

    #region Door Handler
    private bool _isDoorsOpen = true;

    public bool GetDoorState() => _isDoorsOpen;
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

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Math.Abs(_movement) > m_MovementCancelOffset)
        {
            _lastMovement = _movement;
            AddHeight(_movement * m_MovementReducer);
            if (_isDoorsOpen) _isDoorsOpen = false;
        }
    }

    private void Update()
    {
        var gm = GameManager.Instance;
        float y = gm.StartYPos + _currentHeight * gm.FloorSize;
        m_ElevatorUI.SetUIPosition(new Vector3(0, y, 0));

        m_ElevatorUI.SetOn(_isDoorsOpen);
    }

    public void OnMove(InputAction.CallbackContext value)
    {
        _movement = value.ReadValue<Vector2>().y;
        // _movement is between -1 and 1 but the Slider in UI
        // can have value between -2 and 2
        UIManager.Instance.UpdateLeverPos(_movement * 2);
    }

    public void SliderOnMove(float value)
    {
        _movement = value/2;
    }

    public void ToggleDoors()
    {
        _isDoorsOpen = !_isDoorsOpen;
    }
}
