using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Slider = UnityEngine.UIElements.Slider;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private UIDocument pauseRender;
    [SerializeField] private string containerID;
    [SerializeField] private string musicSliderID;
    [SerializeField] private string sfxSliderID;
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        
        UpdateSlidersVolume();
        pauseRender.rootVisualElement.Q(containerID).style.display= DisplayStyle.Flex;
        Time.timeScale = 0;
    }

    void UpdateSlidersVolume()
    {
        pauseRender.rootVisualElement.Q<Slider>(musicSliderID).value = AudioManager.Instance.musicVolume;
        pauseRender.rootVisualElement.Q<Slider>(sfxSliderID).value = AudioManager.Instance.sfxVolume;
    }
    public void GetMusicVolume()
    {
        AudioManager.Instance.UpdateMusicVolume(pauseRender.rootVisualElement.Q<Slider>("musicSliderID").value );
    }
    public void GetSFXVolume()
    {
        AudioManager.Instance.UpdateSFXVolume(pauseRender.rootVisualElement.Q<Slider>("SFXSliderID").value );
    }
    
    public void UnPauseGame()
    {
        Time.timeScale = 1f;
        pauseRender.rootVisualElement.Q(containerID).style.display= DisplayStyle.None;
    }

    public void Exit()
    {
        TransitionManager.Instance.LoadScene("MainMenu",true, () => { Time.timeScale = 1f;});
    }
}
