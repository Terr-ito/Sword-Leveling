using UnityEngine;

public class ChunkTrigger : MonoBehaviour
{
    MapController mc;

    public GameObject targetChunk;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mc = Object.FindAnyObjectByType<MapController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            mc.currentChunk = targetChunk;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(mc.currentChunk == targetChunk)
            {
                mc.currentChunk = null;
            }
        }
    }
}
