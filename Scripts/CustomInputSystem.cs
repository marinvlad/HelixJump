using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[CreateAssetMenu(menuName = "ScriptableObject/CustomInputSistem")]
public class CustomInputSystem : ScriptableObject
{
    public Action<PointerEventData> onBeginDragEvent;
    public Action<PointerEventData> onEndDragEvent;
    public Action<PointerEventData> onDragEvent;
    public Action onPointerDownEvent;
    public Action onPoniterUpEvent;
}
