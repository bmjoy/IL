﻿
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CheckUtil {


    /// <summary>
    /// 检测 string是否为null
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static bool StringIsNull(string str)
    {
        if (str == null || str.Length == 0)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 检测 list是否为null
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static bool ListIsNull<T>(List<T> list)
    {
        if (list == null || list.Count == 0)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 检测Array是否为Null
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <returns></returns>
    public static bool ArrayIsNull<T>(T[] array)
    {
        if (array == null || array.Length == 0)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 检测是否是数字
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public static bool CheckIsNumber(string number)
    {
        int temp;
        return int.TryParse(number, out temp);
    }

    /// <summary>
    /// 判断路径是否有效
    /// </summary>
    /// <param name="startPosition"></param>
    /// <param name="endPosition"></param>
    /// <returns></returns>
    public static bool CheckPath(Vector3 startPosition,Vector3 endPosition)
    {
        
        NavMeshPath navpath = new NavMeshPath();
        NavMesh.CalculatePath(startPosition, endPosition, -1, navpath);
        if (navpath.status == NavMeshPathStatus.PathPartial || navpath.status == NavMeshPathStatus.PathInvalid)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

}