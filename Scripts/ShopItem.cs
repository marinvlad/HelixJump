using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour, IPointerDownHandler
{

    [SerializeField] private MeshHandler _meshHandler;
    [SerializeField] private StepMesh _material;
    private bool _selectable = true;
    private Image _image;
    private void OnEnable()
    {
        _image = GetComponent<Image>();
        _meshHandler._onPlayerDataLoad.Subscribe(IsOwnedByPlayer);
    }

    private void OnDisable()
    {
        _meshHandler._onPlayerDataLoad.Unsubscribe(IsOwnedByPlayer);
    }

    public void OnPointerDown(PointerEventData eventData)
    {    
        if(!_selectable)
            return;
        Debug.Log("ShopItem.cs -> Shop item button pressed");
        _meshHandler.CurrentMesh = _material.id;
    }

    private void IsOwnedByPlayer()
    {
        if(!_meshHandler.PlayerMeshes.Contains(_material.id))
        {
            _image.color = Color.black;
            _selectable = false;
        }
        else
        {
            _image.color = Color.white;
            _selectable = true;
        }
        Debug.Log("ShopItem.cs -> IsOwnedByPlayer");
    }
    
}
