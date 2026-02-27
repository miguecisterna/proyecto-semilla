using UnityEngine;

public class EnemyZigZag : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float zigzagFrequency = 5f;
    [SerializeField] private float zigzagAmplitude = 1f;

    private float startX;

    private void Start()
    {
        startX = transform.position.x;
    }

    private void Update()
    {
        float zigzagOffset = Mathf.Sin(Time.time * zigzagFrequency) * zigzagAmplitude;
        Vector3 movement = new Vector3(startX + zigzagOffset, transform.position.y - moveSpeed * Time.deltaTime, 0f);
        transform.position = movement;
    }
}