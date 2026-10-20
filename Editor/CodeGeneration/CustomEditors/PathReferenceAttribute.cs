using System.Text;
using UnityEditor;
using UnityEngine;

namespace RoRSkinBuilder.CustomEditors
{
    public class PathReferenceAttribute : CompositePropertyAttribute
    {
        private static GameObject memoizedSceneObject;
        private static string memoizedPath;

        private string GetDraggedObjectPath()
        {
            var objectReferences = DragAndDrop.objectReferences;
            if (objectReferences.Length == 1)
            {
                if (objectReferences[0] is GameObject gameObject)
                {
                    if (!ReferenceEquals(memoizedSceneObject, gameObject))
                    {
                        memoizedSceneObject = gameObject;
                        memoizedPath = Helpers.BuildPath(memoizedSceneObject);
                    }
                    return memoizedPath;
                }
            }
            return null;
        }

        public override bool OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            EditorGUI.PropertyField(position, property, label, true);
            if (position.Contains(Event.current.mousePosition))
            {
                switch (Event.current.type)
                {
                    case EventType.DragUpdated:
                    {
                        var path = GetDraggedObjectPath();
                        if (!string.IsNullOrEmpty(path))
                        {
                            DragAndDrop.visualMode = DragAndDropVisualMode.Link;
                        }
                        break;
                    }
                    case EventType.DragPerform:
                    {
                        var path = GetDraggedObjectPath();
                        if (!string.IsNullOrEmpty(path))
                        {
                            DragAndDrop.AcceptDrag();
                            property.stringValue = path;
                        }
                        break;
                    }
                }
            }

            EditorGUI.EndProperty();
            return true;
        }
    }
}
