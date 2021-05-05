using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class MuseumStats : MonoBehaviour
{
    public static MuseumStats instance;
    public MuseumStats_SO museumDefinition;
    public MuseumInventory museumInv;
    float roundedRating;
    int numOfDisplayedExhibits, playerStamina;
    MuseumStatsSaveData data = new MuseumStatsSaveData();

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
        // subscribe this class to saving and loading events.
        Events.SaveInitiated += SaveMuseumStats;
        Events.LoadInitiated += LoadMuseumStats;
        if (!museumDefinition.setManually){
            museumDefinition.maxWealth = 10000000;
            museumDefinition.currentWealth = 1000;

            museumDefinition.maxRating = 5.0f * museumInv.inventoryItemCap;
            museumDefinition.currentRating = 0.0f;

            museumDefinition.playerStamina = 3;
            museumDefinition.currentDay = 1;
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
    public void ApplyStamina(int staminaAmount) {
        museumDefinition.ApplyStamina(staminaAmount);
    }
    public void IncrementDay() {
        museumDefinition.IncrementDay();
    }

    // Return Stat Values
    public int GetWealth(){
        return museumDefinition.currentWealth;
    }
    public float GetRoundedRating(){
        float averageRating = museumDefinition.currentRating / museumInv.inventoryItemCap;
        roundedRating = (float)Math.Round(averageRating * 2, MidpointRounding.AwayFromZero) / 2;
        return roundedRating;
    }
    public float GetRatingRaw() {
        return museumDefinition.currentRating;
    }
    public int GetStamina() {
        return museumDefinition.playerStamina;
    }
    public int GetDay() {
        return museumDefinition.currentDay;
    }
    public void SetWealth(int newWealth) {
        museumDefinition.currentWealth = newWealth; 
    }
    public void SetRating(float newRating) {
        museumDefinition.currentRating = newRating;
    }
    public void SetStamina(int newStamina) {
        museumDefinition.playerStamina = newStamina;
    }
    public void SetDay(int newDay) {
        museumDefinition.currentDay = newDay;
    }
    public void SaveMuseumStats() {
        data.currencyData = instance.GetWealth();
        data.ratingData = instance.GetRatingRaw();
        data.staminaData = instance.GetStamina();
        data.dayData = instance.GetDay();
        SaveLoad.Save(data, "museumStats");
    }
    public void LoadMuseumStats() {
        if (SaveLoad.SaveExists("museumStats")) {;
            MuseumStatsSaveData loadData = SaveLoad.Load<MuseumStatsSaveData>("museumStats");
            SetWealth(loadData.currencyData);
            SetRating(loadData.ratingData);
            SetStamina(loadData.staminaData);
            SetDay(loadData.dayData);
        }
    }
    public void UpdateNumOfDisplayedExhibits() {
        numOfDisplayedExhibits = 0;
        foreach(InventoryEntry exhibit in museumInv.exhibitsInInventory) {
            if(exhibit.invEntry.itemDefinition.isDisplayed == true) {
                numOfDisplayedExhibits += 1;
            }
        }
    }
}

