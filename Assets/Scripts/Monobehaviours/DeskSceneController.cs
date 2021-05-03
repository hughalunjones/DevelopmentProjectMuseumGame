using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeskSceneController : MonoBehaviour {
    bool inRange;
    public GameObject officeUI, confirmationUI, ratingText, wealthText;
    public MuseumStats museStats;

    void Start() {
        museStats = MuseumStats.instance;
    }
    void Update() {
        wealthText.GetComponent<TextMeshProUGUI>().SetText(museStats.GetWealth().ToString());
        ratingText.GetComponent<TextMeshProUGUI>().SetText(museStats.GetRoundedRating().ToString());
        if (inRange && Input.GetKeyDown(KeyCode.E)) {
            DisplayOfficeUI();
        }
    }
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
    public void ChooseDesert() {
        GameManager.Instance.SetExcavationIndex(1);
        Debug.Log("[DeskSceneController] " + GameManager.Instance.GetExcavationSceneIndex());
        DisplayConfirmation();
    }
    public void ChooseMountains() {
        GameManager.Instance.SetExcavationIndex(2);
        Debug.Log("[DeskSceneController] " + GameManager.Instance.GetExcavationSceneIndex());
        DisplayConfirmation();
    }
    public void ChooseForest() {
        GameManager.Instance.SetExcavationIndex(3);
        Debug.Log("[DeskSceneController] " + GameManager.Instance.GetExcavationSceneIndex());
        DisplayConfirmation();
    }
    public void Cancel() {
        DisplayConfirmation();
    }
    public void ConfirmLoad() {        
        GameManager.Instance.LoadLevel("DigSite");
        GameManager.Instance.UnloadLevel("Office");
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
