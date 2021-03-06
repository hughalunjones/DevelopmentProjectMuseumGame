using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public GameObject[] SystemPrefabs;
    public Events.EventGameState OnGameStateChanged;
    int excavationSceneIndex;
    public enum GameState {
        PREGAME,
        RUNNING, 
        PAUSED
    }
    private List<GameObject> instancedSystemPrefabs;
    private List<AsyncOperation> loadOperations;
    GameState currentGameState = GameState.PREGAME;

    private string _currentLevelName = string.Empty;
    
    // Awake method below enforces the singleton pattern for GameManager.
    public GameState CurrentGameState {
        get { return currentGameState; }
        private set { currentGameState = value; }
    }
    private void Start() {
        DontDestroyOnLoad(gameObject);
        instancedSystemPrefabs = new List<GameObject>();
        loadOperations = new List<AsyncOperation>();
        InstantiateSystemPrefabs();               
    }
    private void Update() {
        if (currentGameState == GameState.PREGAME) {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            TogglePause();
        }
    }
    void OnLoadLevelComplete(AsyncOperation ao) {
        if (loadOperations.Contains(ao)) {
            loadOperations.Remove(ao);

            if(loadOperations.Count == 0) {
                UpdateGameState(GameState.RUNNING);
            }

            MuseumInventory.instance.ClearExhibitSlotList();
            MuseumInventory.instance.FillExhibitSlotList();
            MuseumInventory.instance.PlaceSavedPlacedItems();
        }
        Debug.Log("Loading Completed");
    }
    void OnUnloadLevelComplete(AsyncOperation ao) {
        Debug.Log("Unloading Completed");
    }
    void UpdateGameState(GameState state) {
        GameState previouseGameState = currentGameState;
        currentGameState = state;
        switch (currentGameState) {
            case GameState.PREGAME:
            Time.timeScale = 1.0f;
            break;
            case GameState.PAUSED: 
            Time.timeScale = 0.0f;
            break;
            case GameState.RUNNING: 
            Time.timeScale = 1.0f;
            break;
            default:
            break;
        }
        OnGameStateChanged.Invoke(currentGameState, previouseGameState);
        //transition between scenes etc.
    }
    void InstantiateSystemPrefabs() {
        GameObject prefabInstance;
        for(int i = 0; i < SystemPrefabs.Length; i++) {
            prefabInstance = Instantiate(SystemPrefabs[i]);
            instancedSystemPrefabs.Add(prefabInstance);
        }
    }
    public string GetCurrentLevelName() {
        string currentLevel = SceneManager.GetActiveScene().name;
        return currentLevel;
    }
    public void LoadLevel(string levelName) {        
        AsyncOperation ao = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
        if (ao == null) {
            Debug.LogError("[GameManager] Unable to load level" + levelName);
            return;
        }
        ao.completed += OnLoadLevelComplete;
        loadOperations.Add(ao);
        _currentLevelName = levelName;
        Save();
    }
    public void UnloadLevel(string levelName) {
        AsyncOperation ao = SceneManager.UnloadSceneAsync(levelName);
        if (ao == null) {
            Debug.LogError("[GameManager] Unable to unload level" + levelName);
            return;
        }
        ao.completed += OnUnloadLevelComplete;
    }
    protected override void OnDestroy() {
        base.OnDestroy();
        for(int i = 0; i < instancedSystemPrefabs.Count; i++) {
            Destroy(instancedSystemPrefabs[i]);
        }
        instancedSystemPrefabs.Clear();
    }
    public void StartGame() {
        LoadSave();
        LoadLevel("MainHall");
    }
    public void NewGame() {
        SaveLoad.FullSaveReset();
        LoadLevel("MainHall");
    }
    
    // Toggle pause menu and stops game time
    public void TogglePause() {
        UpdateGameState(currentGameState == GameState.RUNNING ? GameState.PAUSED : GameState.RUNNING);
    }   
    public void LoadSave() {
        Events.OnLoadInitiated();
    }
    public void Save() {
        Events.OnSaveInitiated();
    }
    public void QuitGame() {
        Save();
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
        Debug.Log("Game Quit");
    }

    // Utilised to chose which excavation scene is loaded.
    public void SetExcavationIndex(int index) {
        excavationSceneIndex = index;
    }
    public int GetExcavationSceneIndex() {
        return excavationSceneIndex;
    }
}

