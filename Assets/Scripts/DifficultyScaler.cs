using UnityEngine;

public class DifficultyScaler : MonoBehaviour
{
    [SerializeField] AnimationCurve m_DifficultyCurve;
    [SerializeField] float m_SecondBeforeMaxDifficulty;

    [SerializeField] float m_GenPassengerCooldownInit;
    [SerializeField] float m_GenPassengerCooldownMin;

    [SerializeField] float m_PassengerPatience;
    [SerializeField] float m_PassengerPatienceMin;

    public static DifficultyScaler Instance = null;
    private void Awake()
    {
        if (Instance == null) // If there is no instance already
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    public float CurrentGenPassengerCooldown(float timer)
    {
        float f = m_DifficultyCurve.Evaluate(timer / m_SecondBeforeMaxDifficulty);
        return Mathf.Lerp(m_GenPassengerCooldownInit, m_GenPassengerCooldownMin, f);
    }
    public float CurrentPassengerPatience(float timer)
    {
        float f = m_DifficultyCurve.Evaluate(timer / m_SecondBeforeMaxDifficulty);
        return Mathf.Lerp(m_PassengerPatience, m_PassengerPatienceMin, f);
    }
}
