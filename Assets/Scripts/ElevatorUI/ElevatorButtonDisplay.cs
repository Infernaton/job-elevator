using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElevatorButtonDisplay : MonoBehaviour
{
    [SerializeField] Sprite m_ButtonOff;
    [SerializeField] Sprite m_ButtonOn;
    [SerializeField] Image m_ChildImage;

    private Image _image;

    private bool _isOn;

    // Start is called before the first frame update
    void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void SetUIPosition(Vector3 pos)
    {
        _image.rectTransform.anchoredPosition = pos;
    }

    public void SetSpriteNumber(Sprite _nb)
    {
        if (m_ChildImage == null) return;
        m_ChildImage.sprite = _nb;
    }

    public void SetOn(bool isOn)
    {
        _isOn = isOn;
    }

    // Update is called once per frame
    void Update()
    {
        _image.sprite = _isOn ? m_ButtonOn : m_ButtonOff;
    }
}
