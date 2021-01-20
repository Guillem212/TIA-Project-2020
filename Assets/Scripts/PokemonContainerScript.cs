using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonContainerScript : MonoBehaviour
{
    public GameObject[] pokemons;
    // Start is called before the first frame update
    void OnEnable()
    {
        if(pokemons is null)
        {
            Transform[] t = GetComponentsInChildren<Transform>();
            pokemons = new GameObject[t.Length];
            for (int i = 0; i < t.Length; i++)
            {
                pokemons[i] = t[i].gameObject;
            }
        }
    }
}
