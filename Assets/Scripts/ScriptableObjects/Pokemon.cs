using UnityEngine;

[CreateAssetMenu(fileName = "Pokemon", menuName = "ScriptableObjects/Pokemon", order = 0)]
public class Pokemon : ScriptableObject
{
    public string Name;
    public GameObject Model;
    public Type[] Type;

    //Stats
    public int Health;
    public int Attack;
    public int Defense;
    public int Velocity;

    //Set Of Attacks
    public Attack[] m_attacks;

    public void Print(){
        Debug.Log("Name of the Pokemon: " + Name + "\n"
        + "Type of the Pokemon: " + Type + "\n" 
        + "Health: " + Health);
    }

}