using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClickVoiceHandler : MonoBehaviour, IPointerEnterHandler, IPointerUpHandler
{
    public VoiceController voiceController;
    private bool buttonDown, buttonUp;

    // Start is called before the first frame update
    void Start()
    {

        buttonDown = false;
        buttonUp = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!buttonDown)
        {
            if (!buttonUp)
            {
                return;
            }
            else
            {
                buttonUp = false;
                voiceController.StopListening();
            }
        }
        else
        {
            voiceController.StartListening();
            buttonDown = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonDown = false;
        buttonUp = true;
    }
}
