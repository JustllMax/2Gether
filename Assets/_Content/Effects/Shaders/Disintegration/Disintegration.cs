using UnityEngine;

public class DisintegrationEffect : MonoBehaviour
{
    private static Material desintegrationMaterial;
    private static int disintegrationTimeID;
    private static int disintegrationTextureID;

    private Renderer[] objectRenderers;
    private MaterialPropertyBlock[] propertyBlocks;
    private float startTime;

    public void Awake()
    {
        startTime = 0;

        if (desintegrationMaterial == null)
        {
            desintegrationMaterial = Resources.Load<Material>("disintegration_material");
            disintegrationTimeID = Shader.PropertyToID("_InstanceDisintegrationTime");
            disintegrationTextureID = Shader.PropertyToID("_MainTex");
        }
    }

    public void Execute()
    {
        if (startTime != 0)
            return;

        startTime = Time.time;
        objectRenderers = GetComponentsInChildren<Renderer>();
        propertyBlocks = new MaterialPropertyBlock[objectRenderers.Length];
        for (int i = 0; i < objectRenderers.Length; i++)
        {
            propertyBlocks[i] = new MaterialPropertyBlock();


            if (objectRenderers[i].material != null && objectRenderers[i].material.mainTexture != null)
            {
                propertyBlocks[i].SetTexture(disintegrationTextureID, objectRenderers[i].material.mainTexture);
            }
            objectRenderers[i].material = desintegrationMaterial;
        }
    }

    void Update()
    {
        if (startTime == 0)
            return;

        float elapsed = Time.time - startTime;

        for (int i = 0; i < objectRenderers.Length; i++)
        {
            propertyBlocks[i].SetFloat(disintegrationTimeID, elapsed);
            objectRenderers[i].SetPropertyBlock(propertyBlocks[i]);
        }
        
        if (elapsed > 3.0f)
        {
            Destroy(gameObject);
        }
    }
}
