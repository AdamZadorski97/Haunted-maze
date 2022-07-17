using UnityEngine;

namespace Lovatto.MiniMap
{
    public class bl_MiniMapPlane : MonoBehaviour
    {
        public GameObject mapPlane;
        public GameObject gridPlane;
        public Material planeMaterial, planeMobileMaterial;

        private Transform m_Transform;
        private bl_MiniMap m_Minimap;
        private Vector3 currentPosition;
        private Vector3 worldPosition;
        private float defaultYCameraPosition;

        /// <summary>
        /// 
        /// </summary>
        public void Setup(bl_MiniMap minimap)
        {
            m_Transform = transform;
            m_Minimap = minimap;
            //Get Position reference from world space rect.
            Vector3 pos = minimap.WorldSpace.position;
            //Get Size reference from world space rect.
            Vector3 size = minimap.WorldSpace.sizeDelta;
            if (minimap.renderType == bl_MiniMap.RenderType.Picture)
            {
                //Apply material with map texture.
                PlaneRender.material = CreateMaterial();
            }
            //Set position
            m_Transform.localPosition = pos;
            //Set Correct size.
            m_Transform.localScale = (new Vector3(size.x, 10, size.y) / 10);

            m_Transform.rotation = minimap.WorldSpace.rotation;
            var eulers = m_Transform.eulerAngles;
            eulers.x -= 90;
            m_Transform.eulerAngles = eulers;

            worldPosition = minimap.WorldSpace.position;
            //Apply MiniMap Layer
            mapPlane.layer = minimap.MiniMapLayer;
            gridPlane.layer = minimap.MiniMapLayer;
            gameObject.hideFlags = HideFlags.HideInHierarchy;
            gameObject.name = $"Minimap Plane ({minimap.gameObject.name})";
            mapPlane.SetActive(minimap.renderType == bl_MiniMap.RenderType.Picture);
            gridPlane.SetActive(minimap.ShowAreaGrid);
            if (minimap.ShowAreaGrid)
            {
                var mat = gridPlane.GetComponent<Renderer>().material;
                mat.SetTextureScale("_MainTex", Vector2.one * minimap.AreasSize);
                mat.SetColor("_Color", new Color(1, 1, 1, minimap.gridOpacity));
            }
            Invoke(nameof(DelayPositionInvoke), 1);
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnUpdate()
        {
            currentPosition = m_Transform.localPosition;
            //Get Position reference from world space rect.
            float ydif = defaultYCameraPosition - m_Minimap.miniMapCamera.transform.position.y;
            currentPosition.y = currentPosition.y - ydif;
            m_Transform.position = currentPosition;
        }

        void DelayPositionInvoke() { defaultYCameraPosition = m_Minimap.miniMapCamera.transform.position.y; }

        /// <summary>
        /// 
        /// </summary>
        public void SetMapTexture(Texture2D newTexture)
        {
            PlaneRender.material.mainTexture = newTexture;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        public void SetGridSize(float size)
        {
            gridPlane.GetComponent<Renderer>().material.SetTextureScale("_MainTex", Vector2.one * size);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="active"></param>
        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        /// <summary>
        /// Create Material for Minimap image in plane.
        /// you can edit and add your own shader.
        /// </summary>
        /// <returns></returns>
        public Material CreateMaterial()
        {
            Material mat = new Material(m_Minimap.isMobile ? planeMobileMaterial : planeMaterial);

            mat.mainTexture = m_Minimap.MapTexture;
            mat.SetFloat("_Power", m_Minimap.planeSaturation);
            return mat;
        }

        public Renderer PlaneRender
        {
            get { return mapPlane.GetComponent<Renderer>(); }
        }
    }
}