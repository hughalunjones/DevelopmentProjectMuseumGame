using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BidManager : MonoBehaviour {
    int currentBid, currentWealth, bidRatio, exhibitBid, exhibitValue;
    bool playerIsBidHolder = false;
    Exhibit exhibitOnSale;
    public Exhibit[] itemToSell;
    public Transform itemPlinth;

    void Start() {
        currentWealth = MuseumStats.instance.GetWealth();
        exhibitValue = exhibitOnSale.itemDefinition.exhibitValueAmount;
    }
    void Update() {

    }
    public void SubmitBid() {
        if (playerIsBidHolder == false) {
            exhibitBid = currentBid;
            currentBid = exhibitBid;
        }
        else {
            Debug.Log("[BidManager] You hold the current bid!"); // Disable the button
        }
    }
    public void IncreaseBid() {
        if(currentBid == currentWealth) {
            Debug.Log("[BidManager] You don't have enough money");
        }else if(currentBid < currentWealth){
            currentBid += bidRatio;
        }
        
    }
    public void DecreaseBid() {
        if(currentBid == exhibitBid) {
            Debug.Log("[BidManager] Cannot bid lower than the current price!"); // show prompt
        }else if (currentBid > exhibitBid) {            
            currentBid -= bidRatio;
        }
        
    }
    public void CalculateBidRatio() {
        
    }
    public void SelectExhibit() {
        int randomIndex = Mathf.RoundToInt(Random.Range(0, itemToSell.Length));
        Instantiate(itemToSell[randomIndex], itemPlinth);
    }
}
