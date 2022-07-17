using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Lovatto.MiniMap
{
    public class bl_MiniMapBounds : MonoBehaviour
    {

        [Header("Use UI editor Tool for scale the wordSpace")]
        public Color GizmoColor = new Color(1, 1, 1, 0.75f);
        public bool alwaysShow = false;

        private RectTransform m_rectTransform;


        /// <summary>
        /// 
        /// </summary>
        private void OnDrawGizmos()
        {
            if (!alwaysShow) return;

            Draw();
        }

        /// <summary>
        /// Debuging world space of map
        /// </summary>
        void OnDrawGizmosSelected()
        {
            if (alwaysShow) return;

            Draw();
        }

        /// <summary>
        /// 
        /// </summary>
        void Draw()
        {
            if (m_rectTransform == null) m_rectTransform = this.GetComponent<RectTransform>();

            Vector3 v = m_rectTransform.sizeDelta;

            var matrix = Gizmos.matrix;

            Gizmos.matrix = Matrix4x4.TRS(m_rectTransform.position, m_rectTransform.rotation, Vector3.one);

            Gizmos.color = GizmoColor;
            Gizmos.DrawCube(Vector3.zero, new Vector3(v.x, v.y, 2));
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(v.x, v.y, 2));

            Gizmos.matrix = matrix;
        }
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(bl_MiniMapBounds))]
    public class bl_MiniMapBoundsEditor : Editor
    {
        private Tool beforeTool = Tool.Move;

        private void OnEnable()
        {
            beforeTool = Tools.current;
        }

        private void OnDisable()
        {
            Tools.current = beforeTool;
            Tools.current = Tool.Rect;
        }

        void OnSceneGUI()
        {
            // get the chosen game object
            bl_MiniMapBounds t = target as bl_MiniMapBounds;
            if (t == null)
                return;

            Tools.current = Tool.Rect;
            t.transform.position = Handles.DoPositionHandle(t.transform.position, t.transform.rotation);
        }
    }
#endif
}