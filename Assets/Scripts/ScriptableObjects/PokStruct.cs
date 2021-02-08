using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PokStruct 
{
    public string name;
    public int hp;
    public int attack;
    public int specialAttack;
    public int defense;
    public int specialDefense;
    public int velocity;

    /// <summary>
    /// Struct to send across the network
    /// </summary>
    /// <param name="name"></param>
    /// <param name="hp"></param>
    /// <param name="attack"></param>
    /// <param name="specialAttack"></param>
    /// <param name="defense"></param>
    /// <param name="specialDefense"></param>
    /// <param name="velocity"></param>
    public PokStruct(string name, int hp, int attack, int specialAttack, int defense, int specialDefense, int velocity)
    {
        this.name = name;
        this.hp = hp;
        this.attack = attack;
        this.specialAttack = specialAttack;
        this.defense = defense;
        this.specialDefense = specialDefense;
        this.velocity = velocity;
    }
}
