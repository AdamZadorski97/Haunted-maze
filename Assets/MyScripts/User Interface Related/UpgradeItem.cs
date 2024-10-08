using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class UpgradeItem : MonoBehaviour
{
    public TMP_Text statValue;
    public TMP_Text upgradeCost;
    public TMP_Text currentLevel;
    public Image upgradeButton;
    public Button upgradeButtonComponent;  // Reference to the button component

    // Function to set the upgrade values (UI display)
    public void SetUpgradeValues(string statValueText, string upgradeCostText, string currentLevelText)
    {
        statValue.text = statValueText;
        upgradeCost.text = upgradeCostText;
        currentLevel.text = currentLevelText;
    }

    // Function to set the upgrade button click action
    public void SetUpgradeAction(UnityAction action)
    {
        upgradeButtonComponent.onClick.RemoveAllListeners();  // Clear previous listeners
        upgradeButtonComponent.onClick.AddListener(action);   // Add the new action
    }
}
