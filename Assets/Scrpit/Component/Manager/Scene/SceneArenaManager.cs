﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class SceneArenaManager : BaseManager
{
    public Transform arena_1_CombatPosition;
    public Transform arena_1_EjectorPosition_1;
    public Transform arena_1_EjectorPosition_2;
    public Transform arena_1_EjectorPosition_3;
    public Transform arena_1_EjectorPosition_4;
    public Transform arena_1_EjectorPosition_5;
    public Transform arena_1_EjectorPosition_6;
    public Transform arena_1_BarrageUserPosition;

    public Transform arena_2_PlayerStartPosition;
    public Transform arena_2_AuditorStartPosition;
    public Transform arena_2_CompereStartPosition_1;
    public Transform arena_2_CompereStartPosition_2;
    /// <summary>
    /// 获取竞技场1的战斗地点
    /// </summary>
    /// <param name="vector3"></param>
    public void GetArenaForCombatBy1(out Vector3 vector3)
    {
        vector3 = arena_1_CombatPosition.position;
    }

    /// <summary>
    /// 获取竞技场1的发射台位置
    /// </summary>
    /// <returns></returns>
    public List<Vector3> GetArenaForBarrageEjectorBy1(int number)
    {
        List<Vector3> listEjectorPosition = new List<Vector3>();
        if (number == 1)
        {
            listEjectorPosition.Add(arena_1_EjectorPosition_1.position);
        }
        else if (number == 2)
        {
            listEjectorPosition.Add(arena_1_EjectorPosition_2.position);
            listEjectorPosition.Add(arena_1_EjectorPosition_3.position);
        }
        else if (number == 3)
        {
            listEjectorPosition.Add(arena_1_EjectorPosition_4.position);
            listEjectorPosition.Add(arena_1_EjectorPosition_5.position);
            listEjectorPosition.Add(arena_1_EjectorPosition_6.position);
        }
        return listEjectorPosition;
    }

    /// <summary>
    /// 获取竞技场1弹幕游戏用户起始位置
    /// </summary>
    /// <returns></returns>
    public void GetArenaForBarrageUserPositionBy1(out Vector3 vector3)
    {
        vector3 = arena_1_BarrageUserPosition.position;
    }

    /// <summary>
    /// 获取竞技场2的烹饪游戏玩家起始位置
    /// </summary>
    /// <param name="vector3"></param>
    public void GetArenaForCookingPlayerPositionBy2(out Vector3 vector3)
    {
        vector3 = arena_2_PlayerStartPosition.position;
    }

    /// <summary>
    ///  获取竞技场2的烹饪游戏评审员起始位置
    /// </summary>
    /// <param name="auditorStartPosition"></param>
    public void GetArenaForCookingAuditorPositionBy2(out Vector3 auditorStartPosition)
    {
        auditorStartPosition = arena_2_AuditorStartPosition.position;
    }

    /// <summary>
    ///  获取竞技场2的烹饪游戏主持人起始位置
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public List<Vector3> GetArenaForCookingComperePositionBy2(int number)
    {
        List<Vector3> listPosition = new List<Vector3>();
        if (number == 1)
        {
            listPosition.Add(arena_2_CompereStartPosition_1.position);
        }
        else if (number == 2)
        {
            listPosition.Add(arena_2_CompereStartPosition_1.position);
            listPosition.Add(arena_2_CompereStartPosition_2.position);
        }
        return listPosition;
    }
}