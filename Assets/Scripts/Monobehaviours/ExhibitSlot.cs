using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExhibitSlot : MonoBehaviour
{
    public MuseumInventory museInventory;
    public bool inRangeOfSlot, containsExhibit;
    public GameObject interactUI;
    public ExhibitSlot slotInRange;  
    public ExhibitSlot_SO slotDefinition;

    void Start() {
        museInventory = MuseumInventory.instance;
    }
    void Update() {
        if(containsExhibit == false && inRangeOfSlot == true && Input.GetKeyDown(KeyCode.F)) {
            museInventory.exhibitSlot = slotInRange;
            museInventory.DisplayInventory();
            interactUI.SetActive(false);
        }else if(containsExhibit == true && inRangeOfSlot == true && Input.GetKeyDown(KeyCode.F)) {
            Debug.Log("[ExhibitSlot] This slot contains an exhibit");
        }
    }
    void OnTriggerEnter2D(Collider2D triggerCollider) {
        if (triggerCollider.tag == "Player") {
            if(containsExhibit == false) {
                interactUI.SetActive(true);              
            }            
            slotInRange = this;
            inRangeOfSlot = true;
        }
    }
    void OnTriggerExit2D(Collider2D triggerCollider) {
        if (triggerCollider.tag == "Player") {
            interactUI.SetActive(false);
            slotInRange = null;
            inRangeOfSlot = false;
        }
        if (museInventory.inventoryDisplayIsActive) {
            museInventory.DisplayInventory();
        }
    }
}
