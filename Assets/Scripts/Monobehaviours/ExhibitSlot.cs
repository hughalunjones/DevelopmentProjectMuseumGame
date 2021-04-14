using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExhibitSlot : MonoBehaviour
{
    MuseumInventory museInventory;
    bool inRangeOfSlot;
    void Start() {
        museInventory = MuseumInventory.instance;
    }
    void Update() {
        if (inRangeOfSlot && Input.GetKeyDown(KeyCode.E)) {
            museInventory.DisplayInventory();
            GameManager.Instance.TogglePause();
            UIManager.Instance.GetComponentInChildren<PauseMenu>().gameObject.SetActive(false);
        }
    }
    void OnTriggerEnter2D(Collider2D triggerCollider) {
        if (triggerCollider.tag == "Player") {
            inRangeOfSlot = true;
            Debug.Log("[Exhibit] Player Detected " + inRangeOfSlot);
        }
    }
    void OnTriggerExit2D(Collider2D triggerCollider) {
        if (triggerCollider.tag == "Player") {
            inRangeOfSlot = false;
            Debug.Log("[Exhibit] Player Left " + inRangeOfSlot);
        }
    }
}
