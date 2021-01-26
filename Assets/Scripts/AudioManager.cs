using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioClip[] music;
    [SerializeField] private AudioClip[] clicks;
    [SerializeField] private AudioClip[] combatSounds;


    private void Awake() {
        if(instance is null){
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else{
            Destroy(this);
        }
    }


}
