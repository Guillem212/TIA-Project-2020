using UnityEngine;

public enum Category{
    PHYSICAL,
    SPECIAL,
    STATUS
}

public enum StatusModified{
    NONE,
    HP,
    ATTACK,
    SATTACK,
    DEFENSE,
    SDEFENSE,
    VELOCITY
}

public enum Objective
{
    USER,
    FOE
}

[CreateAssetMenu(fileName = "Attack", menuName = "ScriptableObjects/Attack", order = 1)]
public class Attack : ScriptableObject {
    public new string name;
    public string description;

    [Range(1, 40)]
    public int pp;

    /// <summary>
    /// If the power is negative and the category is Status means that the status modified will decrease.
    /// </summary>
    [Range(-250, 250)]
    public int power;

    /// <summary>
    /// If the priroity is 0, is a normal attack, but if it is 1000, is a priority attack, and 2000 is a priority defense movement.
    /// </summary>
    public int priority;

    public Type type;
    public Category category;
    public StatusModified statusModified;

    public Objective objective;
}
