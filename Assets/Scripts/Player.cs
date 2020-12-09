using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Pokemon[] pokemons;
    public Pokemon activePokemon;

    private GameObject pokemonObject;

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
