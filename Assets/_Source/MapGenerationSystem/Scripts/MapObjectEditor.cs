using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapObject))]
public class MapObjectEditor : Editor
{
    private SerializedProperty mapSizeProp;
    private SerializedProperty startPosProp;

    private void OnEnable()
    {
        mapSizeProp = serializedObject.FindProperty("mapSize");
        startPosProp = serializedObject.FindProperty("startPos");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(mapSizeProp);
        EditorGUILayout.PropertyField(startPosProp);

        MapObject mapObject = (MapObject)target;

        if (GUILayout.Button("Initialize Map"))
        {
            InitializeMap(mapObject);
        }

        if (mapObject.map != null && mapObject.map.Length > 0)
        {
            DrawMapEditor(mapObject);
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(mapObject);
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void InitializeMap(MapObject mapObject)
    {
        mapObject.map = new int[mapObject.mapSize.x, mapObject.mapSize.y];
        mapObject.OnBeforeSerialize();
        EditorUtility.SetDirty(mapObject);
    }

    private void DrawMapEditor(MapObject mapObject)
    {
        // Инвертируем порядок строк, чтобы начало массива было в левом нижнем углу
        for (int y = mapObject.mapSize.y - 1; y >= 0; y--)
        {
            EditorGUILayout.BeginHorizontal();
            for (int x = 0; x < mapObject.mapSize.x; x++)
            {
                mapObject.map[x, y] = EditorGUILayout.IntField(mapObject.map[x, y], GUILayout.Width(20));
            }
            EditorGUILayout.EndHorizontal();
        }
        mapObject.OnBeforeSerialize();
        EditorUtility.SetDirty(mapObject);
    }
}
