﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMonoBehaviour : MonoBehaviour {

    /// <summary>
    /// 实例化一个物体
    /// </summary>
    /// <param name="objContent"></param>
    /// <param name="objModel"></param>
    /// <returns></returns>
    public GameObject Instantiate(GameObject objContent,GameObject objModel)
    {
        GameObject objItem = Instantiate(objModel, objContent.transform);
        objItem.SetActive(true);
        return objItem;
    }

    /// <summary>
    /// 实例化一个物体
    /// </summary>
    /// <param name="objContent"></param>
    /// <param name="objModel"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public GameObject Instantiate(GameObject objContent, GameObject objModel,Vector3 position)
    {
        GameObject objItem = Instantiate(objModel, objContent.transform);
        objItem.SetActive(true);
        objItem.transform.position = position;
        return objItem;
    }

    /// <summary>
    /// 查找数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="tagType"></param>
    /// <returns></returns>
    //public T FindObjectOfType<T>(GameObjectTagEnum tagType)
    //{
    //    GameObject objsFind=  GameObject.FindGameObjectWithTag(EnumUtil.GetEnumName(tagType));
    //    return objsFind.GetComponent<T>();
    //}

    /// <summary>
    /// 查找数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="importantType"></param>
    /// <returns></returns>
    public T Find<T>(ImportantTypeEnum importantType)
    {
        GameObject objFind = GameObject.Find(EnumUtil.GetEnumName(importantType));
        if (objFind == null)
        {
            return default;
        }
        else
        {
            return objFind.GetComponent<T>();
        }
    }
}
