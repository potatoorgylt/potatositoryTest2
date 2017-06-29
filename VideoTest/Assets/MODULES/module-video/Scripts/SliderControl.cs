using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.
using UnityEngine.Video;

public class SliderControl : EventTrigger,  ISelectHandler// required interface when using the OnSelect method.
{
    VideoPlayer
        m_Video;
    AudioSource
        m_AudioSource;
    Slider
        m_slider;
    float
       m_flengthVideo;

    enum STATE_SLIDER
    {
        WAITING,
        NOT_SELECTED,
        SELECTED
    }

    STATE_SLIDER
        m_ecurrentState = STATE_SLIDER.WAITING;

    public void Awake()
    {
        m_slider = GetComponent<Slider>();
    }

    public void Initialize(VideoPlayer l_videoPlayer, AudioSource l_audioSource)
    {
        m_Video = l_videoPlayer;
        m_AudioSource = l_audioSource;

        m_flengthVideo = m_Video.frameCount / m_Video.frameRate;
        m_ecurrentState = STATE_SLIDER.NOT_SELECTED;
    }

    //Do this when the selectable UI object is selected.
    public override void OnPointerDown(PointerEventData data)
    {
        m_ecurrentState = STATE_SLIDER.SELECTED;
    }

    public override void OnPointerUp(PointerEventData data)
    {
        m_ecurrentState = STATE_SLIDER.NOT_SELECTED;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        switch (m_ecurrentState)
        {
            case STATE_SLIDER.NOT_SELECTED:
                {
                    m_slider.value = (float)(m_Video.time / m_flengthVideo);
                }
                break;
            case STATE_SLIDER.SELECTED:
                {
                    m_Video.time = m_slider.value * m_flengthVideo;
                }
                break;
        }
    }
}
