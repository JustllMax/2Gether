using UnityEngine;

public class Bomb : MonoBehaviour
{
    private Transform target;
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Collider col;
    private float speed = 15f;
    Vector3 direction;
    float distance;

    public void SeekTarget(Transform _target)
    {
        target = _target;
        direction = target.position - transform.position;
        distance = speed * Time.deltaTime;
    }

    private void Update()
    {      
        transform.Translate(direction.normalized * distance, Space.World);

    }



    private void OnCollisionEnter(Collision c)
    {
        Explode();
    }
    

    private void Explode()
    {
        particles.Play();
        direction = new Vector3(0,0,0);
        distance = 0;
        speed = 0;
        meshRenderer.enabled = false;
        col.enabled = false;
        Destroy(gameObject, 1.1f);
    }

}
