using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // Track Animations component
    // Track AnimationClips - fade in/out
    // Function that can recieve animation events
    // Functions to play animations

    [SerializeField] private Animation mainMenuAnimator;
    [SerializeField] private AnimationClip fadeIn, fadeOut;

    public Events.EventFadeComplete OnMainMenuFadeComplete;
    public void Start() {
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
    }
    public void OnFadeOutComplete() {
        OnMainMenuFadeComplete.Invoke(true);
        Debug.LogWarning("FadeOut complete.");
    }
    public void OnFadeInComplete() {
        OnMainMenuFadeComplete.Invoke(false);
        UIManager.Instance.SetCameraActive(true);
        Debug.LogWarning("FadeIn complete.");
    }
    void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState) {
        if(previousState == GameManager.GameState.PREGAME && currentState == GameManager.GameState.RUNNING) {
            FadeOut();
        }
    }
    public void FadeOut() {
        UIManager.Instance.SetCameraActive(false);
        mainMenuAnimator.Stop();
        mainMenuAnimator.clip = fadeOut;
        mainMenuAnimator.Play();
    }
    public void FadeIn() {        
        mainMenuAnimator.Stop();
        mainMenuAnimator.clip = fadeIn;
        mainMenuAnimator.Play();
    }

}
