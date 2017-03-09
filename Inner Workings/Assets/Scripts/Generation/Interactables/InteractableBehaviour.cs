using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class InteractableBehaviour : MonoBehaviour
{
    Interactable interactor;

    bool HasMaterialSaved = false;
    Material baseMaterial;
    Renderer render;

    public void Start()
    {
        render = gameObject.GetComponent<Renderer>();
    }

    float resetTimer = 0f;

    public void AttachInteractor(Interactable interactorObject)
    {
        interactor = interactorObject;
    }

    public void Update()
    {
        if(resetTimer <= 0.0f)
        {
            if(HasMaterialSaved)
                render.material = baseMaterial;
        }
        else
        {
            resetTimer -= Time.deltaTime;
        }
    }

    public void Hover(Material hover)
    {
        if (!HasMaterialSaved)
        {
            baseMaterial = render.material;
            HasMaterialSaved = true;
        }
        render.material = hover;
        resetTimer = 0.1f;
    }

    public void Interact()
    {
        interactor.Interact();
    }
}