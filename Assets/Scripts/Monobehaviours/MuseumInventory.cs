using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuseumInventory : MonoBehaviour
{    
    public static MuseumInventory instance;
    public MuseumStats museStats;
    GameObject foundStats;
    public GameObject InventoryDisplayHolder;
    public GameObject ConfirmationPopup;
    public Image[] inventoryDisplaySlots = new Image[60];
    int inventoryItemCap = 36;
    int idCount = 1;
    int slotNum = 1;
    bool addedItem = true;
    public Dictionary<int, InventoryEntry> exhibitsInInventory = new Dictionary<int, InventoryEntry>();
    public InventoryEntry exhibitEntry;
    
    private void Awake() {
        if(instance == null) {
            instance = this;
        }
        else {
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
        DontDestroyOnLoad(this);
        museStats = MuseumStats.instance;
        instance = this;
        exhibitEntry = new InventoryEntry(null, null);
        exhibitsInInventory.Clear();
        inventoryDisplaySlots = InventoryDisplayHolder.GetComponentsInChildren<Image>();
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.I)){
            DisplayInventory();
        }
        if (Input.GetKeyDown(KeyCode.G)) {
            exhibitsInInventory.Clear();
            ClearInventoryDisplay();
            Debug.Log("ClearInventoryDisplay() called");
        }
        //Check to see if the Exhibit has already been added - Prevent duplicate adds for 1 Exhibit
        if (!addedItem){
            if (Input.GetKeyDown(KeyCode.E)) {
                PickUp();
                Debug.Log("[MuseumInventory - Update] PickUp() called");              
            }
        }
    }
    // TODO: Complete functions    
    public void StoreItem(Exhibit exhibitToStore){
        addedItem = false;
        exhibitEntry.invEntry = exhibitToStore;
        exhibitEntry.entryName = exhibitToStore.itemDefinition.exhibitName;
        exhibitEntry.hbSprite = exhibitToStore.itemDefinition.exhibitIcon;
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
        exhibitEntry.invEntry.itemDefinition.isDisplayed = false;
        Debug.Log("[AddItemToInv] Exhibit added: " + exhibitEntry.invEntry);
        Destroy(newEntry.invEntry.gameObject);
        Destroy(exhibitEntry.invEntry.gameObject);
        Debug.Log("[AddItemToInv] gameObject destroyed");
        FillInventoryDisplay();
        idCount = IncreaseID(idCount);

        // Reset itemEntry
        exhibitEntry.invEntry = null;
        exhibitEntry.hbSprite = null;

        finishedAdding = true;
        Debug.Log("[AddItemToInv] finishedAdding:" + finishedAdding);
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
            Debug.Log("[FillInventoryDisplay] Inventory display filled");            
        }
    }
    void ClearInventoryDisplay() {
        int slotCounter = 1;
        foreach (KeyValuePair<int, InventoryEntry> ie in exhibitsInInventory) {
            Debug.Log("" + ie.Value.entryName);
            slotCounter++;
            inventoryDisplaySlots[slotCounter].sprite = null;
            Debug.Log("[FillInventoryDisplay] Inventory display cleared");
        }
    }

    // Place the exhibit in the museum when at the correct slot.
    public void PlaceExhibit(int ExhibitToUseID){
        
    }
    // Remove the item from your inventory when sold
    public void RemoveItemFromInv(bool finishedRemoving) {

    }
}