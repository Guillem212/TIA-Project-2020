using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManagerRequest : MonoBehaviour
{
    /// <summary>
    /// List sorted by the velocity of the attacker.
    /// </summary>
    private SortedList<int, Request> requests;
    public static TurnManagerRequest instance;

    private Types_Matrix types;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(instance);
        }

        requests = new SortedList<int, Request>();
    }

    private void Start()
    {
        types = new Types_Matrix();
    }

    #region PUBLIC METHODS

    /// <summary>
    /// Add the request of attacking to the queue of attacks, ordered by the velocity of the attacker.
    /// </summary>
    /// <param name="attacker"></param>
    /// <param name="defender"></param>
    /// <param name="attack"></param>
    public void RequestAttack(Pokemon attacker, Pokemon defender, Attack attack)
    {
        Request newRequest = new Request();
        newRequest.m_Attack = attack;
        newRequest.m_Attacker = attacker;
        newRequest.m_Defender = defender;

        requests.Add(newRequest.m_Attacker.velocity + attack.priority, newRequest);
    }
    

    /// <summary>
    /// Calculate the result of the attacks at the end of the decision stage.
    /// </summary>
    public void StartAttacks()
    {
        foreach (var request in requests)
        {
            switch (request.Value.m_Attack.category)
            {
                case Category.PHYSICAL:
                    OnAttack(request.Value.m_Attack, request.Value.m_Defender, request.Value.m_Attacker);
                    break;
                case Category.SPECIAL:
                    OnAttack(request.Value.m_Attack, request.Value.m_Defender, request.Value.m_Attacker);
                    break;
                case Category.STATUS:
                    OnModifiedStatus(request.Value.m_Attack, request.Value.m_Defender);
                    break;
                default:
                    break;
            }
        }
    }
    #endregion

    #region PRIVATE METHODS

    /// <summary>
    /// Calculate the result of the attack.
    /// </summary>
    /// <param name="attack"> Attack used.</param>
    /// <param name="defending">The defending Pokemon that recieves the attack.</param>
    /// <param name="attacking">The attacking Pokemon that used the attack.</param>
    /// <returns>Returns the value of life that the attack has caused to the objective. If this value is 0, that means that the attack was unsuccessful.</returns>
    private void OnAttack(Attack attack, Pokemon defending, Pokemon attacking)
    {
        //**TO DO**: Callback at finish OnAttack to the original defensorPokemon
        float result = CalculateDamagedBasedOnTheMatrixtype(attack, defending);
        switch (result)
        {
            case 0:
                Debug.Log("It doesn't affect " + defending.name + ".");
                break;
            case 0.5f:
                Debug.Log("It's not very effective...");
                defending.hp -= CalculateDamageGiven(attacking, defending, attack, result);
                break;
            case 2:
                Debug.Log("It's super effective!");
                defending.hp -= CalculateDamageGiven(attacking, defending, attack, result);
                break;
            default:
                Debug.Log("It's effective.");
                defending.hp -= CalculateDamageGiven(attacking, defending, attack, result);
                break;
        }
    }

    /// <summary>
    /// Calculate the result of the status modified.
    /// </summary>
    /// <param name="attack"></param>
    /// <param name="objective"></param>
    private void OnModifiedStatus(Attack attack, Pokemon objective)
    {
        switch (attack.statusModified)
        {
            case StatusModified.HP:
                break;
            case StatusModified.ATTACK:
                objective.attack += (int)Mathf.Lerp(0, objective.attack, attack.power);
                break;
            case StatusModified.SATTACK:
                objective.specialAttack += (int)Mathf.Lerp(0, objective.specialAttack, attack.power);
                break;
            case StatusModified.DEFENSE:
                objective.defense += (int)Mathf.Lerp(0, objective.defense, attack.power);
                break;
            case StatusModified.SDEFENSE:
                objective.specialDefense += (int)Mathf.Lerp(0, objective.specialDefense, attack.power);
                break;
            case StatusModified.VELOCITY:
                objective.velocity += (int)Mathf.Lerp(0, objective.velocity, attack.power);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Calculate the type modifier based on the Types Table.
    /// </summary>
    /// <param name="attack">Attack used.</param>
    /// <param name="defender">Pokemon that is being attacked.</param>
    /// <returns></returns>
    private float CalculateDamagedBasedOnTheMatrixtype(Attack attack, Pokemon defender)
    {
        float result = 1;
        foreach (Type typeDefender in defender.types)
        {
            result *= types.types_Matrix[(int)attack.type, (int)typeDefender];
        }
        return result;
    }

    /// <summary>
    /// Calculate the damage with a simplyfication of this function: https://bulbapedia.bulbagarden.net/wiki/Damage.
    /// </summary>
    /// <param name="Attacking">Attacking Pokemon.</param>
    /// <param name="Defending">Defending Pokemon.</param>
    /// <param name="attack">Attack used.</param>
    /// <param name="multiplier"></param>
    /// <returns>The damage value</returns>
    private int CalculateDamageGiven(Pokemon Attacking, Pokemon Defending, Attack attack, float attackType)
    {
        int attackDamageBase = attack.power;
        float attack_defense = attack.category == Category.PHYSICAL ? Attacking.attack / Defending.defense : Attacking.specialAttack / Defending.specialDefense;
        float level = ((Attacking.level * 2) / 5) + 2;

        float critical = Random.Range(1, 8) == 1 ? 1.5f : 1f;
        float random = Random.Range(0.85f, 1f);
        float STAB = CalculateSTAB(Defending, attack);
        float modifier = attackType * critical * random * STAB;

        return (int)(((level * attackDamageBase * attack_defense / 50) + 2) * modifier);
    }

    /// <summary>
    /// Calculate the STAB --> Same-Type Attack Bonus.
    /// </summary>
    /// <param name="Defending"> Pokemon that is being attacked.</param>
    /// <param name="attack">The Attack used.</param>
    /// <returns></returns>
    private float CalculateSTAB(Pokemon Defending, Attack attack)
    {
        foreach (Type type in Defending.types)
        {
            if (attack.type == type)
            {
                return 1.5f;
            }
        }

        return 1f;
    }
    #endregion
}
