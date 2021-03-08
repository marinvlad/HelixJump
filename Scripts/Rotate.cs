using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Rotate : MonoBehaviour
{
    [SerializeField] private CustomInputSystem _customInputSystem;
    [SerializeField] private float rotationSpeed = 0.25f;
    
    private void OnEnable()
    {
        _customInputSystem.onDragEvent += DoRotation;
    }

    private void OnDisable()
    {
        _customInputSystem.onDragEvent -= DoRotation;
    }
    
    private void DoRotation(PointerEventData eventData)
    {
        float deltaAngle = eventData.delta.magnitude * rotationSpeed;
        if (Mathf.Approximately(deltaAngle, 0.0f))
            return;
        transform.Rotate(0, -deltaAngle * Mathf.Sign(eventData.delta.x),0, Space.World);
    }
}
