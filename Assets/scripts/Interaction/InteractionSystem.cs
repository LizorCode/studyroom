using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSystem : MonoBehaviour
{
    [SerializeField] public RaycastData data = null;
    [SerializeField] public Transform viewCamera = null;
    [SerializeField] public float interactionDistance = 5f;
    [SerializeField] public LayerMask layersToRaycast = 0;
    void Start()
    {
        data.Reset();
    }

   
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("it works correctly!");
            if (data.HitTransform)
            {
                var component = data.HitTransform.GetComponent<IInteractable>();
                if (component != null)
                {
                    component.Interact();
                }
            }
        }
        
        if (Time.frameCount % 4 == 0)
        {
            RaycastHit? hit = DoRaycasting();
                
            if (hit.HasValue)
            {
                if (data.IsThisNewObject(hit.Value.transform))
                {
                        data.UpdateData(hit);
                }
            }
            else
            {
                if (data.HitTransform)
                {
                    data.Reset();
                }
            }
        }
    }

    private RaycastHit? DoRaycasting()
    {
        Ray ray = new Ray(viewCamera.position, viewCamera.forward);
        RaycastHit hit;
        bool hasHit = Physics.Raycast(ray, out hit, interactionDistance, layersToRaycast);
        if (hasHit) {
            return hit;
        }
        return null;
    }
}
