﻿using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class InteractableBehaviour : MonoBehaviour
{
    Interactable interactor;

    public bool HasMaterialSaved = false;
    public Renderer render;

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
        render.material.SetColor("_Color", Color.Lerp(Color.white, Color.black, resetTimer));
        if (resetTimer >= 0.0f)
        {
            resetTimer -= Time.deltaTime;
        }
    }

    public void Hover()
    {
        if(resetTimer <= 1.0f)
        {
            resetTimer += 3.0f * Time.deltaTime;
        }
    }

    public void Interact()
    {
        interactor.Interact();
        if(interactor is RuinMemory)
        {
            ((RuinMemory)interactor).InteractableObject.SetActive(false);
            if (!((RuinMemory)interactor).InteractionSphere.Active)
                ((RuinMemory)interactor).InteractionSphere.gameObject.SetActive(false);
            else
                ((RuinMemory)interactor).InteractionSphere.Exiting = true;
        }
    }
}