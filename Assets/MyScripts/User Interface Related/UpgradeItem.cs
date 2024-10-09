using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class UpgradeItem : MonoBehaviour
{
    public TMP_Text statValue;
    public TMP_Text upgradeCost;
    public TMP_Text currentLevel;
    public Image upgradeButtonImage; // Change to reference the image of the button
    public Button upgradeButtonComponent;  // Reference to the button component

    [SerializeField] private Sprite canUpdateSprite;
    [SerializeField] private Sprite cantUpdateSprite;

    private bool canUpgrade;  // Stores whether the upgrade is possible

    // Function to set the upgrade values (UI display)
    public void SetUpgradeValues(string statValueText, string upgradeCostText, string currentLevelText, bool canUpgrade)
    {
        statValue.text = statValueText;
        upgradeCost.text = upgradeCostText;
        currentLevel.text = currentLevelText;

        this.canUpgrade = canUpgrade;  // Set whether the upgrade is possible

        // Update the button sprite based on whether the player can upgrade
        upgradeButtonImage.sprite = canUpgrade ? canUpdateSprite : cantUpdateSprite;
    }

    // Function to set the upgrade button click action
    public void SetUpgradeAction(UnityAction action)
    {
        upgradeButtonComponent.onClick.RemoveAllListeners();  // Clear previous listeners

        // Only add the action if the upgrade is possible
        if (canUpgrade)
        {
            upgradeButtonComponent.onClick.AddListener(action);   // Add the new action
        }
    }
}
