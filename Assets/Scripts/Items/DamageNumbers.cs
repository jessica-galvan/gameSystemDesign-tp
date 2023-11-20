using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageNumbers : MonoBehaviour, IPoolable, IUpdate
{
    public TextMeshPro text;
    public float timeAlive;
    public AnimationCurve animationSizeCurve;
    public float fadingStart = 0.2f, minFadingAmount = 0.2f, speedMovenet = 10f;
    public Vector3 movementDirection= new Vector3(0, 1f, 0f);
    public float maxDistance = 1f;

    private float currentT, currentTimer;
    private Color fadingColor;
    private Vector3 startingPos;

    public void Initialize() { }

    public void Refresh(float deltaTime)
    {
        currentTimer -= deltaTime;
        currentT = Mathf.InverseLerp(0, timeAlive, currentTimer);

        transform.localScale = Vector3.one * animationSizeCurve.Evaluate(currentT);

        if(Vector3.Distance(startingPos, transform.position) <= maxDistance)
        {
            var offset = movementDirection * speedMovenet * deltaTime;
            transform.position += offset;
        }

        if (fadingStart <= currentT)
        {
            fadingColor = text.color;
            fadingColor.a = minFadingAmount + currentT;
            text.color = fadingColor;
        }

        if(currentT <= 0f)
            ReturnToPool();
    }

    public void ReturnToPool()
    {
        text.enabled = false;
        GameManager.Instance.updateManager.uiCustomUpdate.Remove(this);
    }

    public void SetData(int currentAmount, Vector3 position)
    {
        startingPos = position;
        transform.position = position;
        text.SetText(currentAmount.ToString());
        text.enabled = true;
        text.color = Color.white;
        GameManager.Instance.updateManager.uiCustomUpdate.Add(this);
        currentTimer = timeAlive;
        currentT = 1f;
    }
}
