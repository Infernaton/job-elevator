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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Math.Abs(_movement) > m_MovementCancelOffset)
        {
            GameManager.Instance.AddHeight(_movement * m_MovementReducer);
        }
    }

    public void OnMove(InputAction.CallbackContext value)
    {
        _movement = value.ReadValue<Vector2>().y;
    }
}
