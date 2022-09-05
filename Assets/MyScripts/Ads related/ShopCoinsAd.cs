using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCoinsAd : MonoBehaviour
{
    public InterstitialAd interstitial;
    public MainMenuController mainMenuController;
    public ShopController shopController;

    
   
    
    
    
    public void OnAdsButton()
    {
        shopController.GetPricesList();
        interstitial.ShowAd(OnRewarded);
    }
    public void OnRewarded()
    {
        mainMenuController.saveLoadDataManager.AddCoins(shopController.pricesList[0]);
        shopController.UpdateShopItemValues();
    }
}
