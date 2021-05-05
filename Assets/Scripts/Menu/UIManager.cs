using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private MainMenu mainMenu;
    [SerializeField] private PauseMenu pauseMenu;
    [SerializeField] private MuseumStatsUI museumStatsUI;
    [SerializeField] private HelpMenu helpMenu;
    [SerializeField] private Camera dummyCamera;

    public Events.EventFadeComplete OnMainMenuFadeComplete;

    public void Start() {
        if (!SaveLoad.SaveExists("inventoryExhibitData")) {
            GameObject.FindGameObjectWithTag("start").GetComponent<Button>().interactable = false;
        }
        mainMenu.OnMainMenuFadeComplete.AddListener(HandleOnMainMenuFadeComplete);
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
        GameObject.FindGameObjectWithTag("new").GetComponent<Button>().onClick.AddListener(() => NewGame());
        GameObject.FindGameObjectWithTag("start").GetComponent<Button>().onClick.AddListener(() => ContinueGame());
        GameObject.FindGameObjectWithTag("quit").GetComponent<Button>().onClick.AddListener(() => GameManager.Instance.QuitGame());
    }
    void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState) {
        pauseMenu.gameObject.SetActive(currentState == GameManager.GameState.PAUSED);
    }
    void HandleOnMainMenuFadeComplete(bool fadeOut) {
        OnMainMenuFadeComplete.Invoke(fadeOut);
        mainMenu.gameObject.SetActive(false);
        if (PlayerPrefs.HasKey("notNew")) {
            Debug.Log("Not a new save");
        }else {
            PlayerPrefs.SetInt("notNew", 0);
            PlayerPrefs.Save();
            helpMenu.DisplayHelpMenu();
        }
    }
    private void Update() {
        if(GameManager.Instance.CurrentGameState != GameManager.GameState.PREGAME) {
            return;
        }
    }   
    public void ContinueGame() {
        GameManager.Instance.StartGame();
        museumStatsUI.gameObject.SetActive(true);
    }
    public void NewGame() {
        GameManager.Instance.NewGame();
        museumStatsUI.gameObject.SetActive(true);
    }
    public void SetCameraActive(bool active) {
        dummyCamera.gameObject.SetActive(active);
    }

}
