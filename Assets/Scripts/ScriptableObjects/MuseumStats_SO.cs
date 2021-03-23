using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStats", menuName = "Museum/Stats", order = 1)]
public class MuseumStats_SO : ScriptableObject
{
    [System.Serializable]
    public class CharLevelUps
    {
        public int maxWealth;
    }
    public bool setManually = false;
    public bool saveDataOnClose = false;

    public Exhibit painting { get; private set; }
    public Exhibit fossil { get; private set; }

    public int maxWealth = 0;
    public int currentWealth = 0;
    public float maxRating = 0f;
    public float currentRating = 0f;

    // Stat changers
    public void ApplyWealth(int wealthAmount)
    {
        if ((currentWealth + wealthAmount) > maxWealth)
        {
            currentWealth = maxWealth;
        }
        else
        {
            currentWealth += wealthAmount;
        }
    }
    public void ApplyRating(float ratingAmount) {
        if ((currentRating + ratingAmount) > maxRating) {
            currentRating = maxRating;
        }
        else {
            currentRating += ratingAmount;
        }
    }

    public void PlaceExhibit(MuseumInventory musInventory, GameObject exhibitSlot){

    }
    public bool RemoveExhibit(MuseumInventory musInventory, GameObject exhibitSlot) {
        return true;
    }  

    // Save Museum Data
    public void saveMuseumData()
    {
        saveDataOnClose = true;
        EditorUtility.SetDirty(this);
    }

}
