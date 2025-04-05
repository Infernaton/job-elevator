using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonContainer : MonoBehaviour
{
    [SerializeField] List<ElevatorButtonDisplay> m_ButtonsList;

    [ContextMenu("InitNumber")]
    void InitSpriteNumber()
    {
        for (int i = 0; i < m_ButtonsList.Count; i++)
        {
            var b = m_ButtonsList[i];
            b.SetSpriteNumber(GameManager.Instance.AllFloors[i].ButtonSprite);
        }
    }
}
