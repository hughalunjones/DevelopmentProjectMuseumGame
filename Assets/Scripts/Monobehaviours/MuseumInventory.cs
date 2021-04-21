using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MuseumInventory : MonoBehaviour
{    
    public static MuseumInventory instance;
    public MuseumStats museStats;
    public GameObject InventoryDisplayHolder;
    public GameObject ConfirmationPopup;
    public Image[] inventoryDisplaySlots = new Image[60];
    int inventoryItemCap = 36;
    int idCount = 1;
    int slotNum = 1;
    bool addedItem = true;
    public Dictionary<int, InventoryEntry> exhibitsInInventory = new Dictionary<int, InventoryEntry>();
    public InventoryEntry exhibitEntry;
    ExhibitSlot slotInRange;
    
    private void Awake() {
        if(instance == null) {
            instance = this;
        }
        else {
            Destroy(this.gameObject);
        }
    }
    void Start() {
        DontDestroyOnLoad(this);
        museStats = MuseumStats.instance;
        instance = this;
        exhibitEntry = new InventoryEntry(null, null);
        exhibitsInInventory.Clear();
        inventoryDisplaySlots = InventoryDisplayHolder.GetComponentsInChildren<Image>();
    }

    void Update(){
        if (GameManager.Instance.GetCurrentLevelName() != "DigSite" && Input.GetKeyDown(KeyCode.I)){
            DisplayInventory();
        }
        if (Input.GetKeyDown(KeyCode.G)) {
            exhibitsInInventory.Clear();
            Debug.Log("ClearInventoryDisplay() called");
        }
    }
    // TODO: Complete functions    
    public void StoreItem(Exhibit exhibitToStore){
        addedItem = false;
        exhibitEntry.invEntry = exhibitToStore;
        exhibitEntry.entryName = exhibitToStore.itemDefinition.exhibitName;
        exhibitEntry.hbSprite = exhibitToStore.itemDefinition.exhibitIcon;
        exhibitToStore.itemDefinition.isDisplayed = false;
        exhibitEntry.inventorySlot = slotNum;
        slotNum++;
        Debug.Log("[StoreItem] " + exhibitToStore + ", Inventory slot:" + exhibitEntry.inventorySlot);
        PickUp();
    }
    public void PickUp() {
        // Check item was stored properly.
        if (exhibitEntry.invEntry) {
            Debug.Log("[Museum Inventory - PickUp] Item stored correctly");
            // Check there is space
            if (exhibitsInInventory.Count != inventoryItemCap) {
                addedItem = AddItemToInv(addedItem);
                Debug.Log("[MuseumInventory - PickUp] Item added to inventory, Count: " + exhibitsInInventory.Count);
            }else if(exhibitsInInventory.Count == inventoryItemCap) {
                // TODO: Show prompt/UI in game to show inventory is full.
                Debug.Log("[MuseumInventory - PickUp] Inventory is full");
            }
        }
    }

    // Add a copy of the item to the inventory and display the corresponding sprite.
    bool AddItemToInv(bool finishedAdding){
        InventoryEntry newEntry = new InventoryEntry(Instantiate(exhibitEntry.invEntry), exhibitEntry.hbSprite);
        exhibitsInInventory.Add(idCount, newEntry);
        Debug.Log("[AddItemToInv] Exhibit added: " + exhibitEntry.invEntry);
        newEntry.invEntry.itemDefinition.isDisplayed = false;
        Destroy(newEntry.invEntry.gameObject);
        Destroy(exhibitEntry.invEntry.gameObject);
        Debug.Log("[AddItemToInv] "+ newEntry.invEntry.gameObject + "  destroyed");
        Debug.Log("[AddItemToInv] " + exhibitEntry.invEntry.gameObject + "  destroyed");
        FillInventoryDisplay();
        idCount = IncreaseID(idCount);

        // Reset exhibitEntry
        exhibitEntry.invEntry = null;
        exhibitEntry.hbSprite = null;

        finishedAdding = true;
        Debug.Log("[AddItemToInv] finishedAdding:" + finishedAdding);
        foreach (KeyValuePair<int, InventoryEntry> kvp in exhibitsInInventory) {
            Debug.Log("Key = " + kvp.Key + "|| Value = " + kvp.Value.invEntry.itemDefinition.exhibitName);
        }
        return finishedAdding;
    }
    int IncreaseID(int currentID) {
        int newID = 1;

        for (int itemCount = 1; itemCount <= exhibitsInInventory.Count; itemCount++) {
            if (exhibitsInInventory.ContainsKey(newID)) {
                newID += 1;
            }
            else return newID;
        }
        return newID;
    }

    // Display the inventory menu. 
    public void DisplayInventory(){
       if (InventoryDisplayHolder.activeSelf == true){
           InventoryDisplayHolder.SetActive(false);
       }
       else {
           InventoryDisplayHolder.SetActive(true);
       }
    }
    public void DisplayPopup() {
        if (ConfirmationPopup.activeSelf == true) {
            ConfirmationPopup.SetActive(false);
        }
        else {
            ConfirmationPopup.SetActive(true);
        }
    }
    // Fill the inventory with the corresponding sprite.
    void FillInventoryDisplay(){
        int slotCounter = 1;
        foreach(KeyValuePair<int, InventoryEntry> ie in exhibitsInInventory) {
            // add the name of the inventory to the inventory display.
            // if(exhibit.type == FOSSIL){ slotCounter = 30 }
            slotCounter++;
            inventoryDisplaySlots[slotCounter].sprite = ie.Value.hbSprite;            
            Debug.Log("[FillInventoryDisplay] Inventory display filled, slotCounter = " + slotCounter);            
        }        
    }

    // Place the exhibit in the museum when at the correct slot. - !!MOST IMPORTANT METHOD RIGHT NOW!!
    /*  TODO:
     *  Override the slot 
     */
    public void PlaceExhibit(int invButtonNum, Transform slotToHold) {
        if (!exhibitsInInventory[invButtonNum].invEntry.itemDefinition.isDisplayed) {
            // Create a new version of the stored exhibit
            Instantiate(exhibitsInInventory[invButtonNum].invEntry.itemDefinition.exhibitObject, slotToHold.transform.position, Quaternion.Euler(0,0,0), slotToHold);
            // Apply the rating value of the exhibit
            museStats.ApplyRating(exhibitsInInventory[invButtonNum].invEntry.itemDefinition.exhibitRatingAmount);
            Debug.Log("[PlaceExhibit] Exhibit placed at slot: " + slotToHold.name + ", museStats: " + museStats.GetRating());
            // Set the boolean values to true for future checks.
            slotToHold.GetComponent<ExhibitSlot>().containsExhibit = true;
            exhibitsInInventory[invButtonNum].invEntry.itemDefinition.isDisplayed = true;
            foreach(KeyValuePair<int, InventoryEntry> ie in exhibitsInInventory) {
                slotNum++;
                inventoryDisplaySlots[slotNum].GetComponent<Button>().onClick.RemoveAllListeners();
                Debug.Log("[MuseumInventory] Event Count: " + inventoryDisplaySlots[slotNum].GetComponent<Button>().onClick.GetPersistentEventCount());
            }
            DisplayInventory();
            GameManager.Instance.TogglePause();
            UIManager.Instance.GetComponentInChildren<PauseMenu>().gameObject.SetActive(false); // error here
        }
        else {
            Debug.Log("[MuseumInventory - PlaceExhibit] Exhibit already placed");
        }
    }

    // Remove the item from your inventory when sold
    public void RemoveItemFromInv(int itemNum) {
       
    }
} 

