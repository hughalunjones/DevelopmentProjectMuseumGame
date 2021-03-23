using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour{
    public Transform player;
    private Vector3 cameraTransform;
    public BoxCollider2D mapBounds;
    private float xMin, xMax, camOrthoSize, halfCamView;
    public Camera mainCamera;

    private void Start(){
        xMin = mapBounds.bounds.min.x;
        xMax = mapBounds.bounds.max.x;
        camOrthoSize = mainCamera.orthographicSize;
        halfCamView = (xMax + camOrthoSize) / 2.0f;
    }

    // LateUpdate is called after Update each frame
    void LateUpdate(){
        if (player != null) {
            transform.position = new Vector3(Mathf.Clamp(player.position.x, (xMin + halfCamView), (xMax - halfCamView)),
                                             transform.position.y,
                                             transform.position.z);
        }
    }


}
