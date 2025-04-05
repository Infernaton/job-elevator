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

    [SerializeField] Image m_ElevatorImage;
    private float _movement;
    private float _lastMovement;

    private bool _isDoorsOpen = true;

    #region Elevator Height
    private float _currentHeight;
    public float GetCurrentHeight()
    {
        if (Mathf.Sign(_lastMovement) == 1)
            return Mathf.Ceil(_currentHeight + GetHeightOffset());
        return Mathf.Floor(_currentHeight - GetHeightOffset());
    }
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
        m_ElevatorImage.rectTransform.anchoredPosition = new Vector3(0, y, 0);
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
