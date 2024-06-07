using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootPrintBrush : MonoBehaviour
{
    public CustomRenderTexture HeightMap;
    public Texture2D frame;
    public Material HeightMapUpdate;

    public float SecondsToRestore = 100;

    public GameObject[] Tires;

    private Camera mainCamera;
    private int tireIndex;

    private float timeToRestoreOneTick;
    
    private static readonly int DrawPosition = Shader.PropertyToID("_DrawPosition");
    private static readonly int DrawAngle = Shader.PropertyToID("_DrawAngle");
    private static readonly int RestoreAmount = Shader.PropertyToID("_RestoreAmount");

    private void Start()
    {
        HeightMap.Initialize();
        mainCamera = Camera.main;


    }

    private void Update()
    {
        // Раскомментируйте одну из этих строчек, чтобы выбрать какие объекты будут копать снег
        DrawWithMouse();
        DrawWithTires();

        // Считаем таймер до восстановления каждого пикселя текстуры на единичку 
        timeToRestoreOneTick -= Time.deltaTime;
        if (timeToRestoreOneTick < 0)
        {
            // Если в этот update мы хотим увеличить цвет всех пикселей карты высот на 1
            HeightMapUpdate.SetFloat(RestoreAmount, 1 / 250f);
            timeToRestoreOneTick = SecondsToRestore / 250f;
        }
        else
        {
            // Если не хотим
            HeightMapUpdate.SetFloat(RestoreAmount, 0);
        }
        
        HeightMap.Update();
    }

    private void DrawWithMouse()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector2 hitTextureCoord = hit.textureCoord;

                HeightMapUpdate.SetVector(DrawPosition, hitTextureCoord);
                HeightMapUpdate.SetFloat(DrawAngle, 45 * Mathf.Deg2Rad);
            }
        }
    }

    private void DrawWithTires()
    {
        GameObject tire = Tires[tireIndex++ % Tires.Length];

        Ray ray = new Ray(tire.transform.position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector2 hitTextureCoord = hit.textureCoord;
            float angle = tire.transform.rotation.eulerAngles.y;

            HeightMapUpdate.SetVector(DrawPosition, hitTextureCoord);
            HeightMapUpdate.SetFloat(DrawAngle, angle * Mathf.Deg2Rad);
        }
    }
}
