using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStats", menuName = "Museum/Stats", order = 1)]
public class MuseumStats_SO : ScriptableObject
{
    public bool setManually = false;
    public bool saveDataOnClose = false;
    public int maxWealth = 0;
    public int currentWealth = 0;
    public float maxRating = 0f;
    public float currentRating = 0f;

    // Stat changers
    public void ApplyWealth(int wealthAmount) {
        if ((currentWealth + wealthAmount) > maxWealth) {
            currentWealth = maxWealth;
        }
        else {
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
    public void RemoveRating(float ratingAmount) {
        if ((currentRating - ratingAmount) <= 0) {
            currentRating = 0;
        }
        else {
            currentRating -= ratingAmount;
        }
    }
    public void RemoveWealth(int wealthAmount) {
        if ((currentWealth - wealthAmount) <= 0) {
            currentWealth = 0;
        }
        else {
            currentWealth -= wealthAmount;
        }
    }

    // Save Museum Data
    public void saveMuseumData()
    {
        saveDataOnClose = true;
        EditorUtility.SetDirty(this);
    }

}
