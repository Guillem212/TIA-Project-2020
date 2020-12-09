using UnityEngine;

public enum Type_of_Attack{
    OFFENSIVE,
    DEFENSIVE,
    MODIFIER
}

public enum Stat_Modified{
    NONE,
    HP,
    ATTACK,
    DEFENSE,
    VELOCITY
}

[CreateAssetMenu(fileName = "Attack", menuName = "ScriptableObjects/Attack", order = 1)]
public class Attack : ScriptableObject {
    public int PP;
    public string Name;
    public int Damage;
    public Type Type;
    public Type_of_Attack TypeOfAttack;
    public Stat_Modified StatModified;
}
