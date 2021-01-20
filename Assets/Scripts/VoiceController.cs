using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TextSpeech;
using UnityEngine.Android;

public class VoiceController : MonoBehaviour
{
    const string LANG_CODE = "en-EU";

    public string speech;

    public static VoiceController instance;

    private void Awake()
    {
        if(instance is null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
        speech = "";
    }

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

    private void OnFinalSpeechResult(string result)
    {
        speech = result;
    }

    private void OnPartialSpeechResult(string result)
    {
        speech = result;
    }

    #endregion

    private void Setup(string code)
    {
        SpeechToText.instance.Setting(code);
    }

    private void OnGUI()
    {
        Rect r = new Rect(new Vector2(100, 100), Vector2.one * 100);
        GUI.TextArea(r, speech);
    }
}
