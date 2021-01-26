using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpdatePokemonInformationCanvas : MonoBehaviour
{
    private Player player;
    public GameObject pokemonName,
    type,
    hp,
    attack,
    specialAttack,
    defense,
    specialDefense,
    velocity;

    public Image panel;

    public Image[] attacksImage;
    public TextMeshProUGUI[] attacksName;

    private Camera cam;

    private void OnEnable() {

        //Con el nombre del pokemon padre coger de recursos el scriptable object del pokemon
        player = FindObjectOfType<Player>();

        cam = Camera.main;

        GetComponent<Canvas>().worldCamera = cam;
        
        if(player.activePokemon is null)
            return;
        
        panel.color = player.activePokemon.typeColor;
        pokemonName.GetComponent<TextMeshProUGUI>().text = player.activePokemon.name;
        type.GetComponent<Image>().sprite = player.activePokemon.typeSprite;
        hp.GetComponent<TextMeshProUGUI>().text = "HP - " + player.activePokemon.hp;
        attack.GetComponent<TextMeshProUGUI>().text = "ATTACK - " + player.activePokemon.attack;
        specialAttack.GetComponent<TextMeshProUGUI>().text = "SPECIAL ATTACK - " + player.activePokemon.specialAttack;
        defense.GetComponent<TextMeshProUGUI>().text = "DEFENSE - " + player.activePokemon.defense;
        specialDefense.GetComponent<TextMeshProUGUI>().text = "SPECIAL DEFENSE - " + player.activePokemon.specialDefense;
        velocity.GetComponent<TextMeshProUGUI>().text = "VELOCITY - " + player.activePokemon.velocity;

        for (int i = 0; i < attacksImage.Length; i++)
        {
            attacksImage[i].sprite = player.activePokemon.m_attacks[i].typeSprite;
            attacksName[i].text = player.activePokemon.m_attacks[i].name;
        }
    }

    private void LateUpdate() {
        transform.LookAt(cam.transform.position, Vector2.up);
    }

}
