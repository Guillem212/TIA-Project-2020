using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pokemon_Script : MonoBehaviour
{
    public Pokemon pokemon;

    private GameObject pokemonModel;

    // Start is called before the first frame update
    void Start()
    {
        pokemonModel = Instantiate(pokemon.Model, transform.position, Quaternion.identity);
    }

    /// <summary>
    /// Use the attack selected in the game and try to attack the opponent or use the abillity with itself.
    /// </summary>
    /// <param name="attack"> The attack used.</param>
    /// <param name="objective"> The objective, can be itself.</param>
    /// <param name="attacking">The pokemon that is attacking.</param>
    public void Attack(Attack attack, Pokemon_Script objective){
            switch (attack.TypeOfAttack)
            {
                case Type_of_Attack.OFFENSIVE:
                    Debug.Log(pokemon.Name + " use " + attack.Name + " against " + objective.pokemon.Name + ".");
                    TurnManagerRequest.instance.RequestAttack(this.pokemon, objective.pokemon, attack);
                    break;
                case Type_of_Attack.DEFENSIVE:
                    Debug.Log(pokemon.Name + " use " + attack.Name + ".");
                //
                    break;
                default:
                    Debug.Log(pokemon.Name + " has improved his " + attack.StatModified + " using " + attack.Name + ".");
                //
                    break;
            }
    }
}
