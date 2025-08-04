using UnityEngine;

public class PropsMovement : MonoBehaviour
{
    public Transform[] backgrounds;
    public float tileHeight = 13.2f;
    public Enemy_Spawner enemySpawner;

    private bool isMoving = false;

    private void Update()
    {
        if (!isMoving || enemySpawner == null) return;

        float speed = enemySpawner.GetCurrentSpeed();
        
        foreach (var bg in backgrounds)
        {
            bg.Translate(Vector3.down * speed * Time.deltaTime);
        }

        if (backgrounds[0].position.y <= 0)
        {
            backgrounds[1].position = new Vector3(
                backgrounds[0].position.x,
                backgrounds[0].position.y + tileHeight,
                backgrounds[0].position.z
            );

            var temp = backgrounds[0];
            backgrounds[0] = backgrounds[1];
            backgrounds[1] = temp;
        }
    }

    public void StartMoving()
    {
        isMoving = true;
    }

    public void StopMoving()
    {
        isMoving = false;
    }
}
