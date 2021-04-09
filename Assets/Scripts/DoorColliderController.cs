﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorColliderController : MonoBehaviour
{
    public string nextGameScene, currentGameScene;
    bool inDoor;

    void Update() {
        if(inDoor == true && Input.GetKeyDown(KeyCode.F)) {
            GameManager.Instance.LoadLevel(nextGameScene);
            Debug.Log("[DoorColliderController] LoadLevel attempted");
            GameManager.Instance.UnloadLevel(currentGameScene);
        }
    }
    void OnTriggerEnter2D(Collider2D triggerCollider) {
        if(triggerCollider.tag == "Player") {
            inDoor = true;
            Debug.Log("[DoorColliderController] Player Detected");
        }
    }
    void OnTriggerExit2D(Collider2D triggerCollider) {
        if (triggerCollider.tag == "Player") {
            inDoor = false;
            Debug.Log("[DoorColliderController] Player Left");
        }
    }

}
