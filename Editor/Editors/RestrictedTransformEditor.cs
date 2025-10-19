using skner.DualGrid.Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace skner.DualGrid.Editor
{
    [CanEditMultipleObjects] [CustomEditor(typeof(Transform))]
    public class RestrictedTransformEditor : UnityEditor.Editor
    {
        private UnityEditor.Editor _defaultEditor;

        private void OnEnable()
        {
            _defaultEditor = CreateEditor(targets,
                typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.TransformInspector"));
        }

        public override void OnInspectorGUI()
        {
            Transform transform = (Transform)target;

            // Check if this transform is from the RenderTilemap of a DualGridTilemapModule
            DualGridTilemapModule module =  transform.GetComponentInImmediateParent<DualGridTilemapModule>();
            if (module != null)
            {
                //RenderTilemap
                EditorGUILayout.HelpBox(
                    $"Editing is disabled on a RenderTilemap. The transform is managed by the {nameof(DualGridTilemapModule)}.",
                    MessageType.Info);
                GUI.enabled = false;
                UpdatePosition(transform, module.DataTilemap.orientation);
                _defaultEditor.OnInspectorGUI();
                GUI.enabled = true;
                return;
            }

            module = transform.GetComponent<DualGridTilemapModule>();
            if (module != null)
            {
                //DataTileMap
                UpdatePosition(transform.GetChild(0), module.DataTilemap.orientation);
                _defaultEditor.OnInspectorGUI();
            }
            else
            {
                _defaultEditor.OnInspectorGUI();
            }
        }

        void UpdatePosition(
            Transform t,
            Tilemap.Orientation orientation)
        {
            switch (orientation)
            {
                case Tilemap.Orientation.YX:
                case Tilemap.Orientation.XY:
                    t.position = new Vector3(-0.5f, -0.5f, 0);
                    break;
                case Tilemap.Orientation.ZX:
                case Tilemap.Orientation.XZ:
                    t.position = new Vector3(-0.5f, 0, -0.5f);
                    break;
                case Tilemap.Orientation.YZ:
                case Tilemap.Orientation.ZY:
                    t.position = new Vector3(0, -0.5f, -0.5f);
                    break;
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