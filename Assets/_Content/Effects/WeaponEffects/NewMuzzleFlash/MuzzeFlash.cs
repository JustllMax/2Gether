using UnityEngine;
using System.Collections;

public class MuzzleFlash : MonoBehaviour
{
    public float fps;
    public Texture2D[] frames;
    public Transform scale;

    public MeshRenderer renderer;
    Transform parent;
    int frameIndex;
    float lerpValue = 1f;

    void Start()
    {
        parent = transform.parent;
        scale.localScale = Vector3.zero;
        StartCoroutine(Animate());
    }

    private void Update()
    {
        scale.localScale = Vector3.Lerp(scale.localScale, Vector3.one, fps * 2f * Time.deltaTime);
    }

    private void LateUpdate()
    {
        if (frameIndex >= 2 && parent != null)
        {
            transform.position = Vector3.Lerp(transform.position, parent.position, lerpValue);
            transform.rotation = Quaternion.Lerp(transform.rotation, parent.rotation, lerpValue);
            lerpValue = Mathf.Max(0, lerpValue - Time.deltaTime * fps);
        }

    }

    IEnumerator Animate()
    {
        while (frameIndex < frames.Length)
        {
            renderer.material.SetTexture("_MainTex", frames[frameIndex]);
            frameIndex = frameIndex + 1;

            if (frameIndex == 2)
                transform.parent = null;
            yield return new WaitForSeconds(1f / fps);
        }

        Destroy(gameObject);
    }
}