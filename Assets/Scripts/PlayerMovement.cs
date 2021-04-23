
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public Sprite playerSprite;
    public Animator animator;
    public BoxCollider2D mapBounds;
    private bool facingRight = true;
    private float xMin, xMax, yMin, yMax, xScaleHalf, yScaleHalf;

    void Start() {
        playerSprite = GetComponent<Sprite>();
        xMin = mapBounds.bounds.min.x;
        xMax = mapBounds.bounds.max.x;
        yMin = mapBounds.bounds.min.y;
        yMax = mapBounds.bounds.max.y;
        xScaleHalf = transform.localScale.x / 8;
        yScaleHalf = transform.localScale.y / 3;
    }

    // Assign a Sprite component in the inspector to instantiate
    void Update() {
        float inputX = Input.GetAxisRaw("Horizontal");
        float velocity = inputX * speed;
        animator.SetFloat("Speed", Mathf.Abs(velocity));
        transform.Translate(Vector2.right * velocity * Time.deltaTime);
        if (GameManager.Instance.GetCurrentLevelName() == "MainHall") {
            float inputY = Input.GetAxisRaw("Vertical");
            float vertVel = inputY * speed;
            transform.Translate(Vector2.up * vertVel * Time.deltaTime);
        }
        if(inputX > 0 && !facingRight) {
            Flip();
        }else if(inputX < 0 && facingRight) {
            Flip();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            speed = 5f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift)) {
            speed = 2f;
        }

        if(transform.position.x >= xMax - xScaleHalf) {
            transform.position = new Vector2(xMax - xScaleHalf, transform.position.y);
        }else if(transform.position.x <= xMin + xScaleHalf) {
            transform.position = new Vector2(xMin + xScaleHalf, transform.position.y);
        }
        if (transform.position.y <= yMin + yScaleHalf) {
            transform.position = new Vector2(transform.position.x, yMin + yScaleHalf);
        }else if (transform.position.y >= yMax - yScaleHalf) {
            transform.position = new Vector2(transform.position.x, yMax - yScaleHalf);
        }

    }

    void Flip() {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
