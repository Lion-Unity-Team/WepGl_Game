using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slime_Movement : MonoBehaviour
{
    private Vector2 mousePos;
    public bool canMove=true;

    private void OnEnable()
    {
        canMove = true;
    }

    void Update()
    {
        if (!canMove) return; //UI켜져있을때 클릭 무시

        if (EventSystem.current.IsPointerOverGameObject()) return;  //UI위 클릭은 이동무시

        if (Input.GetMouseButtonDown(0))
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (mousePos.x > 1)
                transform.position = new Vector2(2, -4);
            else if (mousePos.x < -1)
                transform.position = new Vector2(-2, -4);
            else if (mousePos.x < 1 && mousePos.x > -1)
                transform.position = new Vector2(0, -4);
            
            SoundManager.instance.SfxPlay("Move");
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
