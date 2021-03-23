using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigSpotManager : MonoBehaviour
{
    public int startHealth = 12;
    public int currentHealth;
    public bool isDiggable = true;
    void Start() {
        currentHealth = startHealth;
    }
    void Update(){
        if(currentHealth <= 0) {
            Destroy(gameObject);
        }
    }
    public int getDigSpotHealth() {
        return currentHealth;
    }
}
