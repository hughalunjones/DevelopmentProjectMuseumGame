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
            Debug.Log("[ExhibitSlot] Player interacted with: " + slotInRange);
            museInventory.exhibitSlot = slotInRange;
            museInventory.DisplayInventory();
            interactUI.SetActive(false);
        }else if(containsExhibit == true && inRangeOfSlot == true && Input.GetKeyDown(KeyCode.F)) {
            Debug.Log("[ExhibitSlot] This slot contains an exhibit");
        }
    }
/*    public void AssignButtonUse() {
        int slotCounter = 1;
        Debug.Log("[ExhibitSlot] Player interacted with: " + slotInRange);
        museInventory.DisplayInventory();
        try {
            // Somewhere here the button listeners are overwritten
            foreach (InventoryEntry invEntry in museInventory.exhibitsInInventory) {
                *//* using slotCounter here is causing a bug with the sell method, slotCounter and the exhibit
                   key don't line up after the second item is added.*//*
                slotCounter++;
                museInventory.inventoryDisplaySlots[slotCounter].GetComponent<Button>().onClick.RemoveAllListeners();
                Debug.Log("[ExhibitSlot] Listeners removed");
                museInventory.inventoryDisplaySlots[slotCounter].GetComponent<Button>().onClick.AddListener(() => 
                    museInventory.PlaceExhibit(museInventory.exhibitsInInventory[slotCounter].inventorySlot, slotInRange.transform)
                );
                Debug.Log("[ExhibitSlot] inventory slotCounter: " + slotCounter + " || slotInRange transform: " + slotInRange.transform + " || Exhibit at inventory slotCounter: " + museInventory.exhibitsInInventory[slotCounter - 2].invEntry.itemDefinition.exhibitName);
            }
        }
        catch (ArgumentOutOfRangeException aoore) {
            Debug.Log("[ExhibitSlot] Out of range: " + aoore);
        }
    }*/
    void OnTriggerEnter2D(Collider2D triggerCollider) {
        if (triggerCollider.tag == "Player") {
            if(containsExhibit == false) {
                interactUI.SetActive(true);              
            }            
            slotInRange = this;
            inRangeOfSlot = true;
            Debug.Log("[ExhibitSlot] Player Detected at: " + slotInRange);
        }
    }
    void OnTriggerExit2D(Collider2D triggerCollider) {
        if (triggerCollider.tag == "Player") {
            interactUI.SetActive(false);
            slotInRange = null;
            inRangeOfSlot = false;
            Debug.Log("[ExhibitSlot] Player Left");
        }
        if (museInventory.inventoryDisplayIsActive) {
            museInventory.DisplayInventory();
        }
    }
}
