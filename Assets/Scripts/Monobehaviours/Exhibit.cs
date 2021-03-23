using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exhibit : MonoBehaviour {

    public Exhibit_SO itemDefinition;
    public MuseumStats musStats;
    MuseumInventory musInventory;
    GameObject foundStats;

    public Exhibit(){
        musInventory = MuseumInventory.instance;
    }

    void Start(){
        foundStats = GameObject.FindGameObjectWithTag("Player");
        musStats = foundStats.GetComponent<MuseumStats>();
    }
    void OnTriggerEnter2D(Collider2D triggerCollider) {
        if (triggerCollider.tag == "Player") {
            Debug.Log("[Exhibit] Player Detected");
            StoreItem();
        }
    }
    void OnTriggerExit2D(Collider2D triggerCollider) {
        if (triggerCollider.tag == "Player") {
            Debug.Log("[Exhibit] Player Left");
        }
    }
    void StoreItem() {        
        // this reference not working
        musInventory.StoreItem(this);
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
