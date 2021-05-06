using UnityEngine.Events;
public class Events
{
    [System.Serializable] public class EventFadeComplete : UnityEvent<bool> { }
    [System.Serializable] public class EventGameState : UnityEvent<GameManager.GameState, GameManager.GameState> { }

    public static System.Action SaveInitiated;
    public static System.Action LoadInitiated;


    public static void OnSaveInitiated() {
        SaveInitiated?.Invoke();
    }
    public static void OnLoadInitiated() {
        LoadInitiated?.Invoke();
    }
}
