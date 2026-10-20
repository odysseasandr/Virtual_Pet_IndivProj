using UnityEngine;

public class DepthScaler : MonoBehaviour
{
    [Header("Boundaries")]
    public float highY = 0.35f;
    public float lowY = -1.6f;

    [Header("Pet Size")]
    public float minScale = 1f;
    public float maxScale = 3.5f;

    void Update()
    {
        float percent = Mathf.InverseLerp(highY, lowY, transform.position.y);
        float currentScale = Mathf.Lerp(minScale, maxScale, percent);
        transform.localScale = new Vector3(currentScale, currentScale, currentScale);
    }
}