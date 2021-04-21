using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ExhibitTypeDefinitions { PAINTING, FOSSIL, STATUE, ARTEFACT };
public enum ExhibitSizeDefinitions { SMALL, MEDIUM, LARGE };
public enum ExhibitQualityDefinitions { ABYSMAL, POOR, AVERAGE, GOOD, SUPERB};

[CreateAssetMenu(fileName = "NewExhibit", menuName = "Exhibit Object", order = 1)]
public class Exhibit_SO : ScriptableObject
{
    public string exhibitName = "New Exhibit";
    public string exhibitDescription = "New Description";
    public ExhibitTypeDefinitions exhibitType = ExhibitTypeDefinitions.PAINTING;
    public ExhibitSizeDefinitions exhibitSize = ExhibitSizeDefinitions.MEDIUM;
    public ExhibitQualityDefinitions exhibitQuality = ExhibitQualityDefinitions.AVERAGE;
    [Range(0f, 5f)] public float exhibitRatingAmount = 0f;
    [Range(0, 1000000)] public int exhibitValueAmount = 0;
    public Sprite exhibitIcon = null;
    public GameObject exhibitObject = null;
    public bool isDisplayed = false;
    public bool isInteractable = false;
    public bool isStorable = false;
}
