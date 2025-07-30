using UnityEngine;

public class Slime_Movement : MonoBehaviour
{
    private Vector2 mousePos;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (mousePos.x > 1) // 마우스의 클릭 위치좌표의 x가 1보다 크면 오른쪽, 작은데 0보다 크면 중간, -1보다 작으면 왼쪽 터치로 판정.
                transform.position = new Vector2(2, -4);    // (2,-4)좌표에 위치하게 만듬.
            else if (mousePos.x < -1)
                transform.position = new Vector2(-2, -4);
            else if (mousePos.x < 1 && mousePos.x > -1)
                transform.position = new Vector2(0, -4);
        }
    }
}
