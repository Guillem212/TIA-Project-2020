using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private float timeElapsed;
    private int timeMaxToPress = 1;

    private bool isPressed, wasPressed;

    public GameObject attackInformation;
    public PlayerCanvasManager canvasManager;

    private Player player;

    [HideInInspector] public Attack attack;

    private void Start()
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
                player.selectedAttack = attack;
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
        if(player.selectedAttack == attack){
            GetComponent<Animator>().SetTrigger("Selected");
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
