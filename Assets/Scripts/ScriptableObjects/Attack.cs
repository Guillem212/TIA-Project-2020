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
    DEFENSE,
    VELOCITY
}

[CreateAssetMenu(fileName = "Attack", menuName = "ScriptableObjects/Attack", order = 1)]
public class Attack : ScriptableObject {
    public new string name;

    [Range(1, 40)]
    public int pp;
    [Range(0, 250)]
    public int power;

    /// <summary>
    /// If the priroity is 0, is a normal attack, but if it is 1000, is a priority attack, and 2000 is a priority defense movement.
    /// </summary>
    public int priority;

    public Type type;
    public Category category;
    public StatusModified statusModified;
}
