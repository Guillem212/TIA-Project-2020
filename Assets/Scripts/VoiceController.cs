using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TextSpeech;
using UnityEngine.Android;
using UnityEngine.UI;

public class VoiceController : MonoBehaviour
{
    const string LANG_CODE = "en-US";

    public Text uiText;

    private void Start()
    {
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

    public void OnClickSpeak()
    {
        TextToSpeech.instance.StartSpeak(uiText.text);
    }
    public void OnClickStopSpeak()
    {
        TextToSpeech.instance.StopSpeak();
    }

    private void OnFinalSpeechResult(string result)
    {
        uiText.text = "Holaquetal";
        uiText.text = result;
    }

    private void OnPartialSpeechResult(string result)
    {
        uiText.text = result;
    }

    #endregion

    private void Setup(string code)
    {
        SpeechToText.instance.Setting(code);
        TextToSpeech.instance.Setting(code, 1, 1);
    }
}
