using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;


public class Joystick : MonoBehaviour
{
    [SerializeField]
    private RectTransform stickBG;

    [SerializeField]
    private RectTransform stickButton;

    [SerializeField]
    private float smoothSpeed = 10f;


    public event Action<Vector2> onMovingStick;
    public event Action onMovedStick;

    private Coroutine resetStickPosition;
    private Coroutine drag;

    private Vector3 smoothVelocity;
    
    private float bgRadius;
    private float stickDistRatio;
    private float stickReturnTime = 0.5f;


    public Vector2 StickVector { get; private set; }
    public Vector2 StickVectorByRatio { get => StickVector.normalized * stickDistRatio; }
    public bool IsMoving { get; private set; }
    

    private void Start()
    {
        bgRadius = stickBG.rect.width * 0.5f;
        gameObject.SetActive(false);
    }


    private void OnDisable()
    {
        IsMoving = false;
        onMovedStick?.Invoke();
        stickButton.position = stickBG.position;
        gameObject.SetActive(false);
    }


    public void BeginDrag(PointerEventData eventData)
    {
        IsMoving = true;

        if (drag != null)
            StopCoroutine(drag);

        drag = StartCoroutine(Drag(eventData));
    }


    public IEnumerator Drag(PointerEventData eventData)
    {
        while (true)
        {
            MoveStick(eventData.position);
            onMovingStick?.Invoke(StickVector);
            yield return null;
        }           
    }


    public void EndDrag(PointerEventData eventData)
    {
        IsMoving = false;
        onMovedStick?.Invoke();

        if (resetStickPosition != null)
            StopCoroutine(resetStickPosition);

        resetStickPosition = StartCoroutine(ResetStickPosition());
    }


    private void MoveStick(Vector2 pointerPos)
    {
        StickVector = new Vector2(pointerPos.x - stickBG.position.x, pointerPos.y - stickBG.position.y);
        StickVector = Vector2.ClampMagnitude(StickVector, bgRadius);

        stickButton.localPosition = StickVector;
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

}
