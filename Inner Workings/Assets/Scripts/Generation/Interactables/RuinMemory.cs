using System;
using UnityEngine;

public class RuinMemory : Interactable
{
    bool SphereGenerated = false;
    float radius;
    Vector3 position;

    public RuinMemory(Vector3 pos, float radius)
    {
        this.radius = radius;
        position = pos;
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

            obj.GetComponent<Renderer>().enabled = false;
            obj.transform.SetParent(parent, false);

            GameObject obj2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            SphereToggle toggle = obj2.AddComponent<SphereToggle>();
            obj2.transform.position = position;
            obj2.transform.SetParent(parent, false);
            obj2.tag = "Memory";

            //GameObject particles = GameObject.Instantiate(Resources.Load<GameObject>("MemoryTest"));
            //particles.SetActive(false);
            GameObject souls = GameObject.Instantiate(Resources.Load<GameObject>("Soul Anchor"));
            //ParticleSystem particleSystem = particles.GetComponentInChildren<ParticleSystem>();
            //particleSystem.Stop();
            //souls.GetComponentInChildren<ParticleSystem>().Stop();

            toggle.toggle = rend;

            //particles.transform.position = Vector3.zero;// new Vector3(0, -0.25f, 0);
            //particles.transform.SetParent(obj2.transform, false);
            souls.transform.position = Vector3.zero;
            souls.transform.SetParent(obj2.transform, false);

            InteractableBehaviour interactable = obj.AddComponent<InteractableBehaviour>();
            interactable.AttachInteractor(this);

            GameObject.Destroy(obj2.GetComponent<Renderer>());
            SphereCollider trigger = obj2.GetComponent<SphereCollider>();
            trigger.isTrigger = true;
            trigger.radius = radius;
        }
    }

    public void Interact()
    {
        GameObject.FindObjectOfType<MemoryManager>().ShowMemory("Ruin");
    }
}