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

    // Inventory enforced singleton pattern
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

    // Stores an item in the inventory
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
            Debug.Log("[MuseumInventory - PickUp] Inventory is full");
        }
    }

    // The method used to store items from a save file, each item must pass through StoreItem() to ensure the UI is correctly set
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
        SaveInventory();
    }

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
            slotCounter++;
            inventoryNameSlots[slotCounter - 1].SetText(invEntry.invEntry.itemDefinition.exhibitName);
            inventoryDisplaySlots[slotCounter].sprite = invEntry.invEntry.itemDefinition.exhibitIcon;
            inventoryDisplaySlots[slotCounter].GetComponent<Button>().onClick.RemoveAllListeners();
            Debug.Log("[MuseumInventory] Listeners Removed");
            inventoryDisplaySlots[slotCounter].GetComponent<Button>().onClick.AddListener(() => PlaceExhibit(invEntry.invEntry.itemDefinition.exhibitObject, exhibitSlot.slotInRange.transform));
            Debug.Log("[FillInventoryDisplay] Listeners added, Inventory display filled, slotCounter = " + slotCounter + ", invEntry " + invEntry.invEntry.itemDefinition.exhibitName);
        }
    }
    // Clear the inventory display and onClick events
    public void ClearInventoryDisplay() {
        int slotCounter = 1;
        foreach(InventoryEntry ie in exhibitsInInventory) {
            slotCounter++;
            inventoryNameSlots[slotCounter - 1].SetText("");
            inventoryDisplaySlots[slotCounter].sprite = null;
            inventoryDisplaySlots[slotCounter].GetComponent<Button>().onClick.RemoveAllListeners();
            Debug.Log("[ClearInventoryDisplay] InventoryDisplay cleared.");
        }
    }

    // Places an item in the museum, if the player is at an empty slot which meets the correct criteria for placement
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

    // Remove the item from your inventory when sold - buggy
    public void RemoveItemFromInv(Exhibit exhibitToRemove) {
        // LAST METHOD!
    }

    // Fill a list with all the exhibit slots in a scene so any items which were placed when saved will be re-placed
    public void FillExhibitSlotList() {
        if(GameObject.FindGameObjectsWithTag("slot") != null) {
             foreach(GameObject transformObject in GameObject.FindGameObjectsWithTag("slot")) {
                slotsInRoom.Add(transformObject.GetComponent<Transform>());
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

