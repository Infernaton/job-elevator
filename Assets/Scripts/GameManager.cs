using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private int _currentFloor;
    public int CurrentFloor
    {
        get => _currentFloor;
        private set
        {
            _currentFloor = value;
        }
    }

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
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CurrentFloor++;
    }
}
