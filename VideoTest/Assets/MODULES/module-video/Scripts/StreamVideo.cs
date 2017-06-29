using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System;




public class StreamVideo : MonoBehaviour
{
    public static event Action OnStreamVideoEndReached;

    public RawImage image;
    public GameObject playIcon;

    public string
        m_sUrl;

    public VideoClip videoToPlay;

    private VideoPlayer videoPlayer;

    private AudioSource audioSource;

    private bool firstRun = true;

    void Awake()
    {
        image.uvRect = new Rect(0, 0, 1, 1);
        image.CrossFadeAlpha(0.0f, 0.0f, false);
    }

    void Start()
    {
        LoadVideo();
    }

    void InitializeVideo()
    {
        firstRun = false;
        //Add VideoPlayer to the GameObject
        videoPlayer = gameObject.AddComponent<VideoPlayer>();

        //Add AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();

        //Disable Play on Awake for both Video and Audio
        videoPlayer.playOnAwake = false;
        audioSource.playOnAwake = false;
        audioSource.Pause();

        // Vide clip from Url
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = "https://s3.eu-central-1.amazonaws.com/admented/Alliance/2017-05/videos/agora.mp4";
        videoPlayer.isLooping = false;

        //Set Audio Output to AudioSource
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;

        //Assign the Audio from Video to AudioSource to be played
        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.SetTargetAudioSource(0, audioSource);

        // :: EVENT
        videoPlayer.prepareCompleted += Prepared;
        videoPlayer.loopPointReached += EndReached;
    }

    void PlayVideo()
    {
        videoPlayer.Play();
        audioSource.Play();
        playIcon.SetActive(false);
    }

    void PauseVideo()
    {
        videoPlayer.Pause();
        audioSource.Pause();
        playIcon.SetActive(true);
    }

    void LoadVideo()
    {
        InitializeVideo();
        //Set video To Play then prepare Audio to prevent Buffering
        videoPlayer.Prepare();

        //Wait until video is prepared
    }

    void EndReached(VideoPlayer l_vplayer)
    {
        Debug.Log("End Reached ");
        if ( !videoPlayer.isLooping )
            image.CrossFadeAlpha(0.0f, 0.5f, false);
    }

    void Prepared(UnityEngine.Video.VideoPlayer vPlayer)
    {
        //Assign the Texture from Video to RawImage to be displayed
        SliderControl l_sliderControl = GetComponentInChildren<SliderControl>();
        if (l_sliderControl != null )
            l_sliderControl.Initialize(videoPlayer, audioSource);

        image.texture = videoPlayer.texture;
        image.CrossFadeAlpha(1.0f, 1.5f, false);

        PlayVideo();
    }
}