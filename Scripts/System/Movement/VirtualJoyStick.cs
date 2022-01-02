using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoyStick : MonoBehaviour,IBeginDragHandler, 
    IDragHandler,IEndDragHandler
{
    [SerializeField]
    private RectTransform lever;
    private RectTransform rectTransform;

    [SerializeField, Range(10, 150)]
    private float leverRange;

    [SerializeField]
    private PlayerMovement playerController;
   
    private Vector2 inputDirection;
    public bool isInput;

    public enum JoyStickType { Move, Attack, Change}
    public JoyStickType joystickType;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        lever.anchoredPosition = Vector2.zero;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        ControlJoyStickLever(eventData);
        isInput = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        ControlJoyStickLever(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {       
        lever.anchoredPosition = Vector2.zero;
        isInput = false;
         
        playerController.Move(Vector2.zero);
    }

    private void ControlJoyStickLever(PointerEventData eventData)
    {
        var inputPos = eventData.position - rectTransform.anchoredPosition;
        var inputVector = inputPos.magnitude < leverRange ? inputPos : inputPos.normalized * leverRange;
        lever.anchoredPosition = inputVector;
        inputDirection = inputVector / leverRange;
    }

    private void InputControlVector()
    {
        switch (joystickType)
        {
            case JoyStickType.Move:
            playerController.Move(inputDirection);
                break;
            case JoyStickType.Attack:
                break;
            case JoyStickType.Change:  
                break;
        }
    }

    private void Update()
    {
        if(isInput)
        {
            InputControlVector();
        }              
    }
}
