using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;
    public static T Instance {
        get { return instance; }
    }
    public static bool IsInitialised {
        get { return instance != null; }
    }
    protected virtual void Awake() {
        if(instance != null) {
            Debug.LogError("[Singleton] Tried to instantiate a second instance of a singletone class.");
        }
        else {
            instance = (T)this;
        }
    }
    protected virtual void OnDestroy() {
        if(instance == this) {
            instance = null;
        }
    }
}
