using UnityEngine;

public class CameraSafeAreaTopFill : MonoBehaviour
{
    void Start()
    {
        Camera cam = Camera.main;

        // SafeArea 정보
        Rect safe = Screen.safeArea;

        float screenHeight = Screen.height;

        // SafeArea가 위에서 얼마나 내려와 있는지 (px)
        float topInset = screenHeight - (safe.y + safe.height);

        // Orthographic 기준 월드 단위로 변환
        float unitsPerPixel = cam.orthographicSize * 2f / screenHeight;
        float offsetY = topInset * unitsPerPixel * 0.5f;

        // 카메라를 위로 이동
        cam.transform.position += new Vector3(0, offsetY, 0);
    }
}
