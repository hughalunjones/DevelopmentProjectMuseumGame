using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpMenu : MonoBehaviour {

    public GameObject helpMenu;

    public void DisplayHelpMenu() {
        if (helpMenu.activeSelf == true) {
            helpMenu.SetActive(false);
        }
        else {
            helpMenu.SetActive(true);
        }
    }
}
