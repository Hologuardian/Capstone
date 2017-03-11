using UnityEngine;

public abstract class Interactable
{
    public Interactable(Vector3 pos)
    {
        position = pos;
    }
    public Vector3 position;
    public abstract void GenerateInteractionSphere(Transform parent);
    public abstract void Interact();
}