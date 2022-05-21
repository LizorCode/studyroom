using UnityEngine;

public class DoorController : MonoBehaviour
{
    public float distance = 2f;

    public Camera mainCamera;
    
    [Command]
    void Update()
    {
        Debug.Log("����� �����1");
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("����� �����2");
            Ray ray = mainCamera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, distance))
            {
                if (hit.collider.tag == "Door")
                {
                    Debug.Log("����� �����3");
                    door dr = hit.collider.GetComponent<door>();
                    dr.isOpen = !dr.isOpen;
                }
            }
        }
    }

    [Command]
    private void CmdInputHander() 
    {
        Debug.Log("����� �����");
        SerInputHander();
    }

    [Server]
    private void SerInputHander() 
    {

        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, distance))
        {
            if (hit.collider.tag == "Door")
            {
                door dr = hit.collider.GetComponent<door>();
                dr.isOpen = !dr.isOpen;
            }
        }
        
    }

}
