using UnityEngine;

public class Slime_Movement : MonoBehaviour
{
    private Vector2 mousePos;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (mousePos.x > 1) // ���콺�� Ŭ�� ��ġ��ǥ�� x�� 1���� ũ�� ������, ������ 0���� ũ�� �߰�, -1���� ������ ���� ��ġ�� ����.
                transform.position = new Vector2(2, -4);    // (2,-4)��ǥ�� ��ġ�ϰ� ����.
            else if (mousePos.x < -1)
                transform.position = new Vector2(-2, -4);
            else if (mousePos.x < 1 && mousePos.x > -1)
                transform.position = new Vector2(0, -4);
        }
    }
}
