using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveLoad : MonoBehaviour {
    public static void Save<T>(T objectToSave, string key) {
        string path = Application.persistentDataPath + "/saves/";
        Directory.CreateDirectory(path);
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream stream = new FileStream(path + key + ".dat", FileMode.Create)) {
            formatter.Serialize(stream, objectToSave);
        }
        Debug.Log("[GameManager] Game saved: " + objectToSave);
    }
    public static T Load<T>(string key) {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/saves/";
        T returnValue = default(T);
        using (FileStream stream = new FileStream(path + key + ".dat", FileMode.Open)) {
            returnValue = (T)formatter.Deserialize(stream);
        }
        Debug.Log("[GameManager] " + returnValue + " loaded");
        return returnValue;
    }
    public static bool SaveExists(string key) {
        string path = Application.persistentDataPath + "/saves/" + key + ".dat";
        return File.Exists(path);
    }
    public static void FullSaveReset() {
        foreach(Exhibit exhibit in MuseumInventory.instance.allExhibits) {
            exhibit.itemDefinition.isDisplayed = false;
            exhibit.itemDefinition.exhibitSlot = "";
        }
        string path = Application.persistentDataPath + "/saves/";
        PlayerPrefs.DeleteAll();
        if (Directory.Exists(path)) {
            Directory.Delete(path, true);
        }
        Directory.CreateDirectory(path);
    }
}
