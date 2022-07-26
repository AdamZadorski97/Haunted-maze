using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CoinOculusion : MonoBehaviour
{
    public Collider[] currentCoins;
    Collider[] coinsInsideZone;
    Collider[] coinsOutSideZone;

    private void FixedUpdate()
    {
        coinsInsideZone = Physics.OverlapSphere(this.transform.position, 10);

        foreach(var coin in coinsInsideZone)
        {
            coin.transform.GetChild(0).gameObject.SetActive(true);
        }
        coinsOutSideZone = currentCoins.Except(coinsInsideZone).ToArray();
        foreach(var coin in coinsOutSideZone)
        {
            coin.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
