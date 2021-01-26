using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource soundSource;

    [SerializeField] private AudioClip[] music;
    [SerializeField] private AudioClip[] clicks;
    [SerializeField] private AudioClip[] combatSounds;

    [SerializeField] private AudioClip startApp;


    private void Awake() {
        if(instance is null){
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else{
            Destroy(this);
        }
    }

    private void Start() {
        soundSource.clip = startApp;
        soundSource.Play();

        PlayMusic(0);
    }

    public void PlayMusic(int index){
        if(musicSource.isPlaying){
            musicSource.Stop();
            musicSource.clip = null;
        }
        musicSource.clip = music[index];
        musicSource.loop = true;
        musicSource.Play();

    }

    public void PlayClick(){
        soundSource.Stop();
        soundSource.clip = clicks[Random.Range(0, clicks.Length)];
        soundSource.Play();
    }

    public void PlayAttack(AudioClip clip){
        soundSource.Stop();
        soundSource.clip = clip;
        soundSource.Play();
    }


}
