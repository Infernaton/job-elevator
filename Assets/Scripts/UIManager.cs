using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Control Panel")]
    [SerializeField] Slider m_ControlPanel;

    [Header("Container")]
    [SerializeField] ButtonContainer m_ButtonContainer;
    [SerializeField] ButtonContainer m_PointerContainer;

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
}
