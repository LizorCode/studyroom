using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideControl : MonoBehaviour
{
    // Start is called before the first frame update

    public float distanseToSee;
    RaycastHit whatIHit;
    public GameObject Slide1, Slide2, Slide3;
    //Collider ActiveCollider1, ActiveCollider2;

    void Start()
    {
        //ActiveCollider1 = GetComponent<Collider>();
        //ActiveCollider2 = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(this.transform.position, this.transform.forward * distanseToSee, Color.magenta);
        if (Physics.Raycast(this.transform.position,this.transform.forward, out whatIHit, distanseToSee))
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (whatIHit.collider.tag == "Slider")
                {
                    if (Slide1.activeSelf == true) 
                    {
                        Slide1.SetActive(false); 
                        Slide1.GetComponent<BoxCollider>().enabled = false;  
                    }
                    else if (Slide2.activeSelf == true) 
                    { 
                        Slide2.SetActive(false);
                        Slide2.GetComponent<BoxCollider>().enabled = false;
                    }
                    else if (Slide3.activeSelf == true)
                    { 
                        Slide1.SetActive(true);
                        Slide1.GetComponent<BoxCollider>().enabled = true;
                        Slide2.SetActive(true);
                        Slide2.GetComponent<BoxCollider>().enabled = true;
                    };
                }
            }
        }
    }
}
