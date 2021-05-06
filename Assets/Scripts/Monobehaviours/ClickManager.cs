using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClickManager : MonoBehaviour
{
    GameObject digSpot;
    Exhibit artefact;
    RaycastHit2D hit;
    int toolStrength = 3;
    int digStamina = 100;
    public Canvas canvas;
    public GameObject artefactDisplay, artefactName, artefactDescription, btnStoreArtefact;
    public enum SelectedTool {
        HAMMER,
        PICKAXE,
        TROWEL,
        NONE
    }
    SelectedTool currentTool = SelectedTool.NONE;
    public SelectedTool CurrentTool {
        get { return currentTool; }
        private set { currentTool = value; }
    }

    // Returns the player to the office scene
    public void ReturnToMuseum() {
        GameManager.Instance.LoadLevel("Office");
        GameManager.Instance.UnloadLevel("DigSite");
    }

    // In this update method there is a constant check for what the mouse is clicking on, is it a tile or an artefact
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos3D = new Vector3(mousePos.x, mousePos.y, mousePos.z);
            hit = Physics2D.Raycast(mousePos3D, Vector3.zero);
            if (hit.collider != null) {
                string tileTag = hit.collider.tag;
                if(tileTag == "soil" || tileTag == "rock" || tileTag == "hardsoil") {
                    if(currentTool == SelectedTool.HAMMER && (tileTag == "soil" || tileTag == "hardsoil")) {
                        toolStrength = 0;
                    }
                    else if(tileTag == "rock" && currentTool == SelectedTool.TROWEL) {
                        toolStrength = 0;
                    }
                    else {
                        UpdateToolSelection(currentTool);
                    }
                    digSpot = hit.collider.gameObject;
                    if (digSpot.GetComponent<DigSpotManager>().isDiggable && digStamina != 0) {
                        digSpot.GetComponent<DigSpotManager>().currentHealth -= toolStrength;
                        digStamina -= 1;
                        Debug.Log(digSpot.GetComponent<DigSpotManager>().currentHealth);
                    }else {
                        if(digStamina == 0) {
                            Debug.Log("[ClickManager] Out of Stamina");
                        }
                        else {
                            Debug.Log("[ClickManager] Cannot dig this layer");
                        }
                        
                    }
                }
                if(hit.collider.tag == "artefact") {
                    artefact = hit.collider.gameObject.GetComponent<Exhibit>();
                    ShowArtefact(artefact);
                }                
            }
        }
    }

    // The three methods below are assigned to the buttons in the UI
    public void SelectHammer() {
        UpdateToolSelection(currentTool = SelectedTool.HAMMER);
    }
    public void SelectPickAxe() {
        UpdateToolSelection(currentTool = SelectedTool.PICKAXE);
    }
    public void SelectTrowel() {
        UpdateToolSelection(currentTool = SelectedTool.TROWEL);
    }

    // Selects the ammount of damage a click does to a tile
    void UpdateToolSelection(SelectedTool tool) {
        currentTool = tool;
        switch (currentTool) {
            case SelectedTool.HAMMER:
            toolStrength = 4;
            break;
            case SelectedTool.PICKAXE:
            toolStrength = 3;
            break;
            case SelectedTool.TROWEL:
            toolStrength = 2;
            break;
            case SelectedTool.NONE:
            toolStrength = 0;
            break;
            default:
            break;
        }
    }

    // This method displays the "dug up" artefact to the player on the UI overlay
    void ShowArtefact(Exhibit artefact) {
        if(artefact.GetComponent<SpriteRenderer>() != null) {
            canvas.gameObject.SetActive(true);
            btnStoreArtefact.GetComponent<Button>().onClick.AddListener(delegate () { StoreArtefact(artefact); });
            artefactDisplay.GetComponent<Image>().preserveAspect = true;
            artefactDisplay.GetComponent<Image>().sprite = artefact.GetComponent<SpriteRenderer>().sprite;
            artefactName.GetComponent<TextMeshProUGUI>().text = artefact.itemDefinition.exhibitName;
            UpdateToolSelection(currentTool = SelectedTool.NONE);
        }
        else {
            Debug.LogError("[ClickManager] SpriteRenderer is null");
        }
    }

    // Store the exhibit in the inventory
    void StoreArtefact(Exhibit artefact) {
        artefact.StoreItem();
        canvas.gameObject.SetActive(false);
    }
}
    