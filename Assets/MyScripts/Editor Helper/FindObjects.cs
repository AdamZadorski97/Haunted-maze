using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindObjects : MonoBehaviour
{

    void Start()
    {
       
            Debug.Log(GameObject.FindGameObjectsWithTag("Point").Length);
        
    }


}
