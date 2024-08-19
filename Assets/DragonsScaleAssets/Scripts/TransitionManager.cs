using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup cortina;
    [SerializeField] private float duration;
    private bool isBlackOut;
    
    
    public static TransitionManager Instance { get; private set; }

    private void Awake() 
    { 
        // If there is an instance, and it's not me, delete myself.
    
        if (Instance != null && Instance != this) 
        { 
            Destroy(this.gameObject); 
        } 
        else 
        {
            Instance = this; 
        } 
    }
    public void OnEnable()
    {
        SceneManager.sceneLoaded += OnLoadedScene;
    }
    public void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLoadedScene;
    }

    public void LoadScene(string sceneName, bool transition=false, Action initialCallback=null)
    {
        Action callback = () =>
        {
            SceneManager.LoadScene(sceneName);
            initialCallback?.Invoke();
        };
        if (transition)
        {
            StartCoroutine(Transition(1f, callback));
            isBlackOut = true;
        }
        else callback.Invoke();
    }

    private IEnumerator Transition(float targetAlpha, Action onComplete=null)
    {
        float startAlpha = cortina.alpha;
        float time = 0;

        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            cortina.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            yield return null;
        }
        cortina.alpha = targetAlpha;
        onComplete?.Invoke();
    }

    public void OnLoadedScene(Scene scene, LoadSceneMode mode)
    {
        if (isBlackOut)
        {
            StartCoroutine(Transition(0f));
            isBlackOut = false;
        }
    }
}
