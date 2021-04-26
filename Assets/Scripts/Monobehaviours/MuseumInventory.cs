using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuseumInventory : MonoBehaviour {
    public static MuseumInventory instance;
    public MuseumStats museStats;
    public GameObject InventoryDisplayHolder;
    public GameObject ExhibitInformationPanel;
    public GameObject ConfirmationPopup;
    public Image[] inventoryDisplaySlots = new Image[60];
    public int inventoryItemCap = 36;
    int idCount = 1;
    int slotNum = 1;
    bool addedItem = true;
    public Dictionary<int, InventoryEntry> exhibitsInInventory = new Dictionary<int, InventoryEntry>();
    public InventoryEntry exhibitEntry;
    public bool inventoryDisplayIsActive = false;
    ExhibitSlot slotInRange;

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
        Events.SaveInitiated += Save;
        museStats = MuseumStats.instance;
        instance = this;
        exhibitEntry = new InventoryEntry(null, null);
        exhibitsInInventory.Clear();
        inventoryDisplaySlots = InventoryDisplayHolder.GetComponentsInChildren<Image>();
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
            }
            else if (exhibitsInInventory.Count == inventoryItemCap) {
                // TODO: Show prompt/UI in game to show inventory is full.
                Debug.Log("[MuseumInventory - PickUp] Inventory is full");
            }
        }
    }

    // Add a copy of the item to the inventory and display the corresponding sprite.
    bool AddItemToInv(bool finishedAdding) {
        InventoryEntry newEntry = new InventoryEntry(Instantiate(exhibitEntry.invEntry), exhibitEntry.hbSprite);
        exhibitsInInventory.Add(idCount, newEntry);
        Debug.Log("[AddItemToInv] Exhibit added: " + exhibitEntry.invEntry);
        newEntry.invEntry.itemDefinition.exhibitPosKey = idCount;
        newEntry.invEntry.itemDefinition.isDisplayed = false;
        Destroy(newEntry.invEntry.gameObject);
        Destroy(exhibitEntry.invEntry.gameObject);
        Debug.Log("[AddItemToInv] " + newEntry.invEntry.gameObject + "  destroyed");
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
    void FillInventoryDisplay() {
        int slotCounter = 1;
        foreach (KeyValuePair<int, InventoryEntry> ie in exhibitsInInventory) {
            // add the name of the inventory to the inventory display.
            // if(exhibit.type == FOSSIL){ slotCounter = 30 }
            slotCounter++;
            inventoryDisplaySlots[slotCounter].sprite = ie.Value.hbSprite;
            Debug.Log("[FillInventoryDisplay] Inventory display filled, slotCounter = " + slotCounter);
        }
    }

    // Place the exhibit in the museum when at the correct slot. - !!MOST IMPORTANT METHOD RIGHT NOW!!
    /*  TODO:
     *  Stop button method duplication
     */
    public void PlaceExhibit(int invKey, Transform slotToHold) {
        try {
            Exhibit exhibitToPlace = exhibitsInInventory[invKey].invEntry;
            if (!exhibitToPlace.itemDefinition.isDisplayed) {
                if (exhibitToPlace.itemDefinition.exhibitSize.ToString().Equals(slotToHold.GetComponent<ExhibitSlot>().slotInRange.slotDefinition.exhibitSlotSize.ToString()) 
                    && exhibitToPlace.itemDefinition.exhibitType.ToString().Equals(slotToHold.GetComponent<ExhibitSlot>().slotInRange.slotDefinition.exhibitSlotType.ToString())) {
                    // Create a new version of the stored exhibit
                    Instantiate(exhibitToPlace.itemDefinition.exhibitObject, slotToHold.transform.position, Quaternion.Euler(0, 0, 0), slotToHold);
                    exhibitToPlace.exhibitSlot = slotToHold.GetComponent<ExhibitSlot>().slotInRange;
                    // Apply the rating value of the exhibit
                    museStats.ApplyRating(exhibitToPlace.itemDefinition.exhibitRatingAmount);
                    Debug.Log("[PlaceExhibit] Exhibit placed at slot: " + slotToHold.name + ", museStats: " + museStats.GetRating());
                    // Set the boolean values to true for future checks.
                    slotToHold.GetComponent<ExhibitSlot>().containsExhibit = true;
                    exhibitToPlace.itemDefinition.isDisplayed = true;
                    DisplayInventory();
                }else {
                    Debug.Log("[MuseumInventory - PlaceExhibit] The slot size/type and exhibit size/type do not match " + exhibitToPlace.itemDefinition.exhibitSize + " -> " + slotToHold.GetComponent<ExhibitSlot>().slotInRange.slotDefinition.exhibitSlotSize + " || " + exhibitToPlace.itemDefinition.exhibitType + " -> " + slotToHold.GetComponent<ExhibitSlot>().slotInRange.slotDefinition.exhibitSlotType);
                }                 
            }
            else {
                Debug.Log("[MuseumInventory - PlaceExhibit] Exhibit already placed - " + exhibitToPlace.itemDefinition.exhibitName);
            }
        }catch(KeyNotFoundException knfe) {
            Debug.Log("[MuseumInventory] KeyNotFoundException || " + knfe.Data);
        }
        
    }

    // Remove the item from your inventory when sold
    public void RemoveItemFromInv(int itemNum) {
        exhibitsInInventory.Remove(itemNum);
        inventoryDisplaySlots[itemNum + 1].GetComponent<Button>().onClick.RemoveAllListeners();
        inventoryDisplaySlots[itemNum + 1].sprite = null;
        Debug.Log("[MuseumInventory] Current Wealth: " + museStats.GetWealth());
    }

    // Save the inventory
    void Save() {
        GameManager.Save<Dictionary<int, InventoryEntry>>(exhibitsInInventory, "Inventory");
    }
}

