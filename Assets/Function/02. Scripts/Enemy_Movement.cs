using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
    public float speed=5.0f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
    private void Update()
    {
        transform.position += new Vector3(0, -1, 0) * speed * Time.deltaTime;    //y�������� -1�� �����̴� ��ɾ� * �ӵ� * �ð��������Ӽӵ�
    }
}
