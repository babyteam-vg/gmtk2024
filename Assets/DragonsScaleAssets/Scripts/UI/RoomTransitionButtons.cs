using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class RoomTransitionButtons : MonoBehaviour
{
    [Header("Buttons")] public string leftSceneButton = "LeftBar";
    public string rightSceneButton = "RightBar";

    [Header("Scenes")] [CanBeNull] public string leftSceneName;
    [CanBeNull] public int leftSceneAudioIndex;
    [CanBeNull] public string rightSceneName;
    [CanBeNull] public int rightSceneAudioIndex;

    private UIDocument _document;
    private Button _leftSceneButton;
    private Button _rightSceneButton;

    private void Start()
    {
        _document = GetComponent<UIDocument>();

        _leftSceneButton = _document.rootVisualElement.Q<Button>(leftSceneButton);
        if (_leftSceneButton != null)
        {
            _leftSceneButton.clicked += LoadLeftScene;
        }

        _rightSceneButton = _document.rootVisualElement.Q<Button>(rightSceneButton);
        if (_rightSceneButton != null)
        {
            _rightSceneButton.clicked += LoadRightScene;
        }
    }

    private void LoadLeftScene()
    {
        if (string.IsNullOrEmpty(leftSceneName))
        {
            return;
        }

        AudioManager.Instance.SetActivePlayerByIndex(leftSceneAudioIndex, true);
        TransitionManager.Instance.LoadScene(leftSceneName, true);
    }

    private void LoadRightScene()
    {
        if (string.IsNullOrEmpty(rightSceneName))
        {
            return;
        }

        AudioManager.Instance.SetActivePlayerByIndex(rightSceneAudioIndex, true);
        TransitionManager.Instance.LoadScene(rightSceneName, true);
    }

    private void OnDestroy()
    {
        if (_leftSceneButton != null)
        {
            _leftSceneButton.clicked -= LoadLeftScene;
        }

        if (_rightSceneButton != null)
        {
            _rightSceneButton.clicked -= LoadRightScene;
        }
    }
}