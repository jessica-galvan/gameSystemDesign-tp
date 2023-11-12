using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Color = UnityEngine.Color;

public class ExpandingExplosion : MonoBehaviour
{
    public LineRenderer lineRenderer;
    [SerializeField] private int pointsCount = 50;

    [Header("Info")]
    [SerializeField, ReadOnly] private float maxRadius = 25f, speed = 50, startWidth = 5f;
    [SerializeField, ReadOnly] private Gradient gradient;
    [SerializeField, ReadOnly] private bool canPlay = false;

    public void Initialize()
    {
        lineRenderer.positionCount = pointsCount + 1;

        print("Init Explosion");
    }

    public void Play(Gradient gradient, float maxRadius, float speed, float startWidth = 5f)
    {
        this.gradient = gradient;
        this.maxRadius = maxRadius;
        this.speed = speed;
        this.startWidth = startWidth;

        lineRenderer.colorGradient = gradient;
        canPlay = true;

        print("Play Explosion");
        StartCoroutine(Blast());
    }

    public void Refresh()
    {
        if (!canPlay) return;

        print("Refresh Explosion");

        float currentRadius = 0f;

        if (currentRadius < maxRadius)
        {
            currentRadius += Time.deltaTime * speed;
            Draw(currentRadius);
        }
        else
            canPlay = false;
    }

    private IEnumerator Blast()
    {
        float currentRadius = 0f;

        while(currentRadius < maxRadius)
        {
            currentRadius += Time.deltaTime * speed;
            Draw(currentRadius);
            yield return null;
        }
    }

    private void Draw(float currentRadius)
    {
        float angleBetweenPoints = 360f / pointsCount;

        for (int i = 0; i <= pointsCount; i++)
        {
            float angle = 1 * angleBetweenPoints * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(0f, Mathf.Sin(angle), Mathf.Cos(angle));
            Vector3 vector3 = direction * currentRadius;

            lineRenderer.SetPosition(i, vector3);
        }

        lineRenderer.widthMultiplier = Mathf.Lerp(0, startWidth, 1f - currentRadius / maxRadius);

    }
}
