using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[Serializable]
public class Exhibit : MonoBehaviour {

    public Exhibit_SO itemDefinition;
    public MuseumStats musStats;
    public MuseumInventory musInventory;
    public bool inRangeOfExhibit;
    public int ID { get; set; }
    Transform ExhibitInfoPanel;

    public Exhibit(Exhibit_SO exhibitData){
        musInventory = MuseumInventory.instance;
        ID = exhibitData.exhibitKeyID;
        itemDefinition = exhibitData;
    }

    void Start(){       
        musStats = MuseumStats.instance;
        musInventory = MuseumInventory.instance;
    }
    void Update() {
        if (itemDefinition.isDisplayed && inRangeOfExhibit && Input.GetKeyDown(KeyCode.E)) {
            ExhibitInfoPanel = musInventory.ExhibitInformationPanel.transform.Find("Panel");         
            ExhibitInfoPanel.transform.Find("imgExhibitImage").GetComponent<Image>().sprite = this.GetComponent<SpriteRenderer>().sprite;
            ExhibitInfoPanel.transform.Find("imgExhibitImage").GetComponent<Image>().preserveAspect = true;
            ExhibitInfoPanel.transform.Find("txtExhibitName").GetComponent<TextMeshProUGUI>().SetText(itemDefinition.exhibitName);
            ExhibitInfoPanel.transform.Find("txtExhibitDescription").GetComponent<TextMeshProUGUI>().SetText(itemDefinition.exhibitDescription);
            ExhibitInfoPanel.transform.Find("btnStore").GetComponent<Button>().onClick.AddListener(() => ReturnItemToInv());
            //ExhibitInfoPanel.transform.Find("btnSell").GetComponent<Button>().onClick.AddListener(() => SellItem(this));
            musInventory.DisplayExhibitInfoPanel();
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
        if(musInventory.ExhibitInformationPanel.activeSelf == true) {
            musInventory.DisplayExhibitInfoPanel();
        }
    }
    public void ReturnItemToInv() {
        itemDefinition.isDisplayed = false;
        GameObject.Find(itemDefinition.exhibitSlot).GetComponent<ExhibitSlot>().containsExhibit = false;
        itemDefinition.exhibitSlot = "";
        Destroy(gameObject);
        musStats.RemoveRating(itemDefinition.exhibitRatingAmount);
        Debug.Log("[Exhibit] Item returned to inventory");
    }
    public void StoreItem() {
        musInventory.StoreItem(this);
    }
    public void DisplayItem(){
        // Add item to empty object on screen.
        musStats.ApplyRating(itemDefinition.exhibitRatingAmount);
    }
    public void SellItem(Exhibit exhibitToSell) {
        Debug.Log("[Exhibit] SellItem - exhibitToSell posKey = " + itemDefinition.exhibitPosKey);
        musStats.ApplyWealth(itemDefinition.exhibitValueAmount);
        musInventory.RemoveItemFromInv(exhibitToSell);
        GameObject.Find(itemDefinition.exhibitSlot).GetComponent<ExhibitSlot>().containsExhibit = false;
        itemDefinition.isDisplayed = false;
        Destroy(gameObject);
    }
}
