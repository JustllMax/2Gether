using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootPrintBrush : MonoBehaviour
{
    public CustomRenderTexture HeightMap;
    public Material HeightMapUpdate;

    private Camera mainCamera;

    private static readonly int DrawPosition = Shader.PropertyToID("_DrawPosition");
    private static readonly int DrawAngle = Shader.PropertyToID("_DrawAngle");

    private void Start()
    {
        HeightMap.Initialize();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        DrawWithMouse();
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

    void OnTriggerStay(Collider other)
    {
        Debug.Log("collision " + other.gameObject.name);
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            Ray ray = new Ray(other.transform.position, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector2 hitTextureCoord = hit.textureCoord;
                float angle = other.transform.rotation.eulerAngles.y;

                HeightMapUpdate.SetVector(DrawPosition, hitTextureCoord);
                HeightMapUpdate.SetFloat(DrawAngle, angle * Mathf.Deg2Rad);
            }
        }
    }
}