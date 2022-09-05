using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.VFX;
public class DotweenExample : MonoBehaviour
{
    [SerializeField] private Light pointLight;

    [SerializeField] private float maxGlow = 10;
    [SerializeField] private float minGlow = 0;
    [SerializeField] private float time = 1;
  
    private Sequence blinkSequence;

    [SerializeField] private AnimationCurve fadeCurve;

    private void Start()
    {
        blinkSequence = DOTween.Sequence();
        blinkSequence.Append( pointLight.DOIntensity(maxGlow, time).SetEase(fadeCurve));
        blinkSequence.Join(pointLight.DOColor(Color.red, time));
        blinkSequence.AppendInterval(1);
        blinkSequence.Append(pointLight.DOIntensity(minGlow, time));
        blinkSequence.Join(pointLight.DOColor(Color.yellow, time));
        blinkSequence.AppendInterval(1);
        blinkSequence.SetLoops(-1);
    }
}
