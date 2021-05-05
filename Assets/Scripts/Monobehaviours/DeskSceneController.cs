using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeskSceneController : MonoBehaviour {
    bool inRange;
    public GameObject officeUI, confirmationUI, oosUI, oocUI, newDayUI, newDayText, ratingText, wealthText, exhibitsOnDisplayText;
    public MuseumStats museStats;
    public MuseumInventory museumInventory;
    int stamina = MuseumStats.instance.museumDefinition.playerStamina;
    int admissionFee = 15;
    int exhibitsOnDisplay, donationTotal, visitorTotal, admissionTotal;
    List<InventoryEntry> exhibitsOnDisplayList = new List<InventoryEntry>();
    void Start() {
        museStats = MuseumStats.instance;
        museumInventory = MuseumInventory.instance;
    }
    void Update() {
        wealthText.GetComponent<TextMeshProUGUI>().SetText(museStats.GetWealth().ToString());
        ratingText.GetComponent<TextMeshProUGUI>().SetText(museStats.GetRoundedRating().ToString());
        exhibitsOnDisplayText.GetComponent<TextMeshProUGUI>().SetText(GetExhibitsOnDisplay().ToString());
        if (inRange && Input.GetKeyDown(KeyCode.F)) {
            DisplayOfficeUI();
        }
    }
    #region UI Display Methods
    public void DisplayOfficeUI() {
        if(officeUI.activeSelf == true) {
            officeUI.SetActive(false);
        }
        else {
            officeUI.SetActive(true);
        }
    }
    public void DisplayConfirmation() {
        if (confirmationUI.activeSelf == true) {
            confirmationUI.SetActive(false);
        }
        else {
            confirmationUI.SetActive(true);
        }
    }
    public void DisplayOOSUI() {
        if (oosUI.activeSelf == true) {
            oosUI.SetActive(false);
        }
        else {
            oosUI.SetActive(true);
        }
    }
    public void DisplayOOCUI() {
        if (oocUI.activeSelf == true) {
            oocUI.SetActive(false);
        }
        else {
            oocUI.SetActive(true);
        }
    }
    public void DisplayNewDayUI() {
        if (newDayUI.activeSelf == true) {
            newDayUI.SetActive(false);
        }
        else {
            newDayUI.SetActive(true);
        }
    }
    #endregion
    public void ChooseDesert() {
        GameManager.Instance.SetExcavationIndex(1);
        DisplayConfirmation();
    }
    public void ChooseMountains() {
        GameManager.Instance.SetExcavationIndex(2);
        DisplayConfirmation();
    }
    public void ChooseForest() {
        GameManager.Instance.SetExcavationIndex(3);
        DisplayConfirmation();
    }
    public void ConfirmLoad() {      
        if(stamina != 0 && museStats.GetWealth() >= 250) {
            GameManager.Instance.LoadLevel("DigSite");
            GameManager.Instance.UnloadLevel("Office");
            UpdateStats();
        }else if(stamina == 0){
            DisplayOOSUI();
        }else if(museStats.GetWealth() < 250) {
            DisplayOOCUI();
        }
        
    }
    public void StartNewDay() {
        DisplayNewDayUI();
        MuseumStats.instance.SetStamina(MuseumStats.instance.museumDefinition.playerMaxStamina);
        MuseumStats.instance.IncrementDay();
        museStats.ApplyWealth(GetDonations());
        museStats.ApplyWealth(GetAdmissions());
        newDayText.GetComponent<TextMeshProUGUI>().SetText("There were a total of " + visitorTotal + " visitors earning you £" + admissionTotal + " in admissions. £" + donationTotal + " was left in donations");
        GameManager.Instance.Save();
    }
    public void LoadNewDay() {
        GameManager.Instance.UnloadLevel("Office");
        GameManager.Instance.StartGame();
    }
    private int GetAdmissions() {
        admissionTotal = 0;
        admissionTotal = visitorTotal * admissionFee;
        return admissionTotal;
    }
    private int GetDonations() {
        donationTotal = 0;
        int randomNum = Random.Range(1, 3);
        foreach(InventoryEntry ie in exhibitsOnDisplayList) {
            int donation = Mathf.RoundToInt((ie.invEntry.itemDefinition.exhibitValueAmount * 0.04f) * CalculateVisitorRatingRatio(ie.invEntry.itemDefinition.exhibitRatingAmount));
            if(randomNum != 1) {
                donationTotal += donation;
            }            
        }
        return donationTotal;
    }
    private void UpdateStats() {
        MuseumStats.instance.ApplyWealth(-250);
        MuseumStats.instance.ApplyStamina(-1);
    }
    private int GetExhibitsOnDisplay() {
        exhibitsOnDisplay = 0;
        exhibitsOnDisplayList.Clear();
        foreach(InventoryEntry ie in museumInventory.exhibitsInInventory) {
            if(ie.invEntry.itemDefinition.isDisplayed == true) {
                exhibitsOnDisplay += 1;
                exhibitsOnDisplayList.Add(ie);
            }
        }
        return exhibitsOnDisplay;
    }
    private int CalculateVisitorRatingRatio(float exhibitRating) {
        int randomNum = Random.Range(1, 20);
        int numOfVisitors = 0;
        if(randomNum == 1) {
            numOfVisitors = 0;
        }
        else {
            if (exhibitRating <= 1) {
                numOfVisitors = Random.Range(1, 3);
            }
            else if (exhibitRating > 1 && exhibitRating <= 3) {
                numOfVisitors = Random.Range(2, 5);
            }
            else {
                numOfVisitors = Random.Range(5, 8);
            }
        }
        visitorTotal += numOfVisitors;
        return numOfVisitors;
    }
    void OnTriggerEnter2D(Collider2D collider) {
        if(collider.tag == "Player") {
            inRange = true;
        }
    }
    void OnTriggerExit2D(Collider2D collider) {
        if(collider.tag == "Player") {
            inRange = false;
            officeUI.SetActive(false);
        }
    }
}
