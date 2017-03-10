using System;
using UnityEngine;

public class RuinMemory : Interactable
{
    public RuinMemory(Vector3 pos) : base(pos) { }

    public override GameObject GenerateInteractionSphere()
    {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        obj.transform.position = position;
        MeshRenderer rend = obj.GetComponent<MeshRenderer>();
        //rend.material = Resources.Load<Material>("Oscillate");
        obj.GetComponent<Renderer>().enabled = false;

        GameObject obj2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        SphereToggle toggle = obj2.AddComponent<SphereToggle>();
        obj2.transform.position = position;
        obj2.tag = "Memory";

        GameObject particles = GameObject.Instantiate(Resources.Load<GameObject>("MemoryTest"));
        GameObject souls = GameObject.Instantiate(Resources.Load<GameObject>("Soul Anchor"));
        ParticleSystem particleSystem = particles.GetComponentInChildren<ParticleSystem>();
        particleSystem.Stop();
        //souls.GetComponentInChildren<ParticleSystem>().Stop();

        toggle.toggle = rend;
        toggle.particles = particleSystem;
        toggle.souls = souls.GetComponentInChildren<ParticleSystem>();

        particles.transform.position = Vector3.zero;
        particles.transform.SetParent(obj2.transform, false);
        souls.transform.position = Vector3.zero;
        souls.transform.SetParent(obj2.transform, false);

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