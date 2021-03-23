
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public Sprite playerSprite;
    public Animator animator;
    private bool facingRight = true;

    void Start() {
        playerSprite = GetComponent<Sprite>();
    }

    // Assign a Sprite component in the inspector to instantiate
    void Update() {
        float inputX = Input.GetAxisRaw("Horizontal");        
        float velocity = inputX * speed;
        animator.SetFloat("Speed", Mathf.Abs(velocity));        
        transform.Translate(Vector2.right * velocity * Time.deltaTime);
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
    }

    void Flip() {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
