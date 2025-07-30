using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
    public float speed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
        
    }
    private void Update()
    {
        transform.position += new Vector3(0, -1, 0) * speed * Time.deltaTime;    //y방향으로 -1씩 움직이는 명령어 * 속도 * 시간당프레임속도
    }
}
