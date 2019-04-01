﻿using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class ColorBean 
{
    public float r;
    public float g;
    public float b;
    public float a;

    public ColorBean()
    {

    }

    public ColorBean(float r, float g, float b)
    {
        this.r = r;
        this.g = g;
        this.b = b;
        this.a = 1;
    }

    public ColorBean(float r,float g,float b,float a)
    {
        this.r = r;
        this.g = g;
        this.b = b;
        this.a = a;
    }

    public Color GetColor()
    {
        return new Color(r,g,b,a);
    }
}