using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Hegemon/Pop/Class data", fileName = "PopClassData")]
public sealed partial class PopClassDataSerialized : ScriptableObject
{
    public const uint POP_CLASS_CAPACITY = 256;

    public PopClassDataSerializedElement[] Elements;

    public void Check()
    {
        if (Elements == null)
            return;

        var popClassesUsed = new HashSet<PopClass>(Elements.Length);

        for (int i = 0; i < Elements.Length; i++)
        {
            CheckElement(i, in popClassesUsed);
        }
    }

    public void CheckAndErase()
    {
        if (Elements == null)
            return;

        var popClassesUsed = new HashSet<PopClass>(Elements.Length);
        var elementsValid = new List<PopClassDataSerializedElement>(Elements.Length);

        for (int i = 0; i < Elements.Length; i++)
        {
            if (CheckElement(i, in popClassesUsed))
            {
                elementsValid.Add(Elements[i]);
            }
        }

        Elements = elementsValid.ToArray();
    }

    bool CheckElement(int index, in HashSet<PopClass> popClassesUsed)
    {
        ref readonly var el = ref Elements[index];

        if ((uint)el.Class >= POP_CLASS_CAPACITY)
        {
            Debug.LogWarning($"PopClassData :: Validate :: Element ({index}, {el.Class}) exceeds capacity ({POP_CLASS_CAPACITY})!");
            return false;
        }

        if (!popClassesUsed.Add(el.Class))
        {
            Debug.LogWarning($"PopClassData :: Validate :: Element ({index}) has already used PopClass ({el.Class})!");
            return false;
        }

        return true;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(PopClassDataSerialized))]
public sealed class PopClassDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var pcd = (PopClassDataSerialized)target;

        if (GUILayout.Button("Check"))
        {
            pcd.Check();
        }

        if (GUILayout.Button("Check and erase"))
        {
            pcd.CheckAndErase();
        }
    }
}
#endif