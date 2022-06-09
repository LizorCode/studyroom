using UnityEngine;
using Mirror;

public class DoorController : NetworkBehaviour
{
    public float distance = 2f;

    public Camera mainCamera;

    [Command]
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.E))
        {         
            Ray ray = mainCamera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
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
