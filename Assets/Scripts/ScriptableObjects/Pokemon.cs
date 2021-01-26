using UnityEngine;

[CreateAssetMenu(fileName = "Pokemon", menuName = "ScriptableObjects/Pokemon", order = 0)]
public class Pokemon : ScriptableObject
{
    public new string name;
    public Sprite pokemonImage;
    public Type[] types;
    public Sprite typeSprite;
    public Color typeColor;
    public int level;

    //Stats
    public int hp;
    public int attack;
    public int specialAttack;
    public int defense;
    public int specialDefense;
    public int velocity;
    public int player_id;

    //Set Of Attacks
    public Attack[] m_attacks;

    public void Print(){
        Debug.Log("Name of the Pokemon: " + name + "\n"
        + "Type of the Pokemon: " + types[0] + "\n" 
        + "Health: " + hp);
    }

}