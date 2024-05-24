using UnityEngine;

public class DisintegrationEffect : MonoBehaviour
{
    private static Material desintegrationMaterial;
    private static int disintegrationTimeID;
    private static int disintegrationTextureID;

    private MaterialPropertyBlock propertyBlock;
    private Renderer objectRenderer;
    private float startTime;


    void Start()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.AddComponent<DisintegrationEffect>();
        }

        startTime = Time.time;

        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer == null)
            return;

        if (desintegrationMaterial == null)
        {
            desintegrationMaterial = Resources.Load<Material>("disintegration_material");
            disintegrationTimeID = Shader.PropertyToID("_InstanceDisintegrationTime");
            disintegrationTextureID = Shader.PropertyToID("_MainTex");
        }

        propertyBlock = new MaterialPropertyBlock();

        if (objectRenderer.material != null && objectRenderer.material.mainTexture != null)
        {
            propertyBlock.SetTexture(disintegrationTextureID, objectRenderer.material.mainTexture);
        }
        objectRenderer.material = desintegrationMaterial;
    }

    void Update()
    {
        float elapsed = Time.time - startTime;

        if (objectRenderer != null)
        {
            propertyBlock.SetFloat(disintegrationTimeID, elapsed);
            objectRenderer.SetPropertyBlock(propertyBlock);
        }
        
        if (elapsed > 3.0f)
        {
            Destroy(gameObject);
        }
    }
}
