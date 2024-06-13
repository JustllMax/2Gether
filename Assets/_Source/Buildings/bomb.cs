using UnityEngine;

public class Bomb : MonoBehaviour
{
    private Transform target;
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Collider col;
    [SerializeField] private float overlapRadius = 0.15f;
    private AIController controller;
    private float speed = 30f;
    private float damage = 60f;
    Vector3 direction;
    float distance;
    private bool exploded = false;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")){
            Explode();
            ExplosionDamage(transform.position, 2f, 3f);
            Destroy(gameObject, 1.1f);
        }
    }

    private void Explode()
    {
        if (exploded)
            return;

        exploded = true;
        particles.Play();
        direction = Vector3.zero;
        distance = 0;
        speed = 0;
        meshRenderer.enabled = false;
        col.enabled = false;


    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, overlapRadius);
    }

    private void ExplosionDamage(Vector3 center, float radius, float force)
    {

        int enemyLayer = LayerMask.GetMask("Enemy");
        Collider[] hitColliders = Physics.OverlapSphere(center, radius, enemyLayer);

        foreach (var hitCollider in hitColliders)
        {
            Rigidbody rb = hitCollider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                //push
                Vector3 direction = hitCollider.transform.position - center;
                rb.AddForce(direction.normalized * force, ForceMode.Impulse);
            }



            if (hitCollider.TryGetComponent(out AIController controller))
            {

                controller.TakeDamage(damage);

            }
        }
    }




}

