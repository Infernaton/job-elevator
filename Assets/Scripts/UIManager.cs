using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] ButtonContainer m_ButtonContainer;
    [SerializeField] ButtonContainer m_PointerContainer;

    public static UIManager Instance = null;

    private void Awake()
    {
        if (Instance == null) // If there is no instance already
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        m_ButtonContainer.InitSpriteNumber();
        m_PointerContainer.InitSpriteNumber();
    }

    private void Update()
    {

    }

    public void SelectPointer(int id)
    {
        m_PointerContainer.SelectOneButton(id);
    }

    public FloorPointer GetPointer(int id)
    {
        return m_PointerContainer.GetButton(id).GetComponent<FloorPointer>();
    }
}
