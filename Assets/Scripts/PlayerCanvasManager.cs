using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerCanvasManager : MonoBehaviour
{

    private Player player;

    private Sprite[] pokemonImages;
    
    private GameObject scrollViewContent;

    public GameObject pokemon;


    public GameObject[] attacks;

    public Color[] colorsType;

    public GameObject attackInformation;

    private int aux = 0;

    public void InitializeCanvas()
    {
        player = FindObjectOfType<Player>();
        scrollViewContent = FindObjectOfType<ScrollRect>().content.gameObject;
        pokemonImages = new Sprite[player.pokemons.Length];

        for (int i = 0; i < pokemonImages.Length; i++)
        {
            pokemonImages[i] = player.pokemons[i].pokemonImage;
        }

        BuildScrollViewList();
    }

    private void BuildScrollViewList()
    {
        foreach (var p in pokemonImages)
        {
            pokemon.GetComponent<Image>().sprite = p;

            GameObject pokemonButton = Instantiate(pokemon, scrollViewContent.transform, false);

            pokemonButton.GetComponent<Button>().onClick.AddListener(delegate { SetPokemonInstance(p.name); });
        }
    }

    private void SetPokemonInstance(string name)
    {
        player.gameObject.GetComponent<VoiceController>().speech = name;
    }

    /// <summary>
    /// Updates the active attacks of the pokemon.
    /// </summary>
    public void UpdateAttacks()
    {
        for (int i = 0; i < attacks.Length; i++)
        {
            attacks[i].GetComponent<Image>().color = new Color(colorsType[(int)player.activePokemon.m_attacks[i].type].r, colorsType[(int)player.activePokemon.m_attacks[i].type].g, colorsType[(int)player.activePokemon.m_attacks[i].type].b, 0.4f);
            UpdateAttacksChildren(i);
        }
    }

    private void UpdateAttacksChildren(int id)
    {
        Image[] childs = attacks[id].GetComponentsInChildren<Image>();
        TextMeshProUGUI name = attacks[id].GetComponentInChildren<TextMeshProUGUI>();

        aux = childs.Length;

        childs[1].color = new Color(colorsType[(int)player.activePokemon.m_attacks[id].type].r, colorsType[(int)player.activePokemon.m_attacks[id].type].g, colorsType[(int)player.activePokemon.m_attacks[id].type].b, 0.8f);
        childs[2].sprite = player.activePokemon.m_attacks[id].typeSprite;
        name.text = player.activePokemon.m_attacks[id].name;

        attacks[id].GetComponent<AttackButtonScript>().attack = player.activePokemon.m_attacks[id];
    }


    /// <summary>
    /// Updates the information panel, in which the info of the selected attack is.
    /// </summary>
    /// <param name="attack">The attack that you selected</param>
    public void UpdateAttackInformation(Attack attack)
    {

        attackInformation.GetComponent<Image>().color = colorsType[(int)attack.type];

        Image[] imageChilds = attackInformation.GetComponentsInChildren<Image>();
        TextMeshProUGUI[] textChilds = attackInformation.GetComponentsInChildren<TextMeshProUGUI>();

        imageChilds[1].sprite = attack.typeSprite;
        imageChilds[2].sprite = attack.categorySprite;

        textChilds[0].text = attack.type.ToString();
        textChilds[1].text = attack.power.ToString();
        textChilds[2].text = attack.name;
        textChilds[3].text = attack.description;

    }
}
