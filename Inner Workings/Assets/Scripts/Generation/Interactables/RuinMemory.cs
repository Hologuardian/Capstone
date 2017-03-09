using System;
using UnityEngine;

public class RuinMemory : Interactable
{
    public RuinMemory(Vector3 pos) : base(pos) { }

    public override GameObject GenerateInteractionSphere()
    {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        obj.transform.position = position;
        //obj.GetComponent<Renderer>().enabled = false;

        GameObject obj2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        obj2.transform.position = position;
        obj2.tag = "Memory";
        GameObject particles = GameObject.Instantiate(Resources.Load<GameObject>("MemoryTest"));

        particles.transform.position = Vector3.zero;
        particles.transform.SetParent(obj2.transform, false);

        InteractableBehaviour interactable = obj.AddComponent<InteractableBehaviour>();
        interactable.AttachInteractor(this);
        
        GameObject.Destroy(obj2.GetComponent<Renderer>());
        SphereCollider trigger = obj2.GetComponent<SphereCollider>();
        trigger.isTrigger = true;
        trigger.radius = 13.0f;
        return obj;
    }

    public override void Interact()
    {
        GameObject.FindObjectOfType<MemoryManager>().ShowMemory("Ruin");
    }
}