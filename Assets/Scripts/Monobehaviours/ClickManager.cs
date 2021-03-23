using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickManager : MonoBehaviour
{
    GameObject digSpot;
    GameObject artefact;
    MuseumInventory museumInventory;
    RaycastHit2D hit;
    int toolStrength = 3;
    int digStamina = 100;
    public Canvas canvas;
    public GameObject artefactDisplay;
    public enum SelectedTool {
        HAMMER,
        PICKAXE,
        TROWEL
    }
    SelectedTool currentTool = SelectedTool.TROWEL;
    public SelectedTool CurrentTool {
        get { return currentTool; }
        private set { currentTool = value; }
    }
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos3D = new Vector3(mousePos.x, mousePos.y, mousePos.z);
            Debug.Log("[ClickManager] Mouse Clicked at: " + mousePos3D);
            hit = Physics2D.Raycast(mousePos3D, Vector3.zero);
            if (hit.collider != null) {
                Debug.Log("[ClickManager] Object clicked: " + hit.collider);
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
                    artefact = hit.collider.gameObject;
                    // Show pick up screen, fade bg and scale artefact up.
                    ShowArtefact(artefact);
                    // Store the artefact in the inventory.
                    Debug.Log("[ClickManager] Artefact Stored.");
                }                
            }
        }
    }
    public void SelectHammer() {
        UpdateToolSelection(currentTool = SelectedTool.HAMMER);
        Debug.Log("[ClickManager] Current tool:" + currentTool + " - " + toolStrength);
    }
    public void SelectPickAxe() {
        UpdateToolSelection(currentTool = SelectedTool.PICKAXE);
        Debug.Log("[ClickManager] Current tool:" + currentTool + " - " + toolStrength);
    }
    public void SelectTrowel() {
        UpdateToolSelection(currentTool = SelectedTool.TROWEL);
        Debug.Log("[ClickManager] Current tool:" + currentTool + " - " + toolStrength);
    }
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
            default:
            break;
        }
    }
    void ShowArtefact(GameObject artefact) {
        if(artefact.GetComponent<SpriteRenderer>() != null) {
            canvas.gameObject.SetActive(true);
            artefactDisplay.GetComponent<Image>().sprite = artefact.GetComponent<SpriteRenderer>().sprite;
            Destroy(artefact);
        }
        else {
            Debug.LogError("[ClickManager] SpriteRenderer is null");
        }

    }
}
    