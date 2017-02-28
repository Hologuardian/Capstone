using UnityEngine;

public abstract class Interactable
{
    public Interactable(Vector3 pos)
    {
        position = pos;
    }
    public Vector3 position;
    public abstract GameObject GenerateInteractionSphere();
    public abstract void Interact();
}