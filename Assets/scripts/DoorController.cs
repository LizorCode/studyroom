using UnityEngine;
using Mirror;

public class DoorController : NetworkBehaviour
{
    public float distance = 2f;
    public Camera mainCamera;
    [Command]
    void Update()
    {
        Debug.Log("Дошел блять1");
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Дошел блять2");
            Ray ray = mainCamera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, distance))
            {
                if (hit.collider.tag == "Door")
                {
                    Debug.Log("Дошел блять3");
                    door dr = hit.collider.GetComponent<door>();
                    dr.isOpen = !dr.isOpen;
                }
            }
        }
    }

    [Command]
    private void CmdInputHander() 
    {
        Debug.Log("Дошел блять");
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
