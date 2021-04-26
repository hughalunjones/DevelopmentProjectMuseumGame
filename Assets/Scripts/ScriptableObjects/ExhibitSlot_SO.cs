using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ExhibitSlotTypeDefinitions { PAINTING, FOSSIL, STATUE, ARTEFACT };
public enum ExhibitSlotSizeDefinitions { SMALL, MEDIUM, LARGE };

[CreateAssetMenu(fileName = "NewSlot", menuName = "Exhibit Slot Object", order = 1)]
public class ExhibitSlot_SO : ScriptableObject {
    public ExhibitSlotTypeDefinitions exhibitSlotType = ExhibitSlotTypeDefinitions.PAINTING;
    public ExhibitSlotSizeDefinitions exhibitSlotSize = ExhibitSlotSizeDefinitions.SMALL;
}
