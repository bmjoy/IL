﻿using UnityEngine;
using UnityEditor;

public class BaseUIChildComponent<T> : BaseMonoBehaviour
    where T : BaseUIComponent
{
    public T uiComponent;

    public virtual void Close()
    {
        gameObject.SetActive(false);
    }

    public virtual void Open()
    {
        gameObject.SetActive(true);
    }
}