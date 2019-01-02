#pragma warning disable 0414

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ResponsiveElementUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler {

    [SerializeField] bool isOver = false;
    [SerializeField] UnityEvent MouseOver;
    [SerializeField] UnityEvent MouseLeave;
    [SerializeField] UnityEvent MouseDown;
    [SerializeField] UnityEvent MouseUp;

    public void OnPointerDown(PointerEventData eventData) {
        isOver = true;
        MouseDown.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        isOver = true;
        MouseOver.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData) {
        isOver = false;
        MouseLeave.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData) {
        isOver = false;
        MouseUp.Invoke();
    }
}
