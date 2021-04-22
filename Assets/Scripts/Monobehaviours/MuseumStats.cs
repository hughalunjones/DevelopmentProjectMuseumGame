using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuseumStats : MonoBehaviour
{
    public static MuseumStats instance;
    public MuseumStats_SO museumDefinition;
    public MuseumInventory museumInv;
    float roundedRating;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(this.gameObject);
        }
    }
    void Start(){
        DontDestroyOnLoad(this);
        museumInv = MuseumInventory.instance;
        if (!museumDefinition.setManually){
            museumDefinition.maxWealth = 10000000;
            museumDefinition.currentWealth = 1000;

            museumDefinition.maxRating = 5.0f;
            museumDefinition.currentRating = 0.0f;
        }
    }
    private void Update(){
        if(museumDefinition.currentRating <= 0) {
            museumDefinition.currentRating = 0;
        }else if(museumDefinition.currentRating >= (museumInv.inventoryItemCap * 5)) {
            museumDefinition.currentRating = museumInv.inventoryItemCap * 5;
        }
        if(museumDefinition.currentWealth <= 0) {
            museumDefinition.currentWealth = 0;
        }else if(museumDefinition.currentWealth >= museumDefinition.maxWealth) {
            museumDefinition.currentWealth = museumDefinition.maxWealth;
        }
        // This should be triggered by the game manager during a save point
        // museumDefinition.saveMuseumData();
    }
    // Stat Changers
    public void ApplyWealth(int wealthAmount){
        museumDefinition.ApplyWealth(wealthAmount);
    }
    public void ApplyRating(float ratingAmount) {
        museumDefinition.ApplyRating(ratingAmount);
    }
    public void RemoveWealth(int wealthAmount) {
        museumDefinition.RemoveWealth(wealthAmount);
    }
    public void RemoveRating(float ratingAmount) {
        museumDefinition.RemoveRating(ratingAmount);
    }

    // Return Stat Values
    public int GetWealth(){
        return museumDefinition.currentWealth;
    }
    public float GetRating(){
        float averageRating = museumDefinition.currentRating / museumInv.inventoryItemCap;
        roundedRating = (float)Math.Round(averageRating * 2, MidpointRounding.AwayFromZero) / 2;
        return roundedRating;
    }
}
