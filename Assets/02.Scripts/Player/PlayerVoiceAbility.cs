using Photon.Voice.PUN;
using Photon.Voice.Unity;
using UnityEngine;

public class PlayerVoiceAbility : PlayerAbility
{
    [SerializeField] private GameObject _speakingIcon;
    [SerializeField] private PhotonVoiceView _voiceView;

    private Recorder _recorder;

    private float _voiceDetectionThreshold = 0.01f;
    private int _voiceDetectionDelayMs = 300;

    private void Start()
    {
        _recorder = FindAnyObjectByType<Recorder>();

        _recorder.VoiceDetection = true;
        _recorder.VoiceDetectionThreshold = _voiceDetectionThreshold;
        _recorder.VoiceDetectionDelayMs = _voiceDetectionDelayMs;
    }

    private void Update()
    {
        bool isSpeaking = false;

        if (_owner.PhotonView.IsMine)
        {
            isSpeaking = _recorder.IsCurrentlyTransmitting;
            // isSpeaking = _voiceView.IsRecording;
        }
        else
        {
            isSpeaking = _voiceView.IsSpeaking;
        }
        _speakingIcon.gameObject.SetActive(isSpeaking);

        if (Input.GetKeyDown(KeyCode.N))
        {
            // 채팅 음소거를 한다.
            _recorder.TransmitEnabled = !_recorder.TransmitEnabled;
        }
    }
}
