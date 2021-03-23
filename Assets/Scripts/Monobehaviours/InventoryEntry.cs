using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryEntry
{
    public Exhibit invEntry;
    public int inventorySlot;
    public Sprite hbSprite;

    public InventoryEntry(Exhibit invEntry, Sprite hbSprite)
    {
        this.invEntry = invEntry;
        this.inventorySlot = 0;
        this.hbSprite = hbSprite;
    }
}
