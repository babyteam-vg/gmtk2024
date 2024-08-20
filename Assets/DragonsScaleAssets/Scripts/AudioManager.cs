using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomContext
{
    Cave=0,
    CraftingRoom=1,
    Store=2
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    [SerializeField] private int activePlayer = 2;
    [SerializeField] private List<AudioSource> playersRooms;
    [SerializeField] private float fadeTime;
    [SerializeField] private AudioSource playerSfx;
    [SerializeField] private AudioSource playerDragonSfx;
    [Range(0f,1f)] public float musicVolume;
    [Range(0f,1f)] public float sfxVolume;

    private void Awake() 
    { 
        // If there is an instance, and it's not me, delete myself.
    
        if (Instance != null && Instance != this) 
        { 
            Destroy(this.gameObject); 
        } 
        else 
        { 
            DontDestroyOnLoad(this);
            Instance = this; 
        } 
    }

    public void Start()
    {
        PlayAll();
    }

    public void PlayAll()
    {
        foreach (AudioSource player in playersRooms)
        {
            player.volume = 0;
            player.loop=true;
            player.Play();
        }
        SetActivePlayerByIndex(activePlayer,false);
        
    }

    public void UpdateMusicVolume(float updateVolume)
    {
        musicVolume = updateVolume;
        SetActivePlayerByIndex(activePlayer,false);
    }
    public void UpdateSFXVolume(float updateVolume)
    {
        sfxVolume = updateVolume;
        playerSfx.volume = sfxVolume;
        playerDragonSfx.volume = sfxVolume;

    }
    
    public void SetActivePlayerByIndex(int audioSourceToPlay, bool fade)
    {
        activePlayer = audioSourceToPlay;
        for (int i = 0; i < playersRooms.Count; i++)
        {
            AudioSource player = playersRooms[i];
            if (i == audioSourceToPlay)
            {
                if(fade)StartCoroutine(FadeAudio(player, player.volume, musicVolume));
                else player.volume = musicVolume;
            }
            else
            {
                if(fade)StartCoroutine(FadeAudio(player, player.volume, 0f));
                else player.volume = 0f;
            }
        }
    }

    public void PlaySFX(AudioClip sfx, float volume =1f)
    {
        playerSfx.PlayOneShot(sfx, volume*sfxVolume);
    }

    public void PlayDragonSFX(AudioClip sfx,bool playonLeft)
    {
        float value = 0f;
        if (playonLeft) value = -1f;
        playerDragonSfx.panStereo = value;
        playerDragonSfx.PlayOneShot(sfx);
    }
    public void StopDragonSFX()
    {
        playerDragonSfx.Stop();
    }
    
     private IEnumerator FadeAudio(AudioSource AToPlay,float start, float end, Action onComplete=null)                 
    {                                                                                                                                 
        float elapsedTime = 0f;                                                                                                       
        while (elapsedTime < fadeTime)                                                                                                    
        {                                                                                                                             
            elapsedTime += Time.deltaTime;                                                                                            
            float progress = elapsedTime / fadeTime;                                                                                      
            AToPlay.volume = Mathf.Lerp(start, end, progress);                                                                        
            yield return null;                                                                                                        
        }                                                                                                                             
                                                                                                                                      
        //AToPlay.volume = end;                                                                                                         
        if (onComplete != null)                                                                                                       
        {                                                                                                                             
            onComplete();                                                                                                       
        }                                                                                                                             
    }

}
