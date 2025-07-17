using UnityEngine;

public class Destroy_UI : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            this.gameObject.SetActive(false);
        }
    }
}
