using System;
using UnityEngine;

public class RuinMemory : Interactable
{
    public RuinMemory(Vector3 pos) : base(pos) { }

    public override GameObject GenerateInteractionSphere()
    {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        obj.transform.position = position;

        obj.AddComponent<InteractableBehaviour>();
        obj.GetComponent<InteractableBehaviour>().AttachInteractor(this);
        return obj;
    }

    public override void Interact()
    {
        Debug.Log("Hello? From: " + position);
    }
}
