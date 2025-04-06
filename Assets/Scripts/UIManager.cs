using System;
using System.Collections;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Control Panel")]
    [SerializeField] Slider m_ControlPanel;

    [Header("Container")]
    [SerializeField] ButtonContainer m_ButtonContainer;
    [SerializeField] ButtonContainer m_PointerContainer;

    [Header("StatsUI")]
    [SerializeField] TMP_Text m_TimeText;
    [SerializeField] TMP_Text m_ScoreText;
    [SerializeField] TMP_Text m_ElevatorRuleText;

    [SerializeField] Color m_DangerColor;
    [SerializeField] AnimationCurve m_AnimationElevatorMaxCurve;
    //[SerializeField] float m_AnimationTime;

    public ElevatorButtonDisplay GetElevatorButton(int id) => m_ButtonContainer.GetButton(id);
    public FloorPointer GetPointer(int id) => m_PointerContainer.GetButton(id).GetComponent<FloorPointer>();

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

    public void UpdateLeverPos(float height)
    {
        m_ControlPanel.value = height;
    }

    #region Stats
    public void UpdateTimeUI(float time)
    {
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);
        m_TimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void UpdateScoreUI(int score)
    {
        m_ScoreText.text = string.Format("{0:000}", score);
    }

    public void UpdatePassengersCapacityUI(int currentCapacity, int maxCapacity)
    {
        m_ElevatorRuleText.text = string.Format("{0:0} / {1:0}", currentCapacity, maxCapacity);
        float multiplier = 0;
        Color appliedColor = Color.white;
        if (currentCapacity >= maxCapacity)
        {
            multiplier = m_AnimationElevatorMaxCurve.Evaluate(Time.time);
            appliedColor = m_DangerColor;
            Debug.Log(multiplier);
        }

        m_ElevatorRuleText.transform.localScale = Vector3.one + Vector3.one * multiplier;
        m_ElevatorRuleText.color = appliedColor;
    }
    #endregion
}
