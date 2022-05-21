using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class door : NetworkBehaviour
{
    [SerializeField]
    float openDoor;
    [SerializeField]
    float closeDoor;
    [SerializeField]
    float speed = 1;
    [SyncVar, SerializeField]
    float rotate;

    [SyncVar] public bool isOpen;

    void Update()
    {

        if (isOpen)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.x, rotate, transform.rotation.z), speed * Time.deltaTime);
    }

    public void OpenDoor()
    {
        rotate = openDoor;
    }

    void CloseDoor()
    {
        rotate = closeDoor;
    }

}
