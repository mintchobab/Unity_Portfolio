using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractBase : MonoBehaviour
{
    [field: SerializeField]
    public InteractType InteractType;

    protected Sprite buttonImage;
    protected readonly string folderPath = "UI/Interact";


    public abstract void Interact();
    protected abstract void SetButtonImage();


    protected virtual void Awake()
    {
        SetButtonImage();
    }

}
