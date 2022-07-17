using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Lovatto.MiniMap
{
    public class bl_MiniMapUI : MonoBehaviour
    {
        public RectTransform root;
        public CanvasGroup rootAlpha;
        public Image playerIconImage;
        public RectTransform iconsPanel;
        public bl_MiniMapTexture mapTextureRender = null;
        public bl_MaskHelper minimapMaskManager;
        public Animator hitAnimator;

        public float hitEffectSpeed = 1.5f;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="miniMap"></param>
        public void Setup(bl_MiniMap miniMap)
        {
            if(playerIconImage != null)
            {
                playerIconImage.sprite = miniMap.PlayerIconSprite;
                playerIconImage.color = miniMap.playerColor;
                playerIconImage.gameObject.SetActive(miniMap.mapMode == bl_MiniMap.MapType.Local);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="active"></param>
        public void SetActive(bool active)
        {
            root.gameObject.SetActive(active);
        }

        /// <summary>
        /// 
        /// </summary>
        public void DoStartFade(float delay, Action callback)
        {
            if (rootAlpha == null) return;

            StopAllCoroutines();
            StartCoroutine(Fade());

            IEnumerator Fade()
            {
                rootAlpha.alpha = 0;
                yield return new WaitForSeconds(delay);
                while (rootAlpha.alpha < 1)
                {
                    rootAlpha.alpha += Time.deltaTime;
                    yield return null;
                }
                callback?.Invoke();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetIcon"></param>
        public void ConfigureWorldTarget(bl_MiniMapEntityBase targetIcon)
        {
            if (playerIconImage == null)
                return;

            targetIcon.SetIconSettings(new MiniMapIconSettings()
            {
                Icon = playerIconImage.sprite,
                Color = playerIconImage.color, 
                Target = targetIcon.transform,
                Size = playerIconImage.rectTransform.sizeDelta.x + 2,
                ItemEffect = ItemEffect.None,
                Interactable = false
            });
        }

        /// <summary>
        /// 
        /// </summary>
        public void DoHitEffect()
        {
            if (hitAnimator == null) return;

            hitAnimator.speed = hitEffectSpeed;
            hitAnimator.Play("HitEffect", 0, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnZoomClick(bool zoomIn)
        {
            Minimap.ChangeZoom(zoomIn);
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnCentralizeClick()
        {
            Minimap.GoToTarget();
        }

        private bl_MiniMap _minimap;
        public bl_MiniMap Minimap
        {
            get
            {
                if(_minimap == null)
                {
                    _minimap = GetComponentInParent<bl_MiniMap>();
                }
                return _minimap;
            }
        }

        private RectTransform _playerIconTransform = null;
        public RectTransform PlayerIconTransform
        {
            get
            {
                if (_playerIconTransform == null && playerIconImage != null) _playerIconTransform = (RectTransform)playerIconImage.transform;
                return _playerIconTransform;
            }
        }
    }
}