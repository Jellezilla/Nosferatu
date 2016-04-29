using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;

public class ObjectUtilities : MonoBehaviour
{

    [@MenuItem("Custom/Replace Object")]
    static void Replace()
    {
        Object[] targetObjects = FindObjectsOfType(typeof(GameObject));

        foreach (Object obj in targetObjects)
        {
            if (obj.name.Contains("Tombstones.001") || obj.name.Contains("Tombstones.002") || obj.name.Contains("Tombstones.003") || obj.name.Contains("Tombstones.004"))
            {
                GameObject replacement = new GameObject("TombstoneSP");
                GameObject replacedObject = obj as GameObject;
                replacement.transform.position = replacedObject.transform.position;
                replacement.transform.SetParent(replacedObject.transform.parent);
                DestroyImmediate(obj);
            }
        }
    }

    [MenuItem("Custom/Set Blue Label Icon to selected")]
    static void SetIcon()
    {
        foreach (GameObject selectionObj in Selection.gameObjects)
        {
            IconManager.SetIcon(selectionObj, IconManager.LabelIcon.Blue);
        }
    }

    [MenuItem("Custom/Clear Icon to selected")]
    static void ClearIcon()
    {
        foreach (GameObject selectionObj in Selection.gameObjects)
        {
            IconManager.ClearIcon(selectionObj);
        }
    }
}
