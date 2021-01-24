using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private float timeElapsed;
    private const int timeMaxToPress = 1;

    private bool isPressed;

    public GameObject attackInformation;
    public PlayerCanvasManager canvasManager;

    private Player player;

    [HideInInspector] public Attack attack;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
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
        else
        {
            if(timeElapsed < timeMaxToPress)
            {
                //Mandar el ataque seleccionado
                player.selectedAttack = attack;
            }
            timeElapsed = 0;
            attackInformation.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPressed = true;

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPressed = false;
    }
}
