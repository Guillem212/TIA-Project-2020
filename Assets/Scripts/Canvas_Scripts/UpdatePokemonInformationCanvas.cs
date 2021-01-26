using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpdatePokemonInformationCanvas : MonoBehaviour
{
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
        string father = GetComponentInParent<BoxCollider>().gameObject.name;

        Pokemon[] pokemons = Resources.LoadAll<Pokemon>("Resources/Pokemons");

        Pokemon SelectedPokemon = new Pokemon();

        foreach (var item in pokemons)
        {
            if(item.name.Equals(father)){
                SelectedPokemon = item;
                break;
            }
        }

        cam = Camera.main;

        GetComponent<Canvas>().worldCamera = cam;
        
        panel.color = SelectedPokemon.typeColor;
        pokemonName.GetComponent<TextMeshProUGUI>().text = SelectedPokemon.name;
        type.GetComponent<Image>().sprite = SelectedPokemon.typeSprite;
        hp.GetComponent<TextMeshProUGUI>().text = "HP - " + SelectedPokemon.hp;
        attack.GetComponent<TextMeshProUGUI>().text = "ATTACK - " + SelectedPokemon.attack;
        specialAttack.GetComponent<TextMeshProUGUI>().text = "SPECIAL ATTACK - " + SelectedPokemon.specialAttack;
        defense.GetComponent<TextMeshProUGUI>().text = "DEFENSE - " + SelectedPokemon.defense;
        specialDefense.GetComponent<TextMeshProUGUI>().text = "SPECIAL DEFENSE - " + SelectedPokemon.specialDefense;
        velocity.GetComponent<TextMeshProUGUI>().text = "VELOCITY - " + SelectedPokemon.velocity;

        for (int i = 0; i < attacksImage.Length; i++)
        {
            attacksImage[i].sprite = SelectedPokemon.m_attacks[i].typeSprite;
            attacksName[i].text = SelectedPokemon.m_attacks[i].name;
        }
    }

    private void LateUpdate() {
        transform.LookAt(cam.transform.position, Vector2.up);
    }

}
