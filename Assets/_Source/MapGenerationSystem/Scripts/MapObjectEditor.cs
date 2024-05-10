using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapObject))]
public class MapObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MapObject mapObject = (MapObject)target;

        EditorGUI.BeginChangeCheck();

        EditorGUILayout.LabelField("Map Size");
        mapObject.mapSize = EditorGUILayout.Vector2IntField("Size", mapObject.mapSize);

        EditorGUILayout.LabelField("Start Position");
        mapObject.startPos = EditorGUILayout.Vector2IntField("Position", mapObject.startPos);

        EditorGUILayout.LabelField("Map");
        for (int i = 0; i < mapObject.map.GetLength(0); i++)
        {
            EditorGUILayout.BeginHorizontal();

            for (int j = 0; j < mapObject.map.GetLength(1); j++)
            {
                mapObject.map[i, j] = EditorGUILayout.IntField(mapObject.map[i, j]);
            }

            EditorGUILayout.EndHorizontal();
        }
        
        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(mapObject);
        }
    }
}
