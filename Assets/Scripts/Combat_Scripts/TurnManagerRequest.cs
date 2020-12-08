using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManagerRequest : MonoBehaviour
{
    /// <summary>
    /// List sorted by the velocity of the attacker.
    /// </summary>
    private SortedList<int, Request> requests = new SortedList<int, Request>();

    public static TurnManagerRequest instance;

    private Types_Matrix types;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);
        }
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

        requests.Add(newRequest.m_Attacker.Velocity, newRequest);
    }
    

    /// <summary>
    /// Calculate the result of the attacks.
    /// </summary>
    public void StartAttacks()
    {
        foreach (var request in requests)
        {
            OnAttack(request.Value.m_Attack, request.Value.m_Defender, request.Value.m_Attacker);
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
        float result = CalculateDamagedBasedOnTheMatrixType(attack, defending);
        switch (result)
        {
            case 0:
                Debug.Log("It doesn't affect " + defending.Name + ".");
                break;
            case 0.5f:
                Debug.Log("It's not very effective...");
                defending.Health -= CalculateDamageGiven(attacking, defending, attack, result);
                break;
            case 2:
                Debug.Log("It's super effective!");
                defending.Health -= CalculateDamageGiven(attacking, defending, attack, result);
                break;
            default:
                Debug.Log("It's effective.");
                defending.Health -= CalculateDamageGiven(attacking, defending, attack, result);
                break;
        }
    }

    private float CalculateDamagedBasedOnTheMatrixType(Attack attack, Pokemon defender)
    {
        float result = 1;
        foreach (Type typeDefender in defender.Type)
        {
            result *= types.types_Matrix[(int)attack.Type, (int)typeDefender];
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
    private int CalculateDamageGiven(Pokemon Attacking, Pokemon Defending, Attack attack, float multiplier)
    {
        int damage = 0;
        int attackDamageBase = attack.Damage;
        int attack_defense = Attacking.Attack / Defending.Defense;
        damage = (int)((((attackDamageBase * attack_defense) / 50) + 2) * multiplier);
        return damage;
    }
    #endregion
}
