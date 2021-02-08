using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Player : MonoBehaviourPun
{
    [HideInInspector]public Pokemon[] pokemons;
    [HideInInspector]public PokStruct[] pokemonsStruct;


    public Pokemon activePokemon;
    public PokStruct s_activePokemon;
    public Attack selectedAttack = null;
    public Pokemon ObjectiveActivePokemon;
    public PokStruct s_ObjectiveActivePokemon;

    private GameObject pokemonModel;
    //private GameObject pokemonModel_2;

    [HideInInspector] public VoiceController voiceController;

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
            switch (GameManager.instance.turn){
                case 1:
                    activePokemon = GetPokemonInVoiceCommand();
                    if(activePokemon != null)
                    {
                        activePokemon.player_id = GameManager.instance.player_id;
                        view.RPC("ClearPokemon", RpcTarget.All, GameManager.instance.player_id);
                        view.RPC("ActivatePokemon", RpcTarget.All, GameManager.instance.player_id, activePokemon.name);
                    }
                    voiceController.speech = "";
                    break;
                case 2:
                    selectedAttack = GetAttackInVoiceCommand(activePokemon);
                    voiceController.speech = "";
                    break;
                default:
                    break;
            }

        }
    }

    private void generateArrayListStruct(Pokemon[] pokemonsObjects)
    {
        List<PokStruct> aux = new List<PokStruct>();
        foreach(Pokemon p in pokemonsObjects)
        {
            PokStruct poke = new PokStruct(p.name, p.hp, p.attack, p.specialAttack, p.defense, p.specialDefense, p.velocity);
            aux.Add(poke);
        }
        pokemonsStruct = aux.ToArray();
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
                s_activePokemon = new PokStruct(p.name, p.hp, p.attack, p.specialAttack, p.defense, p.specialDefense, p.velocity);
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
            string attackName = a.name;
            string speechAux = VoiceController.instance.speech;
            if (speechAux.ToLower().Contains(attackName.ToLower()))
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
        TurnManagerRequest.instance.view.RPC("DebugTest", RpcTarget.All, pokContainer != null);

        foreach (var p in pokContainer)
        {
            if(p.name == pokemonCalled)
            {
                p.SetActive(true);
                if(player_id == GameManager.instance.player_id) pokemonModel = p;
                else
                {
                    foreach(var pok in pokemons)
                    {
                        if(pok.name.Equals(p.name)) 
                        {
                            ObjectiveActivePokemon = pok;
                            ObjectiveActivePokemon.player_id = GameManager.instance.player_id == 0? 1 : 0;
                            s_ObjectiveActivePokemon = new PokStruct(pok.name, pok.hp, pok.attack, pok.specialAttack, pok.defense, pok.specialDefense, pok.velocity);
                            break;
                        }
                    }   
                }
                break;
            }
        }
    }

    [PunRPC]
    private void ClearPokemon(int player_id)
    {
        GameObject[] pokContainer = GameObject.Find(player_id == 0 ? "Pokemon_1" : "Pokemon_2").GetComponent<PokemonContainerScript>().pokemons;
        //TurnManagerRequest.instance.view.RPC("DebugTest", RpcTarget.All, pokContainer != null);

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
        if(attack is null)
        {
            attack = activePokemon.m_attacks[Random.Range(0, activePokemon.m_attacks.Length)];
        }
        switch (attack.objective)
        {
            case Objective.USER:
                TurnManagerRequest.instance.view.RPC("RequestAttack", RpcTarget.All, GameManager.instance.player_id, GameManager.instance.player_id, attack.name);
                break;
            default:
                TurnManagerRequest.instance.view.RPC("RequestAttack", RpcTarget.All, GameManager.instance.player_id, GameManager.instance.player_id == 0? 1 : 0, attack.name);
                break;
        }
    }

}
