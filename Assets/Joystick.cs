using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Joystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private RectTransform stickBG;

    [SerializeField]
    private RectTransform stickButton;

    [SerializeField]
    private float smoothSpeed = 10f;

    private Coroutine resetStickPosition;

    public Vector2 StickVector { get; private set; }
    private Vector3 smoothVelocity;
    
    private float bgRadius;
    private float stickDistRatio;
    private float stickReturnTime = 0.5f;


    public Vector2 StickVectorByRatio { get => StickVector.normalized * stickDistRatio; }
    public bool IsMoving { get; private set; }
    



    void Start()
    {
        bgRadius = stickBG.rect.width * 0.5f;
    }



    private void MoveStick(Vector2 pointerPos)
    {
        StickVector = new Vector2(pointerPos.x - stickBG.position.x, pointerPos.y - stickBG.position.y);
        StickVector = Vector2.ClampMagnitude(StickVector, bgRadius);

        stickButton.localPosition = StickVector;

        //stickDistRatio = (stickBG.position - stickButton.position).sqrMagnitude / (bgRadius * bgRadius);
    }


    private IEnumerator ResetStickPosition()
    {
        float time = Time.time;
        float inverseSmoothSpeed = 1 / smoothSpeed;

        while (Time.time < time + stickReturnTime)
        {
            stickButton.position = Vector3.SmoothDamp(stickButton.position, stickBG.position, ref smoothVelocity, inverseSmoothSpeed);
            yield return null;
        }

        stickButton.position = stickBG.position;
    }



    public void OnBeginDrag(PointerEventData eventData)
    {
        IsMoving = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        MoveStick(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        IsMoving = false;

        if (resetStickPosition != null)
            StopCoroutine(resetStickPosition);

        resetStickPosition = StartCoroutine(ResetStickPosition());
    }
}
