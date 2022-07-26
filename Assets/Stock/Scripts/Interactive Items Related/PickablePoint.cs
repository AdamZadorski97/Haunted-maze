using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
public class PickablePoint : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshBorder;
    [SerializeField] private MeshRenderer meshBorderHD;
    [SerializeField] private MeshRenderer meshPlate;
    [SerializeField] private TMP_Text textFrontMultipler;
    [SerializeField] private TMP_Text textBackMultipler;
    [SerializeField] private BoxCollider boxCollider;
    private Vector3 tempScale;

    private void Start()
    {
        tempScale = transform.localScale;
    }
    public void OnInteractivePointPickup()
    {
        boxCollider.enabled = false;
        Sequence pointSequence = DOTween.Sequence();
        pointSequence.Append( transform.DOScale(Vector3.zero, 0.2f));

        pointSequence.AppendCallback(()=>
        {
            transform.gameObject.SetActive(false);
            transform.localScale = tempScale;
        });
    }

    public void SetMultipler(string value, Color borderColor, Color plateColor, Color textColor)
    {
        boxCollider.enabled = true;
        textFrontMultipler.text = value;
        textBackMultipler.text = value;
        textBackMultipler.color = textColor;
        textFrontMultipler.color = textColor;
        meshBorder.material.SetColor("_Color", borderColor);
        meshBorderHD.material.SetColor("_Color", borderColor);
        meshPlate.material.SetColor("_Color", plateColor);
    }
}
