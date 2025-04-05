using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElevatorButtonDisplay : MonoBehaviour
{
    [SerializeField] Sprite m_ButtonOff;
    [SerializeField] Sprite m_ButtonOn;
    [SerializeField] Image m_ChildImage;

    public Image Image;

    private bool _isOn;

    // Start is called before the first frame update
    void Awake()
    {
        Image = GetComponent<Image>();
    }

    public void SetSpriteNumber(Sprite _nb)
    {
        m_ChildImage.sprite = _nb;
    }

    public void SetOn(bool isOn)
    {
        _isOn = isOn;
    }

    // Update is called once per frame
    void Update()
    {
        Image.sprite = _isOn ? m_ButtonOn : m_ButtonOff;
    }
}
