using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] TMP_Text CurrentFloor;

    public static UIManager Instance = null;

    private void Awake()
    {
        if (Instance == null) // If there is no instance already
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    private void Update()
    {
        CurrentFloor.text = GameManager.Instance.CurrentFloor.ToString();
    }
}
