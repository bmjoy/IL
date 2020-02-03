﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class NpcAICustomerForGuestTeamCpt : NpcAICustomerCpt
{
    //预备点菜
    public MenuOwnBean menuOwn;
    //团队ID
    public string teamId;
    //集合点
    public Vector3 togetherPosition;

    protected NpcEventBuilder npcEventBuilder;

    public enum CustomerIntentForGuestTeamEnum
    {
        Together,
        WaitTeam,
        GoToTeam,
    }

    public CustomerIntentForGuestTeamEnum guestTeamIntent = CustomerIntentForGuestTeamEnum.Together;

    public override void Awake()
    {
        base.Awake();
        npcEventBuilder = Find<NpcEventBuilder>(ImportantTypeEnum.NpcBuilder);
    }

    /// <summary>
    /// 设置指定食物
    /// </summary>
    /// <param name="menuOwn"></param>
    public void SetMenu(MenuOwnBean menuOwn)
    {
        this.menuOwn = menuOwn;
    }

    /// <summary>
    /// 设置队伍ID
    /// </summary>
    /// <param name="teamId"></param>
    public void SetTeamId(string teamId)
    {
        this.teamId = teamId;
    }

    /// <summary>
    /// 离开处理
    /// </summary>
    public override void HandleForLeave()
    {
        if (!characterMoveCpt.IsAutoMoveStop())
            return;
        switch (guestTeamIntent)
        {
            case CustomerIntentForGuestTeamEnum.GoToTeam:
                //一起离开处理
                guestTeamIntent = CustomerIntentForGuestTeamEnum.WaitTeam;
                List<NpcAICustomerForGuestTeamCpt> listTeamMember = npcEventBuilder.GetGuestTeamByTeamId(teamId);
                bool allReady = true;
                foreach (NpcAICustomerForGuestTeamCpt teamMember in listTeamMember)
                {
                    if (teamMember.guestTeamIntent != CustomerIntentForGuestTeamEnum.WaitTeam)
                    {
                        allReady = false;
                    }
                }
                if (allReady)
                {
                    Vector3 leavePostion = sceneInnManager.GetRandomSceneExportPosition();
                    foreach (NpcAICustomerForGuestTeamCpt teamMember in listTeamMember)
                    {
                        teamMember.guestTeamIntent = CustomerIntentForGuestTeamEnum.Together;
                        teamMember.characterMoveCpt.SetDestination(leavePostion);
                    }
                }

                break;
            case CustomerIntentForGuestTeamEnum.Together:
                Destroy(gameObject);
                break;
        }
    }

    /// <summary>
    /// 点餐
    /// </summary>
    public override void HandleForOrderFood()
    {
        if (!characterMoveCpt.IsAutoMoveStop())
            return;
        //首先调整修改朝向
        SetCharacterFace(orderForCustomer.table.GetUserFace());
        //点餐
        innHandler.OrderForFood(orderForCustomer, menuOwn);
        if (orderForCustomer.foodData != null)
        {
            //喊出需要的菜品
            characterShoutCpt.Shout(orderForCustomer.foodData.name);
        }
        //判断是否出售
        if (!menuOwn.isSell)
        {
            //如果菜品没有出售 心情直接降100 
            ChangeMood(-100);
            //离开
            SetIntent(CustomerIntentEnum.Leave);
            return;
        }
        //如果没有这菜
        if (orderForCustomer.foodData == null)
        {
            //如果没有菜品出售 心情直接降100 
            ChangeMood(-100);
            //离开
            SetIntent(CustomerIntentEnum.Leave);
        }
        else
        {
            //设置等待食物
            SetIntent(CustomerIntentEnum.WaitFood);
        }
    }

    /// <summary>
    /// 意图-想要就餐
    /// </summary>
    public override void IntentForWant()
    {
        base.IntentForWant();
        //通知其他团队成员
        List<NpcAICustomerForGuestTeamCpt> listTeamMember = npcEventBuilder.GetGuestTeamByTeamId(teamId);
        foreach (NpcAICustomerForGuestTeamCpt teamMember in listTeamMember)
        {
            if (teamMember != this && teamMember.customerIntent != CustomerIntentEnum.Want)
            {
                teamMember.SetIntent(CustomerIntentEnum.Want);
                //统一入口
                teamMember.movePosition = movePosition;
                teamMember.characterMoveCpt.SetDestination(movePosition);
            }
        }
    }

    /// <summary>
    /// 意图-离开
    /// </summary>
    public override void IntentForLeave()
    {
        //如果还没有生成订单
        if (orderForCustomer == null)
        {
            characterMoveCpt.SetDestination(movePosition);
            //通知其他团队成员
            List<NpcAICustomerForGuestTeamCpt> listTeamMember = npcEventBuilder.GetGuestTeamByTeamId(teamId);
            foreach (NpcAICustomerForGuestTeamCpt teamMember in listTeamMember)
            {
                if (teamMember != this && teamMember.customerIntent != CustomerIntentEnum.Leave)
                {
                    teamMember.SetIntent(CustomerIntentEnum.Leave);
                }
            }
            return;
        }
        //如果在订单列表 则移除订单列表
        if (innHandler.orderList.Contains(orderForCustomer))
        {
            //根据心情评价客栈 前提订单里有他
            innHandler.InnPraise(innEvaluation.GetPraise());
            //移除订单列表
            innHandler.orderList.Remove(orderForCustomer);
        }
        //随机获取一个退出点
        togetherPosition = innHandler.GetRandomEntrancePosition();
        guestTeamIntent = CustomerIntentForGuestTeamEnum.GoToTeam;
        togetherPosition += new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        characterMoveCpt.SetDestination(togetherPosition);
    }

    /// <summary>
    /// 意图-等待招待过来
    /// </summary>
    public override void IntentForWaitAccost()
    {
        base.IntentForWaitAccost();
        //通知其他团队成员
        List<NpcAICustomerForGuestTeamCpt> listTeamMember = npcEventBuilder.GetGuestTeamByTeamId(teamId);
        foreach (NpcAICustomerForGuestTeamCpt teamMember in listTeamMember)
        {
            if (teamMember != this && teamMember.customerIntent!= CustomerIntentEnum.WaitAccost)
            {
                teamMember.SetIntent(CustomerIntentEnum.WaitAccost);
            }
        }
    }

    /// <summary>
    ///  改变心情
    /// </summary>
    /// <param name="mood"></param>
    /// <param name="isNotice"> 是否通知其他其他成员</param>
    public void ChangeMood(float mood, bool isNotice)
    {
        base.ChangeMood(mood);
        //通知其他团队成员
        if (innEvaluation.mood <= 0 && isNotice)
        {
            List<NpcAICustomerForGuestTeamCpt> listTeamMember = npcEventBuilder.GetGuestTeamByTeamId(teamId);
            foreach (NpcAICustomerForGuestTeamCpt teamMember in listTeamMember)
            {
                if (teamMember != this)
                {
                    teamMember.ChangeMood(-100, false);
                    teamMember.SetIntent(CustomerIntentEnum.Leave);
                }
            }
        }
    }

    public override void ChangeMood(float mood)
    {
        ChangeMood(mood, true);
    }

    /// <summary>
    /// 意图-排队就餐
    /// </summary>
    public override void IntentForWaitSeat()
    {
        OrderForCustomer orderForCustomer = innHandler.CreateOrder(this);
        innHandler.cusomerQueue.Add(orderForCustomer);
    }
}