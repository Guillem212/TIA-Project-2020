using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AttackButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private float timeElapsed;
    private int timeMaxToPress = 1;

    private bool isPressed, wasPressed;

    public GameObject attackInformation;
    public PlayerCanvasManager canvasManager;

    private Player player;

    [HideInInspector] public Attack attack;

    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        timeElapsed = 0;
        isPressed = false;
        wasPressed = false;
    }

    void Update()
    {
        if (isPressed)
        {
            timeElapsed += Time.deltaTime;

            if(timeElapsed >= timeMaxToPress && !attackInformation.activeInHierarchy)
            {
                canvasManager.UpdateAttackInformation(attack);
                attackInformation.SetActive(true);
            }
        }
        else if(wasPressed)
        {
            if(timeElapsed < timeMaxToPress)
            {
                VoiceController.instance.speech = attack.name;
            }
            else
            {
                attackInformation.SetActive(false);
            }
            timeElapsed = 0;
            wasPressed = false;
        }

        checkCurrentActiveAttack();
    }

    private void checkCurrentActiveAttack(){
        if(player.selectedAttack.name.Equals(attack.name)){
            if(GetComponent<Image>().color.a != 0.9f)
                GetComponent<Image>().color = new Color( GetComponent<Image>().color.r,  GetComponent<Image>().color.g,  GetComponent<Image>().color.b, 0.9f);
        }
        else if(GetComponent<Image>().color.a != 0.5f){
            GetComponent<Image>().color = new Color( GetComponent<Image>().color.r,  GetComponent<Image>().color.g,  GetComponent<Image>().color.b, 0.5f);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPressed = true;

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPressed = false;
        wasPressed = true;
    }
}
