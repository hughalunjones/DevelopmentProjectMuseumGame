using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private MainMenu mainMenu;
    [SerializeField] private PauseMenu pauseMenu;
    [SerializeField] private MuseumInventory museumInventory;
    [SerializeField] private Camera dummyCamera;

    public Events.EventFadeComplete OnMainMenuFadeComplete;

    public void Start() {
        mainMenu.OnMainMenuFadeComplete.AddListener(HandleOnMainMenuFadeComplete);
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
    }
    void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState) {
        pauseMenu.gameObject.SetActive(currentState == GameManager.GameState.PAUSED);
    }
    void HandleOnMainMenuFadeComplete(bool fadeOut) {
        OnMainMenuFadeComplete.Invoke(fadeOut);
        mainMenu.gameObject.SetActive(false);
    }
    private void Update() {
        if(GameManager.Instance.CurrentGameState != GameManager.GameState.PREGAME) {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            GameManager.Instance.StartGame();
        }
    }    
    public void SetCameraActive(bool active) {
        dummyCamera.gameObject.SetActive(active);
    }

}
