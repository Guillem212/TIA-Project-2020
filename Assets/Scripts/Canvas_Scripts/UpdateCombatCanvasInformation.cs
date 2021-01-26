using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
public class UpdateCombatCanvasInformation : MonoBehaviour
{
    // Start is called before the first frame update

    [HideInInspector] public Pokemon pokemon;

    public GameObject panel, pokemonImage, PokemonName, imageType, userName;

    public void StartHacerCosas() {
        if(GameManager.instance.player_id == 0 && gameObject.name.Equals("Pokemon_1_Canvas"))
        {
            pokemon = GameManager.instance.player.GetComponent<Player>().activePokemon;
            GameObject.Find("Pokemon_2_Canvas").GetComponent<UpdateCombatCanvasInformation>().pokemon = GameManager.instance.player.GetComponent<Player>().ObjectiveActivePokemon;

        }
        else if(GameManager.instance.player_id != 0 && gameObject.name.Equals("Pokemon_2_Canvas"))
        {
            pokemon = GameManager.instance.player.GetComponent<Player>().activePokemon;
            GameObject.Find("Pokemon_1_Canvas").GetComponent<UpdateCombatCanvasInformation>().pokemon = GameManager.instance.player.GetComponent<Player>().ObjectiveActivePokemon;
        }

        panel.SetActive(true);

        panel.GetComponent<Image>().color = pokemon.typeColor;
        pokemonImage.GetComponent<Image>().sprite = pokemon.pokemonImage;

        PokemonName.GetComponent<TextMeshProUGUI>().text = pokemon.name;
        imageType.GetComponent<Image>().sprite = pokemon.typeSprite;
        userName.GetComponent<TextMeshProUGUI>().text = PhotonNetwork.NickName;
    }
}
