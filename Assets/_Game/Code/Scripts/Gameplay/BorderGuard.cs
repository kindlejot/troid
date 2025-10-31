using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderGuard : MonoBehaviour
{
    float ortoHeight;
    float ortoWidth;

    void Start()
    {
        ortoHeight = Camera.main.orthographicSize;
        ortoWidth = Camera.main.orthographicSize * Camera.main.aspect;
    }

    void OnDrawGizmosSelected()
    {
        float ortoHeight = Camera.main.orthographicSize;
        float ortoWidth = Camera.main.orthographicSize * Camera.main.aspect;

        Vector3 topLeft = Vector3.up * ortoHeight + Vector3.left * ortoWidth;
        Vector3 topRight = Vector3.up * ortoHeight + Vector3.right * ortoWidth;
        Vector3 bottomLeft = Vector3.down * ortoHeight + Vector3.left * ortoWidth;
        Vector3 bottomRight = Vector3.down * ortoHeight + Vector3.right * ortoWidth;

        Debug.DrawLine (topLeft, topRight, Color.red);
        Debug.DrawLine (topRight, bottomRight, Color.red);
        Debug.DrawLine (bottomRight, bottomLeft, Color.red);
        Debug.DrawLine (bottomLeft, topLeft, Color.red);
    }

    void Update()
    {
    #if UNITY_EDITOR
        ortoHeight = Camera.main.orthographicSize;
        ortoWidth = Camera.main.orthographicSize * Camera.main.aspect;
    #endif

        if (transform.position.x > ortoWidth) {
            Vector3 pos = transform.position;
            pos.x -= ortoWidth * 2;
            transform.position = pos;
        } else if (transform.position.x < -ortoWidth) {
            Vector3 pos = transform.position;
            pos.x += ortoWidth * 2;
            transform.position = pos;
        }
        if (transform.position.y > ortoHeight) {
            Vector3 pos = transform.position;
            pos.y -= ortoHeight * 2;
            transform.position = pos;
        } else if (transform.position.y < -ortoHeight) {
            Vector3 pos = transform.position;
            pos.y += ortoHeight * 2;
            transform.position = pos;
        }
    }
}
