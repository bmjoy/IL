﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class CptUtil
{
    /// <summary>
    /// 删除所有子物体
    /// </summary>
    /// <param name="tf"></param>
    public static void RemoveChild(Transform tf)
    {
        for (int i = 0; i < tf.childCount; i++)
        {
          GameObject.Destroy(tf.GetChild(i).gameObject);
        }
    }
    /// <summary>
    /// 删除所有子物体
    /// </summary>
    /// <param name="tf"></param>
    public static void RemoveChild(GameObject obj)
    {
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            GameObject.Destroy(obj.transform.GetChild(i).gameObject);
        }
    }
    /// <summary>
    /// 删除所有显示的子物体
    /// </summary>
    /// <param name="tf"></param>
    public static void RemoveChildsByActive(Transform tf)
    {
        for (int i = 0; i < tf.childCount; i++)
        {
            if (tf.GetChild(i).gameObject.activeSelf)
            {
                GameObject.Destroy(tf.GetChild(i).gameObject);
            }
        }
    }
    /// <summary>
    /// 删除所有显示的子物体
    /// </summary>
    /// <param name="tf"></param>
    public static void RemoveChildsByActive(GameObject obj)
    {
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            if (obj.transform.GetChild(i).gameObject.activeSelf)
            {
                GameObject.Destroy(obj.transform.GetChild(i).gameObject);
            }
        }
    }

    /// <summary>
    /// 根据名字删除所有显示的子物体
    /// </summary>
    /// <param name="tf"></param>
    /// <param name="name"></param>
    /// <param name="activeSelf"></param>
    public static void RemoveChildsByName(Transform tf,string name,bool activeSelf)
    {
        for (int i = 0; i < tf.childCount; i++)
        {
            if (tf.GetChild(i).gameObject.activeSelf == activeSelf&& tf.GetChild(i).gameObject.name.Contains(name))
            {
                GameObject.Destroy(tf.GetChild(i).gameObject);
            }
        }
    }

    /// <summary>
    /// 通过名字获取子列表的控件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static T GetCptInChildrenByName<T>(GameObject obj,string name) where T : Component
    {
       T[] cptList= obj.GetComponentsInChildren<T>();
        foreach (T item in cptList)
        {
            if (item.name.Equals(name))
            {
                return item;
            }
        }
        return null;
    }


    /// <summary>
    /// 通过名字获取 所有子列表的控件
    /// </summary>
    /// <param name="tf"></param>
    public static List<T> GetAllCptInChildrenByName<T>(GameObject obj, string name) where T : Component
    {
        //for (int i = 0; i < obj.transform.childCount; i++)
        //{
        //    GameObject itemObj = obj.transform.GetChild(i).gameObject;
        //    if (name.Equals(itemObj.name))
        //    {
        //        return itemObj.GetComponent<T>();
        //    } 
        //}
        List<T> listCpt = new List<T>();
        T[] cptList = obj.GetComponentsInChildren<T>();
        foreach (T item in cptList)
        {
            if (item.name.Equals(name))
            {
                listCpt.Add(item);
            }
        }
        return listCpt;
    }

    /// <summary>
    /// 通过包含该名字获取 所有子列表的控件
    /// </summary>
    /// <param name="tf"></param>
    public static List<T> GetAllCptInChildrenByContainName<T>(GameObject obj, string name) where T : Component
    {
        //for (int i = 0; i < obj.transform.childCount; i++)
        //{
        //    GameObject itemObj = obj.transform.GetChild(i).gameObject;
        //    if (name.Equals(itemObj.name))
        //    {
        //        return itemObj.GetComponent<T>();
        //    } 
        //}
        List<T> listCpt = new List<T>();
        T[] cptList = obj.GetComponentsInChildren<T>();
        foreach (T item in cptList)
        {
            if (item.name.Contains(name))
            {
                listCpt.Add(item);
            }
        }
        return listCpt;
    }
}