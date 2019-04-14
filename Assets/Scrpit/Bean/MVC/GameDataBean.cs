﻿using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[Serializable]
public class GameDataBean
{
    public string userId;//用户ID
    public long moneyS;//1黄金=10白银  1白银=1000文
    public long moneyM;
    public long moneyL;

    public string innName;//客栈名称
    public CharacterBean userCharacter;// 老板
    public List<CharacterBean> staffCharacterList;//员工
    public InnBuildBean innBuildData;//客栈建筑数据
    public TimeBean gameTime;//游戏时间

    public List<ItemBean> buildItemList = new List<ItemBean>();//所拥有的建筑材料
    public List<ItemBean> equipItemList = new List<ItemBean>();//所拥有的装备


    public static void GetMoneyDetails(long money, out long L, out long M, out long S)
    {
        long temp1 = money % 10;
        long temp2 = money % 100 / 10;
        long temp3 = money % 1000 / 100;
        long temp4 = money % 10000 / 1000;
        S = temp3 * 100 + temp2 * 10 + temp1;
        M = temp4;
        L = money / 10000;
    }

}