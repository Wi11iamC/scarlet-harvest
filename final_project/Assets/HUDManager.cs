using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class HUDManager : MonoBehaviour
{
    public int tomatoes = 10;
    public int pigs = 10;
    private TextMeshProUGUI hudText; // changed from Text to TextMeshProUGUI

    void Start()
    {
        hudText = GetComponent<TextMeshProUGUI>();
        UpdateHUD();
    }

    void UpdateHUD()
    {
        hudText.text = "Tomatoes: " + tomatoes + " | " + " Pig Population: " + pigs;
    }

    public void AddTomatoes(int amount)
    {
        tomatoes += amount;
        UpdateHUD();
    }

    public void AddPigs(int amount)
    {
        pigs += amount;
        UpdateHUD();
    }

    public void SubtractTomatoes(int amount)
    {
        tomatoes -= amount;
        UpdateHUD();
    }

        public void SubtractPigs(int amount)
    {
        pigs -= amount;
        UpdateHUD();
    }
    public int getTomatoes()
    {
        return tomatoes;
    }
    public int getPigs()
    {
        return pigs;
    }
}
