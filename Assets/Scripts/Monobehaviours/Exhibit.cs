﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exhibit : MonoBehaviour {

    public Exhibit_SO itemDefinition;
    public MuseumStats musStats;
    public MuseumInventory musInventory;
    GameObject foundStats, foundInventory;
    bool inRangeOfExhibit;

    public Exhibit(){
        musInventory = MuseumInventory.instance;
    }

    void Start(){
        foundStats = GameObject.FindGameObjectWithTag("Player");
        foundInventory = GameObject.FindGameObjectWithTag("Player");
        musStats = foundStats.GetComponent<MuseumStats>();
        musInventory = foundInventory.GetComponentInChildren<MuseumInventory>();
    }
    void Update() {
        if (inRangeOfExhibit && Input.GetKeyDown(KeyCode.E)) {
            StoreItem();
        }
        if(itemDefinition.isDisplayed == false) {
            Destroy(this.gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D triggerCollider) {
        if (triggerCollider.tag == "Player") {
            inRangeOfExhibit = true;
            Debug.Log("[Exhibit] Player Detected " + inRangeOfExhibit);            
        }
    }
    void OnTriggerExit2D(Collider2D triggerCollider) {
        if (triggerCollider.tag == "Player") {
            inRangeOfExhibit = false;
            Debug.Log("[Exhibit] Player Left " + inRangeOfExhibit);
        }
    }
    void StoreItem() {
        musInventory.StoreItem(this);
        musInventory.PickUp();
        Destroy(this);
        Debug.Log("[Exhibit] Item Stored");
    }
    public void DisplayItem(){
        // Add item to empty object on screen.
        musStats.ApplyRating(itemDefinition.exhibitRatingAmount);
    }
    public void SellItem() {
        musStats.ApplyWealth(itemDefinition.exhibitValueAmount);
    }
}
