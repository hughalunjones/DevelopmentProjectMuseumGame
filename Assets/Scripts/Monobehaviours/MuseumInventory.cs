using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuseumInventory : MonoBehaviour
{    
    public static MuseumInventory instance;
    public MuseumStats museStats;
    public GameManager gameManager;
    GameObject foundStats;
    public GameObject InventoryDisplayHolder;
    public GameObject ConfirmationPopup;
    public Image[] inventoryDisplaySlots = new Image[60];
    int inventoryItemCap = 60;
    int idCount = 1;
    bool addedItem = true;
    public Dictionary<int, InventoryEntry> exhibitsInInventory = new Dictionary<int, InventoryEntry>();
    public InventoryEntry exhibitEntry;
 
    void Start()
    {
        instance = this;
        exhibitEntry = new InventoryEntry(null, null);
        exhibitsInInventory.Clear();

        inventoryDisplaySlots = InventoryDisplayHolder.GetComponentsInChildren<Image>();

        foundStats = GameObject.FindGameObjectWithTag("Player");
        museStats = foundStats.GetComponent<MuseumStats>();
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.I)){
            DisplayInventory();
        }
        //Check to see if the Exhibit has already been added - Prevent duplicate adds for 1 Exhibit
        if (!addedItem){
            if (Input.GetKeyDown(KeyCode.E)) {
                PickUp();
                Debug.Log("[MuseumInventory] Item picked up.");
            }
        }
    }
    // TODO: Complete functions    
    public void StoreItem(Exhibit exhibitToStore){
        addedItem = false;
        exhibitEntry.invEntry = exhibitToStore;
        exhibitEntry.hbSprite = exhibitToStore.itemDefinition.exhibitIcon;
        exhibitEntry.inventorySlot = 1;
    }
    public void PickUp() {
        // Check item was stored properly.
        if (exhibitEntry.invEntry) {
            // Check there is space
            if (exhibitsInInventory.Count == 0) {
                addedItem = AddItemToInv(addedItem);
                Debug.Log("[MuseumInventory] Item added to inventory");
            }else if(exhibitsInInventory.Count == inventoryItemCap) {
                // TODO: Show prompt/UI in game to show inventory is full.
                Debug.Log("[MuseumInventory] Inventory is full");
            }
        }
    }

    // Add a copy of the item to the inventory and display the corresponding sprite.
    bool AddItemToInv(bool finishedAdding){
        exhibitsInInventory.Add(idCount, new InventoryEntry(Instantiate(exhibitEntry.invEntry), exhibitEntry.hbSprite));
        Destroy(exhibitEntry.invEntry.gameObject);
        FillInventoryDisplay();
        idCount = IncreaseID(idCount);

        // Reset itemEntry
        exhibitEntry.invEntry = null;
        exhibitEntry.hbSprite = null;

        finishedAdding = true;
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
    void DisplayInventory(){
       if (InventoryDisplayHolder.activeSelf == true){
           InventoryDisplayHolder.SetActive(false);
       }
       else {
           InventoryDisplayHolder.SetActive(true);
        }
    }
    void DisplayPopup() {
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
            // if(exhibit.type == FOSSIL){ slotCounter = 30 }
            slotCounter++;
            inventoryDisplaySlots[slotCounter].sprite = ie.Value.hbSprite;
        }
    }

    // Place the exhibit in the museum when at the correct slot.
    public void TriggerExhibitUse(int ExhibitToUseID){
        
    }
}