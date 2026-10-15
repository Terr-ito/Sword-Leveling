using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    Transform target;
    public float moveSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = Object.FindAnyObjectByType<PlayerMovement>().transform;
    }

    // Update is called once per frame
    void Update()   
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
    }
}
