using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TextSpeech;
using UnityEngine.Android;
using TMPro;

public class VoiceController : MonoBehaviour
{
    const string LANG_CODE = "en-US";

    //DEBUG
    public TextMeshProUGUI uiText;

    private Player player;

    private void Start()
    {
        player = GetComponent<Player>();
        Setup(LANG_CODE);
#if UNITY_ANDROID
        SpeechToText.instance.onPartialResultsCallback = OnPartialSpeechResult;
#endif
        SpeechToText.instance.onResultCallback = OnFinalSpeechResult;

        CheckPermission();
    }

    private void CheckPermission()
    {
#if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }
#endif
    }

    #region Speech to Text

    public void StartListening()
    {
        SpeechToText.instance.StartRecording("Speak any");
    }

    public void StopListening()
    {
        SpeechToText.instance.StopRecording();
    }

    private void OnFinalSpeechResult(string result)
    {
        player.actualVoiceCommand = result;

        //DEBUG
        uiText.text = player.actualVoiceCommand;
    }

    private void OnPartialSpeechResult(string result)
    {
        player.actualVoiceCommand = result;
    }

    #endregion

    private void Setup(string code)
    {
        SpeechToText.instance.Setting(code);
    }
}
