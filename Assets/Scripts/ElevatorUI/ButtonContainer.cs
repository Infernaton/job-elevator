using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ButtonContainer : MonoBehaviour
{
    [SerializeField] ElevatorButtonDisplay m_ButtonPrefab;
    [SerializeField] float m_FloorSizeReducer = 0;
    [SerializeField] float m_FloorStartOffset = 0;



    private List<ElevatorButtonDisplay> _buttonsList = new();

    public void InitSpriteNumber()
    {
        var gm = GameManager.Instance;
        for (int i = 0; i < gm.AllFloors.Count; i++)
        {
            var b = Instantiate(m_ButtonPrefab, transform); // m_ButtonsList[i];
            float y = (gm.StartYPos + m_FloorStartOffset) - i * (gm.FloorSize - m_FloorSizeReducer);
            b.SetUIPosition(new Vector3(0, y, 0));
            b.SetSpriteNumber(gm.AllFloors[i].ButtonSprite);
            _buttonsList.Add(b);
        }
    }

    public ElevatorButtonDisplay GetButton(int id)
    {
        return _buttonsList[id];
    }

    public void SelectOneButton(int index)
    {
        for (int i = 0; i < _buttonsList.Count; i++)
        {
            _buttonsList[i].IsOn = false;
        }
        _buttonsList[index].IsOn = true;
    }
}
