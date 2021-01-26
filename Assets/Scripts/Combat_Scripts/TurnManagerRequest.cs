using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TurnManagerRequest : MonoBehaviourPun
{
    #region Private Variables
    /// <summary>
    /// List sorted by the velocity of the attacker.
    /// </summary>
    private SortedList<int, Request> requests;
    private Types_Matrix types;

    private bool IsLastRequestFinished; 

    #endregion

    #region Public Variables
    public static TurnManagerRequest instance;
   [HideInInspector] public PhotonView view;

    #endregion
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
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
        view = PhotonView.Get(this);
    }

    #region RPC FUNCTIONS

    /// <summary>
    /// Add the request of attacking to the queue of attacks, ordered by the velocity of the attacker.
    /// </summary>
    /// <param name="attacker"></param>
    /// <param name="defender"></param>
    /// <param name="attack"></param>
    [PunRPC]
    public void RequestAttack(int attacker, int defender, string attack_name)
    {
        //if(!PhotonNetwork.IsMasterClient) return;
        Pokemon aux1 = GameManager.instance.player_id == attacker? GameManager.instance.player.GetComponent<Player>().activePokemon : GameManager.instance.player.GetComponent<Player>().ObjectiveActivePokemon; //The attacker
        Pokemon aux2 = GameManager.instance.player_id != attacker? GameManager.instance.player.GetComponent<Player>().activePokemon : GameManager.instance.player.GetComponent<Player>().ObjectiveActivePokemon; //The enemy of the attacker
        
        Request newRequest = new Request();
        newRequest.m_Attacker = aux1;
        newRequest.m_Defender = attacker == defender? aux1 : aux2;

        foreach(Attack a in newRequest.m_Attacker.m_attacks)
        {
            if(attack_name.Equals(a.name)) 
            {
                newRequest.m_Attack = a;
                break;
            }
        }

        requests.Add(newRequest.m_Attacker.velocity + newRequest.m_Attack.priority, newRequest);
    }

/// <summary>
/// Do damage for all the clients and refresfh the UI.
/// </summary>
/// <param name="player_id"></param>
/// <param name="damageReceived"></param>
/// <param name="effective"></param>
    [PunRPC]
    public void SendAttackResult(int player_id, int damageReceived, float effective){ 
        string attackedPokemon = "";
        if(player_id == GameManager.instance.player_id)
        {
            GameManager.instance.player.GetComponent<Player>().ObjectiveActivePokemon.hp -= damageReceived;
            if(GameManager.instance.player.GetComponent<Player>().ObjectiveActivePokemon.hp <= 0) GameManager.instance.view.RPC("theWinnerIs", RpcTarget.All, player_id);
            attackedPokemon = GameManager.instance.player.GetComponent<Player>().ObjectiveActivePokemon.name;
        }
        else
        {
            GameManager.instance.player.GetComponent<Player>().activePokemon.hp -= damageReceived;
            attackedPokemon = GameManager.instance.player.GetComponent<Player>().activePokemon.name;
        }
        GameManager.instance.RefresUIAttacksResult(attackedPokemon, effective);
    }

    [PunRPC]
    public void SendAttackStatusResult(string attack_name, bool toMyself, int attacker)
    {
        Attack AuxAttack = new Attack();
        Attack[] attacks = GameManager.instance.player_id == attacker? GameManager.instance.player.GetComponent<Player>().activePokemon.m_attacks : GameManager.instance.player.GetComponent<Player>().ObjectiveActivePokemon.m_attacks; //The attacker
        foreach(Attack a in attacks)
        {
            if(attack_name.Equals(a.name)) 
            {
                AuxAttack = a;
                break;
            }
        }
        #region Manage who's status is going to be modified
        if(attacker == GameManager.instance.player_id)
        {
            if(toMyself)
            {
                switch (AuxAttack.statusModified)
                {
                    case StatusModified.HP:
                        break;
                    case StatusModified.ATTACK:
                        GameManager.instance.player.GetComponent<Player>().activePokemon.attack += (int)Mathf.Lerp(0, GameManager.instance.player.GetComponent<Player>().activePokemon.attack, AuxAttack.power);
                        break;
                    case StatusModified.SATTACK:
                        GameManager.instance.player.GetComponent<Player>().activePokemon.specialAttack += (int)Mathf.Lerp(0, GameManager.instance.player.GetComponent<Player>().activePokemon.specialAttack, AuxAttack.power);
                        break;
                    case StatusModified.DEFENSE:
                        GameManager.instance.player.GetComponent<Player>().activePokemon.defense += (int)Mathf.Lerp(0, GameManager.instance.player.GetComponent<Player>().activePokemon.defense, AuxAttack.power);
                        break;
                    case StatusModified.SDEFENSE:
                        GameManager.instance.player.GetComponent<Player>().activePokemon.specialDefense += (int)Mathf.Lerp(0, GameManager.instance.player.GetComponent<Player>().activePokemon.specialDefense, AuxAttack.power);
                        break;
                    case StatusModified.VELOCITY:
                        GameManager.instance.player.GetComponent<Player>().activePokemon.velocity += (int)Mathf.Lerp(0, GameManager.instance.player.GetComponent<Player>().activePokemon.velocity, AuxAttack.power);
                        break;
                    default:
                        break;
               }
            }
            else
            {
                 switch (AuxAttack.statusModified)
                {
                    case StatusModified.HP:
                        break;
                    case StatusModified.ATTACK:
                        GameManager.instance.player.GetComponent<Player>().ObjectiveActivePokemon.attack += (int)Mathf.Lerp(0, GameManager.instance.player.GetComponent<Player>().ObjectiveActivePokemon.attack, AuxAttack.power);
                        break;
                    case StatusModified.SATTACK:
                        GameManager.instance.player.GetComponent<Player>().ObjectiveActivePokemon.specialAttack += (int)Mathf.Lerp(0, GameManager.instance.player.GetComponent<Player>().ObjectiveActivePokemon.specialAttack, AuxAttack.power);
                        break;
                    case StatusModified.DEFENSE:
                        GameManager.instance.player.GetComponent<Player>().ObjectiveActivePokemon.defense += (int)Mathf.Lerp(0, GameManager.instance.player.GetComponent<Player>().ObjectiveActivePokemon.defense, AuxAttack.power);
                        break;
                    case StatusModified.SDEFENSE:
                        GameManager.instance.player.GetComponent<Player>().ObjectiveActivePokemon.specialDefense += (int)Mathf.Lerp(0, GameManager.instance.player.GetComponent<Player>().ObjectiveActivePokemon.specialDefense, AuxAttack.power);
                        break;
                    case StatusModified.VELOCITY:
                        GameManager.instance.player.GetComponent<Player>().ObjectiveActivePokemon.velocity += (int)Mathf.Lerp(0, GameManager.instance.player.GetComponent<Player>().ObjectiveActivePokemon.velocity, AuxAttack.power);
                        break;
                    default:
                        break;
               }
            }
        }
        else
        {
            if(!toMyself)
            {
                switch (AuxAttack.statusModified)
                {
                    case StatusModified.HP:
                        break;
                    case StatusModified.ATTACK:
                        GameManager.instance.player.GetComponent<Player>().activePokemon.attack += (int)Mathf.Lerp(0, GameManager.instance.player.GetComponent<Player>().activePokemon.attack, AuxAttack.power);
                        break;
                    case StatusModified.SATTACK:
                        GameManager.instance.player.GetComponent<Player>().activePokemon.specialAttack += (int)Mathf.Lerp(0, GameManager.instance.player.GetComponent<Player>().activePokemon.specialAttack, AuxAttack.power);
                        break;
                    case StatusModified.DEFENSE:
                        GameManager.instance.player.GetComponent<Player>().activePokemon.defense += (int)Mathf.Lerp(0, GameManager.instance.player.GetComponent<Player>().activePokemon.defense, AuxAttack.power);
                        break;
                    case StatusModified.SDEFENSE:
                        GameManager.instance.player.GetComponent<Player>().activePokemon.specialDefense += (int)Mathf.Lerp(0, GameManager.instance.player.GetComponent<Player>().activePokemon.specialDefense, AuxAttack.power);
                        break;
                    case StatusModified.VELOCITY:
                        GameManager.instance.player.GetComponent<Player>().activePokemon.velocity += (int)Mathf.Lerp(0, GameManager.instance.player.GetComponent<Player>().activePokemon.velocity, AuxAttack.power);
                        break;
                    default:
                        break;
               }
            }
            else
            {
                 switch (AuxAttack.statusModified)
                {
                    case StatusModified.HP:
                        break;
                    case StatusModified.ATTACK:
                        GameManager.instance.player.GetComponent<Player>().ObjectiveActivePokemon.attack += (int)Mathf.Lerp(0, GameManager.instance.player.GetComponent<Player>().ObjectiveActivePokemon.attack, AuxAttack.power);
                        break;
                    case StatusModified.SATTACK:
                        GameManager.instance.player.GetComponent<Player>().ObjectiveActivePokemon.specialAttack += (int)Mathf.Lerp(0, GameManager.instance.player.GetComponent<Player>().ObjectiveActivePokemon.specialAttack, AuxAttack.power);
                        break;
                    case StatusModified.DEFENSE:
                        GameManager.instance.player.GetComponent<Player>().ObjectiveActivePokemon.defense += (int)Mathf.Lerp(0, GameManager.instance.player.GetComponent<Player>().ObjectiveActivePokemon.defense, AuxAttack.power);
                        break;
                    case StatusModified.SDEFENSE:
                        GameManager.instance.player.GetComponent<Player>().ObjectiveActivePokemon.specialDefense += (int)Mathf.Lerp(0, GameManager.instance.player.GetComponent<Player>().ObjectiveActivePokemon.specialDefense, AuxAttack.power);
                        break;
                    case StatusModified.VELOCITY:
                        GameManager.instance.player.GetComponent<Player>().ObjectiveActivePokemon.velocity += (int)Mathf.Lerp(0, GameManager.instance.player.GetComponent<Player>().ObjectiveActivePokemon.velocity, AuxAttack.power);
                        break;
                    default:
                        break;
               }
            }
        }
        #endregion
    }
    #endregion

    #region PUBLIC METHODS
    
    /// <summary>
    /// Calculate the result of the attacks at the end of the decision stage.
    /// </summary>
    public void StartAttacks()
    {
        if(!PhotonNetwork.IsMasterClient) return;
        IsLastRequestFinished = false;
        foreach (var request in requests)
        {
            StartCoroutine(StartAttackCoroutine(request.Value));
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
        float result = CalculateDamagedBasedOnTheMatrixtype(attack, defending);
        view.RPC("SendAttackResult", RpcTarget.All, GameManager.instance.player_id, CalculateDamageGiven(attacking, defending, attack, result), result);
    }

    /// <summary>
    /// Calculate the result of the status modified.
    /// </summary>
    /// <param name="attack"></param>
    /// <param name="objective"></param>
    private void OnModifiedStatus(Attack attack, Pokemon objective, Pokemon attacker)
    {
        view.RPC("SendAttackStatusResult", RpcTarget.All, attack.name, objective.player_id == attacker.player_id, attacker.player_id);
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

    #region COROUTINES
    IEnumerator StartAttackCoroutine(Request request)
    {
        if(requests.IndexOfValue(request) == 1) yield return new WaitUntil(() => IsLastRequestFinished);
        GameManager.instance.view.RPC("RefreshUIAttacksStart", RpcTarget.All, request.m_Attack.category, request.m_Attacker.name, request.m_Attack.name, request.m_Attack.statusModified.ToString());

        yield return new WaitForSeconds(2f);

        switch (request.m_Attack.category)
        {
                case Category.PHYSICAL:
                    OnAttack(request.m_Attack, request.m_Defender, request.m_Attacker);
                    break;
                case Category.SPECIAL:
                    OnAttack(request.m_Attack, request.m_Defender, request.m_Attacker);
                    break;
                case Category.STATUS:
                    OnModifiedStatus(request.m_Attack, request.m_Defender, request.m_Attacker);
                    break;
                default:
                    break;
        }
        IsLastRequestFinished = true;

    }
    #endregion
}
