using UnityEngine;
public class PlayerInteraction : MonoBehaviour
{
    public float MaxDistance = 10.0f;
    public void Update()
    {
        RaycastHit hit;
        Physics.Raycast(gameObject.transform.position, gameObject.transform.forward, out hit);
        if(hit.collider != null && hit.collider.gameObject != null && hit.distance <= MaxDistance)
        {
            InteractableBehaviour behaviour = hit.collider.gameObject.GetComponent<InteractableBehaviour>();
            if (behaviour != null)
            {
                behaviour.Hover();
                if (Input.GetMouseButtonDown(0))
                {
                    behaviour.Interact();
                }
            }
        }
    }
}
