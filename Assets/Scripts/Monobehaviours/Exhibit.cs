using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exhibit : MonoBehaviour {

    public Exhibit_SO itemDefinition;
    public ExhibitSlot exhibitSlot;
    public MuseumStats musStats;
    public MuseumInventory musInventory;
    public bool inRangeOfExhibit;

    public Exhibit(){
        musInventory = MuseumInventory.instance;        
    }

    void Start(){       
        musStats = MuseumStats.instance;
        musInventory = MuseumInventory.instance;
        this.itemDefinition.isDisplayed = true;
    }
    void Update() {
        if (!itemDefinition.isDisplayed && inRangeOfExhibit && Input.GetKeyDown(KeyCode.E)) {
            Debug.Log("[Exhibit] StoreItem() called from Exhibit.cs");
            StoreItem();
        }
        else if (itemDefinition.isDisplayed && inRangeOfExhibit && Input.GetKeyDown(KeyCode.E)) {
            Destroy(gameObject);
            itemDefinition.isDisplayed = false;
            exhibitSlot.GetSlotInRange().containsExhibit = false;
            musStats.RemoveRating(this.itemDefinition.exhibitRatingAmount);            
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
    public void StoreItem() {
        musInventory.StoreItem(this);
    }
    public void DisplayItem(){
        // Add item to empty object on screen.
        musStats.ApplyRating(itemDefinition.exhibitRatingAmount);
    }
    public void SellItem() {
        musStats.ApplyWealth(itemDefinition.exhibitValueAmount);
    }
}
