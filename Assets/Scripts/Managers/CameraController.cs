using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Camera main;
    private Vector3[] frustumCorners = new Vector3[4];

    public void Initialize()
    {
        main = GetComponent<Camera>();
        main.CalculateFrustumCorners(new Rect(0, 0, 1, 1), main.farClipPlane, Camera.MonoOrStereoscopicEye.Mono, frustumCorners);
    }


    private void Update()
    {
        GetSpawnPoint(Vector2.zero);
    }

    public Vector2 GetSpawnPoint(Vector2 direction)
    {
        for (int i = 0; i < 4; i++)
        {
            var worldSpaceCorner = main.transform.TransformVector(frustumCorners[i]);
            Debug.DrawRay(main.transform.position, worldSpaceCorner, Color.blue);
        }
        return Vector2.zero;

        main.ViewportToWorldPoint(direction);
    }

    public Vector2 MouseWorldPos()
    {
        var mousePos = Mouse.current.position.ReadValue();
        return main.ScreenToWorldPoint(mousePos);
    }

}
