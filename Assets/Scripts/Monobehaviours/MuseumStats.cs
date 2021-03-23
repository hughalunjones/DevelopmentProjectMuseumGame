﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuseumStats : MonoBehaviour
{
    public MuseumStats_SO museumDefinition;
    public MuseumInventory museumInv;

    public MuseumStats(){
        museumInv = MuseumInventory.instance;
    }    
    void Start(){
        if (!museumDefinition.setManually){
            museumDefinition.maxWealth = 10000000;
            museumDefinition.currentWealth = 0;

            museumDefinition.maxRating = 5.0f;
            museumDefinition.currentRating = 0.0f;
        }
    }
    private void Update(){
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

    // Return Stat Values
    public int GetWealth(){
        return museumDefinition.currentWealth;
    }
    public float GetRating(){
        return museumDefinition.currentRating;
    }
}