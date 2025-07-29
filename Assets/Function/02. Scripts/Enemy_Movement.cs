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
        transform.position += new Vector3(0, -1, 0) * speed * Time.deltaTime;    //y방향으로 -1씩 움직이는 명령어 * 속도 * 시간당프레임속도
    }
}
