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


    // Start is called before the first frame update
    void Start()
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
            attacks[i].GetComponent<Image>().color = colorsType[(int)player.activePokemon.m_attacks[i].type];
            UpdateAttacksChildren(i);
        }
    }

    private void UpdateAttacksChildren(int id)
    {
        Image[] childs = attacks[id].GetComponentsInChildren<Image>();
        TextMeshProUGUI name = attacks[id].GetComponentInChildren<TextMeshProUGUI>();

        childs[0].color = colorsType[(int)player.activePokemon.m_attacks[id].type];
        childs[1].sprite = player.activePokemon.m_attacks[id].typeSprite;
        name.text = player.activePokemon.m_attacks[id].name;
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

        imageChilds[0].sprite = attack.typeSprite;
        imageChilds[1].sprite = attack.categorySprite;

        textChilds[0].text = attack.type.ToString();
        textChilds[1].text = attack.power.ToString();
        textChilds[2].text = attack.name;
        textChilds[3].text = attack.description;

    }
}
