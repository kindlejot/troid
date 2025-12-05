using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderGuard : MonoBehaviour
{
    public Vector2 ObjectAccumulatedPosition => _objectAccumulatedPosition;

    private float _ortoHeight;
    private float _ortoWidth;

    private Vector3 _objectAccumulatedPosition;
    private Vector3 _previousPosition;

    void Start()
    {
        _ortoHeight = Camera.main.orthographicSize;
        _ortoWidth = Camera.main.orthographicSize * Camera.main.aspect;

        _objectAccumulatedPosition = transform.position;
        _previousPosition = transform.position;
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
        _objectAccumulatedPosition += transform.position - _previousPosition;

    #if UNITY_EDITOR
        _ortoHeight = Camera.main.orthographicSize;
        _ortoWidth = Camera.main.orthographicSize * Camera.main.aspect;
    #endif

        if (transform.position.x > _ortoWidth) {
            Vector3 pos = transform.position;
            pos.x -= _ortoWidth * 2;
            transform.position = pos;
        } else if (transform.position.x < -_ortoWidth) {
            Vector3 pos = transform.position;
            pos.x += _ortoWidth * 2;
            transform.position = pos;
        }
        if (transform.position.y > _ortoHeight) {
            Vector3 pos = transform.position;
            pos.y -= _ortoHeight * 2;
            transform.position = pos;
        } else if (transform.position.y < -_ortoHeight) {
            Vector3 pos = transform.position;
            pos.y += _ortoHeight * 2;
            transform.position = pos;
        }

        _previousPosition = transform.position;
    }
}
