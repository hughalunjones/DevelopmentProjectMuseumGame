using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MuseumInventory : MonoBehaviour {
    public static MuseumInventory instance;
    public MuseumStats museStats;
    public GameObject InventoryDisplayHolder;
    public GameObject ExhibitInformationPanel;
    public GameObject ConfirmationPopup;
    public ExhibitSlot exhibitSlot;
    public Image[] inventoryDisplaySlots = new Image[60];
    public TextMeshProUGUI[] inventoryNameSlots = new TextMeshProUGUI[60];
    public int inventoryItemCap = 36;
    int slotNum = 0;
    public List<Exhibit> allExhibits = new List<Exhibit>();
    public List<Transform> slotsInRoom = new List<Transform>();
    public List<InventoryEntry> exhibitsInInventory { get; set; } = new List<InventoryEntry>();
    public bool inventoryDisplayIsActive = false;
    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(this.gameObject);
        }
    }
    void Start() {
        DontDestroyOnLoad(this);
        museStats = MuseumStats.instance;
        Events.SaveInitiated += SaveInventory;
        Events.LoadInitiated += LoadInventory;
        instance = this;
        exhibitsInInventory.Clear();
        inventoryDisplaySlots = InventoryDisplayHolder.GetComponentsInChildren<Image>();
        inventoryNameSlots = InventoryDisplayHolder.GetComponentsInChildren<TextMeshProUGUI>();
    }

    void Update() {
        if (GameManager.Instance.GetCurrentLevelName() != "DigSite" && Input.GetKeyDown(KeyCode.I)) {
            DisplayInventory();
        }
        if (Input.GetKeyDown(KeyCode.G)) {
            exhibitsInInventory.Clear();
            Debug.Log("ClearInventoryDisplay() called");
        }
    }
    // TODO: Complete functions    
    public void StoreItem(Exhibit exhibitToStore) {
        if (exhibitsInInventory.Count != inventoryItemCap) {
            InventoryEntry exhibitEntry = new InventoryEntry(exhibitToStore, slotNum);
            exhibitEntry.invEntry = exhibitToStore;
            exhibitEntry.invEntry.itemDefinition.exhibitPosKey = slotNum;
            exhibitEntry.entryName = exhibitToStore.itemDefinition.exhibitName;
            exhibitEntry.inventorySlot = slotNum;
            slotNum++;
            Debug.Log("[StoreItem] " + exhibitToStore.itemDefinition.exhibitName + " stored at Inventory slot:" + exhibitEntry.inventorySlot + " and isDisplayed value: " + exhibitEntry.invEntry.itemDefinition.isDisplayed);
            exhibitsInInventory.Add(exhibitEntry);
            Destroy(exhibitEntry.invEntry.gameObject);
            FillInventoryDisplay();
        } else if (exhibitsInInventory.Count == inventoryItemCap) {
            // TODO: Show prompt/UI in game to show inventory is full.
            Debug.Log("[MuseumInventory - PickUp] Inventory is full");
        }
        // PickUp();
    }
    public void StoreItemsFromLoad(List<ExhibitSaveData> exhibitSaves) {
        List<Exhibit> exhibitsToLoad = new List<Exhibit>();
        foreach(ExhibitSaveData eSD in exhibitSaves) {
            foreach(Exhibit exhibit in allExhibits) {
                if(exhibit.itemDefinition.exhibitKeyID == eSD.idKeyData) {
                    exhibit.itemDefinition.exhibitPosKey = eSD.posKeyData;
                    exhibit.itemDefinition.isDisplayed = eSD.isDisplayedData;
                    if (exhibit.itemDefinition.isDisplayed == true) {
                       exhibit.itemDefinition.exhibitSlot = eSD.transformNameData;
                    }
                    exhibitsToLoad.Add(exhibit);
                }
            }
        }
        foreach(Exhibit loadExhibit in exhibitsToLoad) {
            StoreItem(loadExhibit);                     
        }
    }
   /* public void PickUp() {
        // Check item was stored properly.
        if (exhibitEntry.invEntry) {
            Debug.Log("[Museum Inventory - PickUp] Item stored correctly");
            // Check there is space
            if (exhibitsInInventory.Count != inventoryItemCap) {
                addedItem = AddItemToInv(addedItem);
                Debug.Log("[MuseumInventory - PickUp] Item added to inventory, Count: " + exhibitsInInventory.Count);
            }
            else if (exhibitsInInventory.Count == inventoryItemCap) {
                // TODO: Show prompt/UI in game to show inventory is full.
                Debug.Log("[MuseumInventory - PickUp] Inventory is full");
            }
        }
    }
    /*
    // Add a copy of the item to the inventory and display the corresponding sprite.
    bool AddItemToInv(bool finishedAdding) {
        InventoryEntry newEntry = new InventoryEntry(Instantiate(exhibitEntry.invEntry), slotNum);
        exhibitsInInventory.Add(newEntry);
        Debug.Log("[AddItemToInv] Exhibit added: " + newEntry.invEntry);
        newEntry.invEntry.itemDefinition.exhibitPosKey = idCount;
        newEntry.invEntry.itemDefinition.isDisplayed = false;
        Destroy(newEntry.invEntry.gameObject);
        Destroy(exhibitEntry.invEntry.gameObject);
        Debug.Log("[AddItemToInv] " + newEntry.invEntry.gameObject + "  destroyed");
        Debug.Log("[AddItemToInv] " + exhibitEntry.invEntry.gameObject + "  destroyed");
        FillInventoryDisplay();
        idCount = IncreaseID(idCount);

        // Reset exhibitEntry
        //exhibitEntry.invEntry = null;

        finishedAdding = true;
        Debug.Log("[AddItemToInv] finishedAdding:" + finishedAdding);
        foreach (InventoryEntry invEntry in exhibitsInInventory) {
            Debug.Log("Name = " + invEntry.invEntry.itemDefinition.exhibitName);
        }
        return finishedAdding;
    }*/
   /* int IncreaseID(int currentID) {
        int newID = 1;

        for (int itemCount = 1; itemCount <= exhibitsInInventory.Count; itemCount++) {
            if (exhibitsInInventory.Contains(exhibitEntry)) {
                newID += 1;
            }
            else return newID;
        }
        return newID;
    }*/

    // Display the inventory menu, exhibit information panel and the storage confirmation pop-up. 
    public void DisplayInventory() {
        if (InventoryDisplayHolder.activeSelf == true) {
            InventoryDisplayHolder.SetActive(false);
            inventoryDisplayIsActive = false;
        }
        else {
            InventoryDisplayHolder.SetActive(true);
            inventoryDisplayIsActive = true;
        }
    }
    public void DisplayExhibitInfoPanel() {
        if (ExhibitInformationPanel.activeSelf == true) {
            ExhibitInformationPanel.SetActive(false);
        }
        else {
            ExhibitInformationPanel.SetActive(true);
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
    public void FillInventoryDisplay() {
        int slotCounter = 1;
        foreach (InventoryEntry invEntry in exhibitsInInventory) {
            // add the name of the inventory to the inventory display.
            // if(exhibit.type == FOSSIL){ slotCounter = 30 }
            slotCounter++;
            inventoryNameSlots[slotCounter - 1].SetText(invEntry.invEntry.itemDefinition.exhibitName);
            inventoryDisplaySlots[slotCounter].sprite = invEntry.invEntry.itemDefinition.exhibitIcon;
            inventoryDisplaySlots[slotCounter].GetComponent<Button>().onClick.RemoveAllListeners();
            Debug.Log("[MuseumInventory] Listeners Removed");
            inventoryDisplaySlots[slotCounter].GetComponent<Button>().onClick.AddListener(() => PlaceExhibit(invEntry.invEntry.itemDefinition.exhibitObject, exhibitSlot.slotInRange.transform));
            Debug.Log("[FillInventoryDisplay] Listeners added, Inventory display filled, slotCounter = " + slotCounter + ", invEntry " + invEntry.invEntry.itemDefinition.exhibitName);
        }
    }
    public void PlaceExhibit(GameObject exhibitObject, Transform slotToHold) {
        try {
            Exhibit exhibitToPlace = exhibitObject.GetComponent<Exhibit>();
            exhibitToPlace.itemDefinition.exhibitSlot = slotToHold.GetComponent<Transform>().name;
            // check the target slot is not null
            if(exhibitToPlace.itemDefinition.exhibitSlot != "") {
                // check whether the exhibit has already been displayed
                if (!exhibitToPlace.itemDefinition.isDisplayed) {
                    // check whether the exhibit can be placed in the desired slot
                    if (exhibitToPlace.itemDefinition.exhibitSize.ToString().Equals(slotToHold.GetComponent<ExhibitSlot>().slotInRange.slotDefinition.exhibitSlotSize.ToString())
                        && exhibitToPlace.itemDefinition.exhibitType.ToString().Equals(slotToHold.GetComponent<ExhibitSlot>().slotInRange.slotDefinition.exhibitSlotType.ToString())) {
                        // Create a new version of the stored exhibit
                        Instantiate(exhibitToPlace.itemDefinition.exhibitObject, new Vector3(slotToHold.position.x, slotToHold.position.y, -0.3f), Quaternion.Euler(0, 0, 0), slotToHold);
                        // Apply the rating value of the exhibit
                        museStats.ApplyRating(exhibitToPlace.itemDefinition.exhibitRatingAmount);
                        Debug.Log("[PlaceExhibit] Exhibit placed at slot: " + slotToHold.name + ", museStats: " + museStats.GetRatingRaw());
                        // Set the boolean values to true for future checks.
                        slotToHold.GetComponent<ExhibitSlot>().containsExhibit = true;
                        exhibitToPlace.itemDefinition.isDisplayed = true;
                        DisplayInventory();
                        SaveInventory();
                    }
                    else {
                        Debug.Log("[MuseumInventory - PlaceExhibit] The slot size/type and exhibit size/type do not match " + exhibitToPlace.itemDefinition.exhibitSize + " -> " + slotToHold.GetComponent<ExhibitSlot>().slotInRange.slotDefinition.exhibitSlotSize + " || " + exhibitToPlace.itemDefinition.exhibitType + " -> " + slotToHold.GetComponent<ExhibitSlot>().slotInRange.slotDefinition.exhibitSlotType);
                    }
                }
                else {
                    Debug.Log("[MuseumInventory - PlaceExhibit] Exhibit already placed - " + exhibitToPlace.itemDefinition.exhibitName);
                }
            }else {
                Debug.Log("[MuseumInventory] exhibitSlot = null");
            }            
        }catch(KeyNotFoundException knfe) {
            Debug.Log("[MuseumInventory] KeyNotFoundException || " + knfe.Data);
        }
        
    }

    // Remove the item from your inventory when sold - currently not attached to any buttons
    public void RemoveItemFromInv(Exhibit exhibitToRemove) {
        int listNum = exhibitToRemove.itemDefinition.exhibitPosKey;
        Debug.Log("[MuseumInventory] RemoveItemFromInv - listNum to remove = " + listNum);
        exhibitsInInventory.RemoveAt(listNum);
        inventoryDisplaySlots[listNum + 2].GetComponent<Button>().onClick.RemoveAllListeners();
        inventoryDisplaySlots[listNum + 2].sprite = null;
        Debug.Log("[MuseumInventory] Current Wealth: " + museStats.GetWealth());
    }
    public void FillExhibitSlotList() {
        if(GameObject.FindGameObjectsWithTag("slot") != null) {
             foreach(GameObject transformObject in GameObject.FindGameObjectsWithTag("slot")) {
                slotsInRoom.Add(transformObject.GetComponent<Transform>());
                Debug.Log("[MuseumInventory] slotsInRoom: " + transformObject.name);
            }
        }                   
    }
    public void ClearExhibitSlotList() {
        slotsInRoom.Clear();
        Debug.Log("[MuseumInventory] Slot list cleared");
    }

    // If an item was placed when the player last saved and quit or unloaded the scene, re-place them.
    public void PlaceSavedPlacedItems() {
        foreach(InventoryEntry ie in exhibitsInInventory) {
            foreach(Transform slotTransform in slotsInRoom) {
                if (ie.invEntry.itemDefinition.isDisplayed == true && ie.invEntry.itemDefinition.exhibitSlot == slotTransform.name) {
                    Instantiate(ie.invEntry.itemDefinition.exhibitObject, new Vector3(slotTransform.position.x, slotTransform.position.y, -0.3f), Quaternion.Euler(0, 0, 0), slotTransform);
                    ie.invEntry.itemDefinition.isDisplayed = true;
                    slotTransform.GetComponent<ExhibitSlot>().containsExhibit = true;
                }
            }
        }
    }

    // Save the inventory
    void SaveInventory() {
        List<ExhibitSaveData> exhibitSaves = new List<ExhibitSaveData>();
        foreach(InventoryEntry ie in exhibitsInInventory) {
            ExhibitSaveData exhibitSaveData = new ExhibitSaveData();
            // Set each of the values which change 
            exhibitSaveData.idKeyData = ie.invEntry.itemDefinition.exhibitKeyID;
            exhibitSaveData.posKeyData = ie.invEntry.itemDefinition.exhibitPosKey;
            exhibitSaveData.isDisplayedData = ie.invEntry.itemDefinition.isDisplayed;
            if (ie.invEntry.itemDefinition.exhibitSlot != "" && ie.invEntry.itemDefinition.isDisplayed) {
                exhibitSaveData.transformNameData = ie.invEntry.itemDefinition.exhibitSlot;                
            }
            Debug.Log("[MuseumInventory - SaveInventory()] exhibitSaveData - " + exhibitSaveData.posKeyData + " isDisplayed: " + exhibitSaveData.isDisplayedData);
            exhibitSaves.Add(exhibitSaveData);
        }
        SaveLoad.Save<List<ExhibitSaveData>>(exhibitSaves, "inventoryExhibitData");
    }
    void LoadInventory() {
        // Clear the inventory to stop duplication of the inventory
        exhibitsInInventory.Clear();
        Debug.Log("[MuseumInventory] Inventory Cleared");
        if (SaveLoad.SaveExists("inventoryExhibitData")) {
           StoreItemsFromLoad(SaveLoad.Load<List<ExhibitSaveData>>("inventoryExhibitData"));
            if(slotsInRoom != null) {
                PlaceSavedPlacedItems();
            }
        }
    }
}

