using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Step : MonoBehaviour
{
    [SerializeField] private Material _destroyMaterial;
    [SerializeField] private IntEvent _onStepDestroyEvent;

    private void OnEnable()
    {
        _onStepDestroyEvent.Subscribe(OnStepDestroy);
    }

    private void OnDisable()
    {
        _onStepDestroyEvent.Unsubscribe(OnStepDestroy);
    }

    private void OnStepDestroy(int objectID)
    {
        if (objectID != gameObject.GetInstanceID())
            return;

        foreach (Transform child in gameObject.transform)
        {
            MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();
            if(meshRenderer!=null)
                meshRenderer.material = _destroyMaterial;
            
            Rigidbody rigidbody = child.GetComponent<Rigidbody>();
            if(rigidbody!=null)
            {
                rigidbody.constraints = RigidbodyConstraints.None;
                rigidbody.useGravity = true;
                rigidbody.AddForce(Random.onUnitSphere * Random.Range(0,50), ForceMode.Impulse);
            }

            MeshCollider meshCollider = child.GetComponent<MeshCollider>();
            if(meshCollider!=null)
                meshCollider.enabled = false;
        }
        if(gameObject.CompareTag("StartStep") || gameObject.CompareTag("Final"))
        {
            Destroy(gameObject);
        }
        else
        {
            
        }
        Destroy(gameObject,0.5f);
    }
}