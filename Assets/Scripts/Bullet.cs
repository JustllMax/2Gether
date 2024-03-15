using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private Transform target;
    public float speed = 1000.0f;

    public void setTarget(Transform _target)
    {
        target = _target;
    }

    void Update()
    {
        if(target == null) 
        { 
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distThisFrame = speed * Time.deltaTime;
        if(dir.magnitude <= distThisFrame)
        {
            hitTarget();
            return;
        }

        transform.Translate(dir.normalized * distThisFrame, Space.World);

    }

    void hitTarget()
    {
        Debug.Log("hit");
        Destroy(gameObject);
    }
}
