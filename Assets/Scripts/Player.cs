using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private Pokemon[] pokemons;

    public Pokemon activePokemon;
    private Pokemon ObjectiveActivePokemon;

    private GameObject pokemonObject;

    private VoiceController voiceController;

    private void Awake()
    {
        pokemons = Resources.LoadAll<Pokemon>("Pokemons");
        voiceController = FindObjectOfType<VoiceController>();
    }

    private void Update()
    {
        if(voiceController.speech != "")
        {
            activePokemon = GetPokemonInVoiceCommand();
            if(activePokemon != null)
            {
                ClearPokemon();
                ActivatePokemon();
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
    public void ActivatePokemon()
    {
        Transform[] t = GameObject.Find("Pokemon_1").GetComponentsInChildren<Transform>();
        
        foreach (var item in t)
        {
            if(item.name == activePokemon.name)
            {
                SkinnedMeshRenderer[] skinned = item.GetComponentsInChildren<SkinnedMeshRenderer>();
                foreach (var s in skinned)
                {
                    s.enabled = true;
                }
            }
        }
        pokemonObject = activePokemon.model;
    }

    public void ClearPokemon()
    {
        Transform[] t = GameObject.Find("Pokemon_1").GetComponentsInChildren<Transform>();

        foreach (var item in t)
        {
            SkinnedMeshRenderer[] skinned = item.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (var s in skinned)
            {
                s.enabled = false;
            }
        }
        pokemonObject = null;
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
                TurnManagerRequest.instance.RequestAttack(activePokemon, activePokemon, attack);
                break;
            default:
                TurnManagerRequest.instance.RequestAttack(activePokemon, ObjectiveActivePokemon, attack);
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
