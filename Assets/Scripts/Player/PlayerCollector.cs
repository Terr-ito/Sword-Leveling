using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collided object has a component that implements ICollectible
        if (collision.TryGetComponent(out ICollectible collectible))
        {
            collectible.Collect();
        }
    }
}
