using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MuseumStatsUI : MonoBehaviour
{
    public MuseumStats museStats;
    public MuseumStatsUI museStatsUI;
    public GameObject ratingText, wealthText, dayText;
    void Start() {
        museStats = MuseumStats.instance;
        museStatsUI = this;
    }
    void Update() {
        wealthText.GetComponent<TextMeshProUGUI>().SetText("Currency: " + museStats.GetWealth());
        ratingText.GetComponent<TextMeshProUGUI>().SetText("Rating: " + museStats.GetRoundedRating());
        dayText.GetComponent<TextMeshProUGUI>().SetText("Day: " + museStats.GetDay());
    }
}
