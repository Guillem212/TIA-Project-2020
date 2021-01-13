using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Pokemon[] pokemons;
    public Pokemon activePokemon;

    public string actualVoiceCommand;

    private GameObject pokemonObject;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private Pokemon GetPokemonInVoiceCommand()
    {
        List<string> words = new List<string>();

        string characters = "";
        foreach (char c in actualVoiceCommand)
        {
            if(c == ' ')
            {
                words.Add(characters);
            }
            else
            {
                characters += c;
            }
        }

        foreach (Pokemon p in pokemons)
        {
            foreach (string w in words)
            {
                if(p.name == w)
                {
                    return p;
                }
            }
        }

        //Si no devuelve nada pues estamos jodidos.
        return null;
    }

    private Attack GetAttackInVoiceCommand()
    {
        List<string> words = new List<string>();

        string characters = "";
        foreach (char c in actualVoiceCommand)
        {
            if (c == ' ')
            {
                words.Add(characters);
            }
            else
            {
                characters += c;
            }
        }

        foreach (Attack a in activePokemon.m_attacks)
        {
            foreach (string w in words)
            {
                if (a.name == w)
                {
                    return a;
                }
            }
        }

        //Si no devuelve nada pues estamos jodidos.
        return null;
    }

    /// <summary>
    /// Active the pokemon specified.
    /// </summary>
    /// <param name="id"></param>
    public void ActivatePokemon(int id)
    {
        activePokemon = pokemons[id];
        //Falta cambiar el transform.position por el transform de la carta. Aunque al final el player puede ser la carta en si.
        pokemonObject = Instantiate(activePokemon.model, transform.position, Quaternion.identity);
    }

    /// <summary>
    /// Use the attack selected in the game and try to attack the opponent or use the abillity with itself.
    /// </summary>
    /// <param name="attack"> The attack used.</param>
    /// <param name="objective"> The objective, can be itself.</param>
    /// <param name="attacking">The pokemon that is attacking.</param>
    public void Attack(Attack attack, Pokemon objective)
    {
        switch (attack.category)
        {
            case Category.PHYSICAL:
                Debug.Log(activePokemon.name + " use " + attack.name + " against " + objective.name + ".");
                TurnManagerRequest.instance.RequestAttack(this.activePokemon, objective, attack);
                break;
            case Category.SPECIAL:
                Debug.Log(activePokemon.name + " use " + attack.name + " against " + objective.name + ".");
                TurnManagerRequest.instance.RequestAttack(this.activePokemon, objective, attack);
                break;
            default:
                Debug.Log(activePokemon.name + " has improved his " + attack.statusModified + " using " + attack.name + ".");
                //
                break;
        }
    }
}
