using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Player : MonoBehaviourPun
{
    private Pokemon[] pokemons;
    public Pokemon activePokemon;
    //public Attack selectedAttack = null;
    private Pokemon ObjectiveActivePokemon;

    private GameObject pokemonModel;
    //private GameObject pokemonModel_2;

    private VoiceController voiceController;

    [HideInInspector] public PhotonView view;

    private void Awake()
    {
        pokemons = Resources.LoadAll<Pokemon>("Pokemons");
        voiceController = GetComponent<VoiceController>();
        view = PhotonView.Get(this);
    }

    private void Update()
    {
        if(voiceController.speech != "")
        {
            activePokemon = GetPokemonInVoiceCommand();
            if(activePokemon != null)
            {
                view.RPC("ClearPokemon", RpcTarget.All, GameManager.instance.player_id);
                view.RPC("ActivatePokemon", RpcTarget.All, GameManager.instance.player_id, activePokemon.name);
            }
            voiceController.speech = "";
        }
    }

    /// <summary>
    /// Get the pokemon selected through voice command
    /// </summary>
    /// <returns></returns>
    private Pokemon GetPokemonInVoiceCommand()
    {
        foreach (Pokemon p in pokemons)
        {
            if (VoiceController.instance.speech.Contains(p.name))
            {
                print(p.name);
                return p;
            }
        }
        return null;
    }


    /// <summary>
    /// Get the attack selected through voice command
    /// </summary>
    /// <param name="actualPokemon"></param>
    /// <returns></returns>
    private Attack GetAttackInVoiceCommand(Pokemon actualPokemon)
    {
        foreach (Attack a in actualPokemon.m_attacks)
        {
            if (VoiceController.instance.speech.Contains(a.name))
                return a;
        }
        return null;
    }

    /// <summary>
    /// Active the pokemon specified.
    /// </summary>
    /// <param name="id"></param>
    [PunRPC]
    private void ActivatePokemon(int player_id, string pokemonCalled)
    {
        GameObject[] pokContainer = GameObject.Find(player_id == 0 ? "Pokemon_1" : "Pokemon_2").GetComponent<PokemonContainerScript>().pokemons;
        
        foreach (var p in pokContainer)
        {
            if(p.name == pokemonCalled)
            {
                p.SetActive(true);
                if(player_id == GameManager.instance.player_id) pokemonModel = p;
                break;
            }
        }
    }

    [PunRPC]
    private void ClearPokemon(int player_id)
    {
        GameObject[] pokContainer = GameObject.Find(player_id == 0 ? "Pokemon_1" : "Pokemon_2").GetComponent<PokemonContainerScript>().pokemons;

        foreach (var p in pokContainer)
        {
            p.SetActive(false);
        }
        pokemonModel = null;
    }

    /// <summary>
    /// Use the attack selected in the game and try to attack the opponent or use the abillity with itself.
    /// </summary>
    /// <param name="attack"> The attack used.</param>
    /// <param name="objective"> The objective, can be itself.</param>
    /// <param name="attacking">The pokemon that is attacking.</param>
    public void Attack(Attack attack)
    {
        switch (attack.objective)
        {
            case Objective.USER:
                TurnManagerRequest.instance.view.RPC("RequestAttack", RpcTarget.All, activePokemon, activePokemon, attack);
                break;
            default:
                TurnManagerRequest.instance.view.RPC("RequestAttack", RpcTarget.All, activePokemon, ObjectiveActivePokemon, attack);
                break;
        }

        //DEBUG
        switch (attack.category)
        {
            case Category.PHYSICAL:
                Debug.Log(activePokemon.name + " use " + ".");
                break;
            case Category.SPECIAL:
                Debug.Log(activePokemon.name + " use " + ".");
                break;
            default:
                Debug.Log(activePokemon.name + " has improved his " + attack.statusModified + " using " + attack.name + ".");
                break;
        }
    }

}
