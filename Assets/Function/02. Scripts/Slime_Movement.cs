using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Slime_Movement : MonoBehaviour
{
    private Vector2 mousePos;
    private bool canMove=true;

    void Update()
    {
        if (!canMove)
            return;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(EnableMovement(0.5f));
    }

    IEnumerator EnableMovement(float delay)
    {
        canMove = false;
        yield return new WaitForSeconds(delay);
        canMove = true;
    }

    
}
