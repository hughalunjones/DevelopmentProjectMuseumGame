using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class InventoryEntry
{
    public Exhibit invEntry;
    public int inventorySlot;
    public string entryName;

    public InventoryEntry(Exhibit invEntry, int key) {
        this.invEntry = invEntry;
        this.inventorySlot = key;
        this.entryName = "";
    }
}
