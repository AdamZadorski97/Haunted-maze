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
    [SerializeField] private MeshRenderer glow;
    [SerializeField] private TMP_Text textFrontMultipler;
    [SerializeField] private TMP_Text textBackMultipler;
    [SerializeField] private BoxCollider boxCollider;
    private Vector3 tempScale;
    private Vector3 tempPosition;
    public Color glowAlpha;

    private void Start()
    {
        tempScale = transform.localScale;
        tempPosition = transform.position;
        glowAlpha = glow.material.GetColor("_TintColor");
    }
    public void OnInteractivePointPickup()
    {
        boxCollider.enabled = false;
        Sequence pointSequence = DOTween.Sequence();
        pointSequence.Append( transform.DOScale(Vector3.zero, 0.45f));
        pointSequence.Join(transform.DOMove(PlayerController.Instance.transform.position, 0.45f));
        pointSequence.Join(glow.material.DOColor(Vector4.zero, "_TintColor", 0.45f));


        pointSequence.AppendCallback(()=>
        {
            transform.gameObject.SetActive(false);
            transform.localScale = tempScale;
            transform.position = tempPosition;
            glow.material.SetColor("_TintColor", glowAlpha);
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
