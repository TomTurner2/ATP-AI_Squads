using System;
using AIStateSystem;
using UnityEditor;
using UnityEngine;


[CustomPropertyDrawer(typeof(State))]
public class DisplayScriptableObjectPropertiesDrawer : PropertyDrawer
{
    bool show = false;
    float draw_height = 0;
    private float position_increment = 20;
    private float position_height = 16;

    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var editor = Editor.CreateEditor(property.objectReferenceValue);
        var indent = EditorGUI.indentLevel;
        Rect temp = new Rect(position.x, position.y, 16, 16);

        if (GUI.Button(temp, ""))
            show = !show;

        draw_height = 0;
        position.height = position_height;
        EditorGUI.PropertyField(position, property);
        position.y += position_increment;

        if (!show)
            return;

        if (editor == null)
            return;

        position.x += position_increment;
        position.width -= position_increment * 2;

        var serialized_object = editor.serializedObject;
        serialized_object.Update();

        var prop = serialized_object.GetIterator();
        prop.NextVisible(true);

        bool show_childen = false;
        int depth_childen = 0;
        

        while (prop.NextVisible(true))
        {
            if (prop.depth == 0)
            {
                show_childen = false;
                depth_childen = 0;
            }

            if (show_childen && prop.depth > depth_childen)
            {
                continue;
            }

            position.height = position_height;
            EditorGUI.indentLevel = indent + prop.depth;


            if (EditorGUI.PropertyField(position, prop))
            {
                show_childen = false;
            }
            else
            {
                show_childen = true;
                depth_childen = prop.depth;
            }

            position.y += position_increment;
            SetDrawerHeight(position_increment);
        }

        if (GUI.changed)
        {
            serialized_object.ApplyModifiedProperties();
        }
    }


    public override float GetPropertyHeight(SerializedProperty _property, GUIContent _label)
    {
        float height = base.GetPropertyHeight(_property, _label);
        height += draw_height;
        return height;
    }


    void SetDrawerHeight(float _height)
    {
        draw_height += _height;
    }
}