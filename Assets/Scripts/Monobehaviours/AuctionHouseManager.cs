using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuctionHouseManager : MonoBehaviour {
    public BidManager bidManager;
    public NPCManager npcManager;
    public Exhibit[] itemsToSell;
    public Transform itemPlinth;

    void Start() {

    }
    public void SelectExhibit() {
        int randomIndex = Mathf.RoundToInt(Random.Range(0, itemsToSell.Length));
        Instantiate(itemsToSell[randomIndex], itemPlinth);
    }

}
