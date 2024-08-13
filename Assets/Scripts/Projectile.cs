using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float rayLength = 1f;
    public LayerMask targetLayer;

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero, rayLength, targetLayer);

        if (hit.collider != null)
        {
            Debug.Log($"Hit object: {hit.collider.name}, Tag: {hit.collider.tag}");
            if (hit.collider.CompareTag("Target"))
            {
                Debug.Log("Target hit detected!");
                Target target = hit.collider.GetComponent<Target>();
                if (target != null)
                {
                    target.HandleHit();
                }
                Destroy(hit.collider.gameObject);
                Destroy(gameObject);
            }
        }
    }

    private void OnDrawGizmos()
    {
        // Visualize the ray in the Scene view
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, Vector3.forward * rayLength);
    }
}
