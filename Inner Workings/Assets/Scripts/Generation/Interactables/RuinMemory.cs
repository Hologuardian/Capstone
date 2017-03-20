using System;
using UnityEngine;

public class RuinMemory : Interactable
{
    bool SphereGenerated = false;
    float radius;
    Vector3 position;
    static int memoryIndex;
    const int numRuinMemories = 14;

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
            
            GameObject souls = GameObject.Instantiate(Resources.Load<GameObject>("Soul Anchor"));
            var soulSystem = souls.GetComponentInChildren<ParticleSystem>();
            var soulEmissionShape = soulSystem.shape;
            soulEmissionShape.radius = radius;

            toggle.toggle = rend;
            
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
        GameObject.FindObjectOfType<MemoryManager>().ShowMemory("Ruin" + (memoryIndex % numRuinMemories));
        memoryIndex++;
        if (memoryIndex % numRuinMemories == 0)
        {
            GameObject.FindObjectOfType<MemoryManager>().SwitchToIsolationScene(6.0f);
        }
    }
}