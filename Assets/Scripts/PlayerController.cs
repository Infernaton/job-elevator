using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float m_MovementCancelOffset;
    [SerializeField] float m_MovementReducer;
    private float _movement;

    private bool _isDoorsOpen = true;

    #region Elevator Height
    private float _currentHeight;
    public float GetCurrentHeight() => Mathf.Round(_currentHeight);
    public void AddHeight(float add) { _currentHeight += add; }
    #endregion

    public static PlayerController Instance = null;
    private void Awake()
    {
        if (Instance == null) // If there is no instance already
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Math.Abs(_movement) > m_MovementCancelOffset)
        {
            AddHeight(_movement * m_MovementReducer);
            if (_isDoorsOpen) _isDoorsOpen = false;
        }
    }

    public void OnMove(InputAction.CallbackContext value)
    {
        _movement = value.ReadValue<Vector2>().y;
    }

    public void ToggleDoors()
    {
        _isDoorsOpen = !_isDoorsOpen;
    }
}
