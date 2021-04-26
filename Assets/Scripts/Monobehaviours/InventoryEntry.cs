using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryEntry
{
    public Exhibit invEntry;
    public int inventorySlot;
    public Sprite hbSprite;
    public string entryName;

    public InventoryEntry(Exhibit invEntry, Sprite hbSprite)
    {
        this.invEntry = invEntry;
        this.inventorySlot = 0;
        this.entryName = "";
        this.hbSprite = hbSprite;
    }
}
