﻿using UnityEngine;
using UnityEditor;

public class NpcAIWorkerCpt : BaseNpcAI
{

    public enum WorkerIntentEnum
    {
        Idle,//空闲
        Waiter,//跑堂
        Cook,//做菜
        Accounting,//结账
    }

    //厨师AI控制
    public NpcAIWorkerForChefCpt aiForChef;
    //跑堂AI控制
    public NpcAIWorkerForWaiterCpt aiForWaiter;
    //结账AI控制
    public NpcAIWorkerForAccountingCpt aiForAccounting;

    public WorkerIntentEnum workerIntent = WorkerIntentEnum.Idle;//工作者的想法

    //是否开启厨师
    public bool isChef;
    //是否开启服务员
    public bool isWaiter;
    //是否开启算账
    public bool isAccounting;

    private void Awake()
    {

    }

    /// <summary>
    /// 设置料理
    /// </summary>
    public void SetIntentForCook(BuildStoveCpt stoveCpt,MenuForCustomer foodData)
    {
        workerIntent = WorkerIntentEnum.Cook;
        aiForChef.SetCookData(stoveCpt, foodData);
    }

    /// <summary>
    /// 设置跑堂
    /// </summary>
    /// <param name="stoveCpt"></param>
    public void SetIntentForWaiterSend(FoodForCustomerCpt food)
    {
        workerIntent = WorkerIntentEnum.Waiter;
        aiForWaiter.SetFoodSend(food);
    }

    /// <summary>
    /// 设置跑堂
    /// </summary>
    /// <param name="stoveCpt"></param>
    public void SetIntentForWaiterClear(FoodForCustomerCpt food)
    {
        workerIntent = WorkerIntentEnum.Waiter;
        aiForWaiter.SetFoodClear(food);
    }

    /// <summary>
    /// 设置结账
    /// </summary>
    /// <param name="customerCpt"></param>
    public void SetIntentForAccounting(NpcAICustomerCpt customerCpt)
    {
        workerIntent = WorkerIntentEnum.Accounting;
        aiForAccounting.SetAccounting(customerCpt);
    }
}