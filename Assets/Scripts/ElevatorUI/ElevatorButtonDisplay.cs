using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class ElevatorButtonDisplay : MonoBehaviour
{
    [SerializeField] Sprite m_ButtonOff;
    [SerializeField] Sprite m_ButtonOn;
    [SerializeField] Image m_ChildImage;

    [SerializeField] AudioSource m_SetOn;

    private Image _image;

    private bool _isOn;
    public bool IsOn
    {
        get => _isOn;
        set
        {
            if (value && _isOn != value && m_SetOn != null)
                SoundModifier.PlayAdjustPitch(m_SetOn);

            _isOn = value;
        }
    }

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

    // Update is called once per frame
    void Update()
    {
        _image.sprite = IsOn ? m_ButtonOn : m_ButtonOff;
    }
}
