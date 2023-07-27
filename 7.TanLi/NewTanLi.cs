using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewTanLi : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed;
    private Vector2 tanDir;
    private Vector2 inDir;
    private Vector2 outDir;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        rb = collision.attachedRigidbody;
        if (rb == null) return;
        if(rb != null)
        {
            inDir = -rb.velocity.normalized;
            tanDir = transform.up.normalized;
            float cos = Vector2.Dot(inDir, tanDir);
            if (collision.transform.position.x < transform.position.x)
            {
                outDir = new Vector2(tanDir.x * cos - tanDir.y * (-Mathf.Sqrt(1 - cos * cos)), tanDir.x * (-Mathf.Sqrt(1 - cos * cos)) + tanDir.y * cos);
            }
            else
            {
                outDir = new Vector2(tanDir.x * cos - tanDir.y * Mathf.Sqrt(1 - cos * cos), tanDir.x * Mathf.Sqrt(1 - cos * cos) + tanDir.y * tanDir.y * cos);
            }
            rb.velocity = speed * outDir.normalized;
        }
        
    }
}
