using UnityEngine;

[RequireComponent(typeof(Camera))]
public class Camera_FullscreenAdaptive : MonoBehaviour
{
    public float referenceWidth = 9f;
    public float referenceHeight = 16f;

    private Camera cam;
    private float initialOrthoSize;

    void Awake()
    {
        cam = GetComponent<Camera>();
        cam.rect = new Rect(0, 0, 1, 1);  
        initialOrthoSize = cam.orthographicSize;

        Apply();
    }

    void OnRectTransformDimensionsChange()
    {
        Apply(); 
    }

    void Apply()
    {
        Debug.Log(cam.orthographicSize);
        float targetAspect = referenceWidth / referenceHeight;
        float deviceAspect = (float)Screen.width / Screen.height;

        if (deviceAspect < targetAspect)
        {
            float scale = targetAspect / deviceAspect;
            cam.orthographicSize = initialOrthoSize * scale;
        }
        else
        {
            cam.orthographicSize = initialOrthoSize;
        }
    }
}
