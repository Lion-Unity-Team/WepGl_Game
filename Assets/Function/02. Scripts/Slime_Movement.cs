using UnityEngine;

public class Slime_Movement : MonoBehaviour
{
    private Vector2 mousePos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (mousePos.x > 1)
                transform.position = new Vector2(2, -4);
            else if (mousePos.x < -1)
                transform.position = new Vector2(-2, -4);
            else if (mousePos.x < 1 && mousePos.x > -1)
                transform.position = new Vector2(0, -4);
        }
    }
}
