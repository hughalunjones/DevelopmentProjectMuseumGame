using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour{
    public Transform player;
    public BoxCollider2D mapBounds;
    private Vector3 minBounds, maxBounds;
    private float halfCamViewWidth, halfCamViewHeight;
    public Camera mainCamera;

    private void Start(){
        minBounds = mapBounds.bounds.min;
        maxBounds = mapBounds.bounds.max;
        mainCamera = GetComponent<Camera>();
        halfCamViewHeight = mainCamera.orthographicSize;
        halfCamViewWidth = halfCamViewHeight * Screen.width / Screen.height;
    }
    void Update() {
        transform.position = new Vector3(player.position.x, player.position.y, player.position.z - 10);
        float clampedX = Mathf.Clamp(transform.position.x, minBounds.x + halfCamViewWidth, maxBounds.x - halfCamViewWidth);
        float clampedY = Mathf.Clamp(transform.position.y, minBounds.y + halfCamViewHeight, maxBounds.y - halfCamViewHeight);
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}
