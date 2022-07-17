using UnityEngine;
using UnityEngine.UI;

namespace Lovatto.MiniMap
{
    public abstract class bl_MiniMapIconBase : MonoBehaviour
    {
        public abstract Image GetImage
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public abstract void SetUp(bl_MiniMapEntityBase entity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newIcon"></param>
        public abstract void SetIcon(Sprite newIcon);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        public abstract void SetText(string text);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newColor"></param>
        public abstract void SetColor(Color newColor);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="opacity"></param>
        public abstract void SetOpacity(float opacity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="AreaColor"></param>
        /// <returns></returns>
        public abstract RectTransform SetCircleArea(float radius, Color AreaColor);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="active"></param>
        public abstract void SetActive(bool active);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="active"></param>
        public abstract void SetActiveCircleArea(bool active);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inmediate"></param>
        public abstract void DestroyIcon(bool inmediate, Sprite overrideDeathIcon = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="delay"></param>
        public abstract void SpawnedDelayed(float delay);

        /// <summary>
        /// 
        /// </summary>
        public abstract void ForceFaceUp();
    }
}