using skner.DualGrid.Extensions;
using UnityEditor;
using UnityEngine;

namespace skner.DualGrid.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Transform))]
    public class RestrictedTransformEditor : UnityEditor.Editor
    {
        private UnityEditor.Editor _defaultEditor;

        private void OnEnable()
        {
            _defaultEditor = CreateEditor(targets, typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.TransformInspector"));
        }

        public override void OnInspectorGUI()
        {
            Transform transform = (Transform)target;

            // Check if this transform is from the RenderTilemap of a DualGridTilemapModule
            DualGridTilemapModule module = transform.GetComponentInImmediateParent<DualGridTilemapModule>();
            if (module != null)
            {
                EditorGUILayout.HelpBox($"Editing is disabled on a RenderTilemap. The transform is managed by the {nameof(DualGridTilemapModule)}.", MessageType.Info);
                GUI.enabled = false;
                transform.position = -1 * module.DataTilemap.tileAnchor;
                _defaultEditor.OnInspectorGUI();
                GUI.enabled = true;
            }
            else
            {
                _defaultEditor.OnInspectorGUI();
            }
        }

        private void OnDisable()
        {
            if (_defaultEditor != null)
            {
                DestroyImmediate(_defaultEditor);
            }
        }
    }
}