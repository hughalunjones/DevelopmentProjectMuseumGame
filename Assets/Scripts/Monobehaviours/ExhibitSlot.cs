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

    void Start() {
        museInventory = MuseumInventory.instance;
    }
    void Update() {
        if(containsExhibit == false && inRangeOfSlot == true && Input.GetKeyDown(KeyCode.F)) {
            AssignButtonUse();
            interactUI.SetActive(false);
        }else if(containsExhibit == true && inRangeOfSlot == true && Input.GetKeyDown(KeyCode.F)) {
            Debug.Log("[ExhibitSlot] This slot contains an exhibit");
        }
    }
    public void AssignButtonUse() {
        int slotCounter = 1;
        Debug.Log("[ExhibitSlot] Player interacted with: " + slotInRange);
        museInventory.DisplayInventory();
        try {
            // Somewhere here the button listeners are overwritten
            foreach (KeyValuePair<int, InventoryEntry> ie in museInventory.exhibitsInInventory) {
                slotCounter++;
                museInventory.inventoryDisplaySlots[slotCounter].GetComponent<Button>().onClick.RemoveAllListeners();
                Debug.Log("[ExhibitSlot] Listeners removed");
                museInventory.inventoryDisplaySlots[slotCounter].GetComponent<Button>().onClick.AddListener(() => 
                    museInventory.PlaceExhibit(museInventory.exhibitsInInventory[slotCounter - 1].invEntry.itemDefinition.exhibitObject, slotInRange.transform)
                );
                Debug.Log("[ExhibitSlot] slotCounter: " + slotCounter + " || slotInRange transform: " + slotInRange.transform + " || Exhibit at slotCounter: " + museInventory.exhibitsInInventory[slotCounter - 1].invEntry.itemDefinition.exhibitName);
            }
        }
        catch (NullReferenceException ex) {
            Debug.Log("[ExhibitSlot] Something was null: " + ex);
        }
    }
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
