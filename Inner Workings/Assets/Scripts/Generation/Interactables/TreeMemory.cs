using System;
using UnityEngine;

public class TreeMemory : MonoBehaviour, Interactable
{
    bool SphereGenerated = false;
    Vector3 position;

    public void Start()
    {
        GenerateInteractionSphere(transform);
    }

    public void GenerateInteractionSphere(Transform parent)
    {
        if (!SphereGenerated)
        {
            SphereGenerated = true;
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            obj.transform.position = position;

            MeshRenderer rend = obj.GetComponent<MeshRenderer>();
            rend.material = Resources.Load<Material>("Oscillate");
            
            obj.transform.SetParent(parent, false);

            InteractableBehaviour interactable = obj.AddComponent<InteractableBehaviour>();
            interactable.AttachInteractor(this);
        }
    }

    public void Interact()
    {
        GameObject.FindObjectOfType<MemoryManager>().ShowMemory("Tree");
    }
}