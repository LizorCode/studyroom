using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkControl : MonoBehaviour
{
    public float distance = 2f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, distance))
            {
                if (hit.collider.tag == "Link")
                {
                    Application.OpenURL("https://www.youtube.com/playlist?list=PLpyssslyeRz6Yd4SdrY-O_kyFiyeK8w6l");
                }
            }
        }


        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, distance))
            {
                if (hit.collider.tag == "Link")
                {
                    Application.OpenURL("https://docs.unity3d.com/Manual/index.html");
                }
            }
        }

    }
}
