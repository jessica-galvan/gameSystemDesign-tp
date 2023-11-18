using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Camera main;
    [ReadOnly] public Camera uiCamera;
    [ReadOnly] public Cinemachine.CinemachineVirtualCamera virtualCamera;

    [Header("Directions")]
    public Transform[] top;
    public Transform[] bottom;
    public Transform[] left;
    public Transform[] right;
    public Transform[] topRight;
    public Transform[] topLeft;
    public Transform[] bottomRight;
    public Transform[] bottomLeft;

    private Dictionary<Vector2Int, List<Transform>> spawnPoints = new Dictionary<Vector2Int, List<Transform>>();
    private List<Transform> allSpawnPoints = new List<Transform>();
    private int maxRandom = -1;

    public void Initialize()
    {
        virtualCamera = Instantiate(GameManager.Instance.prefabReferences.virtualCamera);
        uiCamera = Instantiate(GameManager.Instance.prefabReferences.uiCamera);

        virtualCamera.Follow = GameManager.Instance.Player.transform;

        spawnPoints.Clear();
        spawnPoints.Add(Vector2Int.up, top.ToList());
        spawnPoints.Add(Vector2Int.down, bottom.ToList());
        spawnPoints.Add(Vector2Int.left, left.ToList());
        spawnPoints.Add(Vector2Int.right, right.ToList());

        spawnPoints.Add(new Vector2Int(1, 1), topRight.ToList()); //right up
        spawnPoints.Add(new Vector2Int(-1, 1), topLeft.ToList()); //left up
        spawnPoints.Add(new Vector2Int(-1, -1), bottomLeft.ToList()); //left dow
        spawnPoints.Add(new Vector2Int(1, -1), bottomRight.ToList()); //right dow

        allSpawnPoints.Clear();
        allSpawnPoints.AddRange(top);
        allSpawnPoints.AddRange(bottom);
        allSpawnPoints.AddRange(left);
        allSpawnPoints.AddRange(right);
        allSpawnPoints.AddRange(topRight);
        allSpawnPoints.AddRange(topLeft);
        allSpawnPoints.AddRange(bottomLeft);
        allSpawnPoints.AddRange(bottomRight);

        //spawnPoints.Clear();
        //spawnPoints[Vector2Int.up] = topSpawnPoint; //up
        //spawnPoints[Vector2Int.down] = bottomSpawnPoint; //down
        //spawnPoints[Vector2Int.left] = leftSpawnPoint;
        //spawnPoints[Vector2Int.right] = rightSpawnPoint;
        //spawnPoints[new Vector2Int(1,1)] = topRightSpawnPoint; //right up
        //spawnPoints[new Vector2Int(-1, 1)] = topLeftSpawnPoint; //left up
        //spawnPoints[new Vector2Int(-1, -1)] = bottomLeftSpawnPoint; //left down
        //spawnPoints[new Vector2Int(1, -1)] = bottomRightSpawnPoint; //right down

        //allSpawnPoints.Clear();
        //allSpawnPoints.Add(topSpawnPoint);
        //allSpawnPoints.Add(bottomSpawnPoint);
        //allSpawnPoints.Add(leftSpawnPoint);
        //allSpawnPoints.Add(rightSpawnPoint);
        //allSpawnPoints.Add(topRightSpawnPoint);
        //allSpawnPoints.Add(topLeftSpawnPoint);
        //allSpawnPoints.Add(bottomLeftSpawnPoint);
        //allSpawnPoints.Add(bottomRightSpawnPoint);

        maxRandom = allSpawnPoints.Count - 1;
    }

    public Vector2 GetSpawnPoint(Vector2 direction, bool extraRandom, bool isCompletlyRandom = false)
    {
        if (!isCompletlyRandom)
        {
            Vector2Int dir = new Vector2Int((int)direction.x, (int)direction.y);
            if (spawnPoints.TryGetValue(dir, out List<Transform> points))
            {
                var random = Random.Range(0, points.Count - 1);
                var position = points[random].position;

                if (extraRandom)
                {
                    if (direction.x != 0)
                    {
                        var area = GameManager.Instance.globalConfig.invisibleCollision.y / 2 - 1;
                        var newY = Mathf.Clamp(Random.Range(-area, area) + position.y, -area, area);
                        position.y = newY;
                    }

                    if (direction.y != 0)
                    {
                        var area = GameManager.Instance.globalConfig.invisibleCollision.x / 2 - 1;
                        var newY = Mathf.Clamp(Random.Range(-area, area) + position.x, -area, area);
                        position.x = newY;
                    }
                }

                return position;
            }
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

    public bool IsInsideFrustrum(Vector3 currentPos)
    {
        Vector3 distance = transform.position - currentPos;
        distance = new Vector3(Mathf.Abs(distance.x), Mathf.Abs(distance.y), 0f);
        bool xDistance = distance.x <= GameManager.Instance.globalConfig.invisibleCollision.x / 2;
        bool yDistance = distance.y <= GameManager.Instance.globalConfig.invisibleCollision.y / 2;

        return xDistance && yDistance;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        foreach (var point in allSpawnPoints)
            Gizmos.DrawWireSphere(point.position, 0.2f);
    }

}
