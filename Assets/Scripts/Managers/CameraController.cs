using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Camera main;
    [ReadOnly] public Camera uiCamera;
    [ReadOnly] public Cinemachine.CinemachineVirtualCamera virtualCamera;

    [Header("Directions")]
    public Transform topSpawnPoint;
    public Transform leftSpawnPoint;
    public Transform rightSpawnPoint;
    public Transform bottomSpawnPoint;
    public Transform topLeftSpawnPoint;
    public Transform topRightSpawnPoint;
    public Transform bottomLeftSpawnPoint;
    public Transform bottomRightSpawnPoint;

    private Dictionary<Vector2Int, Transform> spawnPoints = new Dictionary<Vector2Int, Transform>();
    private List<Transform> allSpawnPoints = new List<Transform>();
    private int maxRandom = -1;

    public void Initialize()
    {
        virtualCamera = Instantiate(GameManager.Instance.prefabReferences.virtualCamera);
        uiCamera = Instantiate(GameManager.Instance.prefabReferences.uiCamera);

        virtualCamera.Follow = GameManager.Instance.Player.transform;

        spawnPoints.Clear();
        spawnPoints[Vector2Int.up] = topSpawnPoint; //up
        spawnPoints[Vector2Int.down] = bottomSpawnPoint; //down
        spawnPoints[Vector2Int.left] = leftSpawnPoint;
        spawnPoints[Vector2Int.right] = rightSpawnPoint;
        spawnPoints[new Vector2Int(1,1)] = topRightSpawnPoint; //right up
        spawnPoints[new Vector2Int(-1, 1)] = topLeftSpawnPoint; //left up
        spawnPoints[new Vector2Int(-1, -1)] = bottomLeftSpawnPoint; //left down
        spawnPoints[new Vector2Int(1, -1)] = bottomRightSpawnPoint; //right down

        allSpawnPoints.Clear();
        allSpawnPoints.Add(topSpawnPoint);
        allSpawnPoints.Add(bottomSpawnPoint);
        allSpawnPoints.Add(leftSpawnPoint);
        allSpawnPoints.Add(rightSpawnPoint);
        allSpawnPoints.Add(topRightSpawnPoint);
        allSpawnPoints.Add(topLeftSpawnPoint);
        allSpawnPoints.Add(bottomLeftSpawnPoint);
        allSpawnPoints.Add(bottomRightSpawnPoint);

        maxRandom = allSpawnPoints.Count - 1;
    }

    public Vector2 GetSpawnPoint(Vector2 direction, bool extraRandom)
    {
        Vector2Int dir = new Vector2Int((int)direction.x, (int)direction.y);
        if (spawnPoints.TryGetValue(dir, out Transform point))
        {
            var position = point.position;
            if (extraRandom)
            {
                if (direction.x != 0)
                {
                    var area = GameManager.Instance.globalConfig.invisibleCollision.y / 2 - 1;
                    var newY = Mathf.Clamp(Random.Range(-area, area) + position.y, -area, area);
                    position.y = newY;
                }

                if(direction.y != 0)
                {
                    var area = GameManager.Instance.globalConfig.invisibleCollision.x / 2 - 1;
                    var newY = Mathf.Clamp(Random.Range(-area, area) + position.x, -area, area);
                    position.x = newY;
                }
            }

            return position;
        }

        return GetRandomDirection();
    }

    public Vector2 GetRandomDirection()
    {
        int randomNumber = Random.Range(0, maxRandom);
        return allSpawnPoints[randomNumber].position;
    }

    public Vector2 MouseWorldPos()
    {
        var mousePos = Mouse.current.position.ReadValue();
        return main.ScreenToWorldPoint(mousePos);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        foreach (var point in spawnPoints)
            Gizmos.DrawSphere(point.Value.position, 0.5f);
    }

}
