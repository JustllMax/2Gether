using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlashSpawner : MonoBehaviour
{
    public float fps = 30.0f;
    public Texture2D[] frames;
    public GameObject muzzleFlashPrefab;

    public void Spawn()
    {
        GameObject gameObject = Instantiate(muzzleFlashPrefab, transform.position, transform.rotation);
        gameObject.transform.parent = transform;
        gameObject.transform.localScale = Vector3.one;

        MuzzleFlash muzzleFlash = gameObject.GetComponent<MuzzleFlash>();
        muzzleFlash.fps = fps;
        muzzleFlash.frames = frames;

    }
}
