using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorColliderController : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D triggerCollider) {
        if(triggerCollider.tag == "Player") {
            Debug.Log("[DoorColliderController] Player Detected");
        }
    }
    void OnTriggerExit2D(Collider2D triggerCollider) {
        if (triggerCollider.tag == "Player") {
            Debug.Log("[DoorColliderController] Player Left");
        }
    }

}
