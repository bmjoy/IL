﻿using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public class NpcEventBuilder : NpcNormalBuilder, IBaseObserver
{
    //捣乱者模型
    public GameObject objRascalModel;

    private void Start()
    {
        gameTimeHandler.AddObserver(this);
    }

    /// <summary>
    /// 开始事件
    /// </summary>
    public void StartEvent()
    {
        //TODO 各种事件的完善
        //RascalEvent();
    }

    /// <summary>
    /// 团队事件
    /// </summary>
    public void TeamEvent()
    {
        Vector3 npcPosition = GetRandomStartPosition();
        List<CharacterBean> listTeam = npcInfoManager.GetCharacterDataByType(NPCTypeEnum.GuestTeam);
        CharacterBean randomCharacterData = RandomUtil.GetRandomDataByList(listTeam);
        BuildGuestTeam(randomCharacterData, npcPosition);
    }

    public void TeamEvent(long teamNpcId)
    {
        Vector3 npcPosition = GetRandomStartPosition();
        CharacterBean characterData = npcInfoManager.GetCharacterDataById(teamNpcId);
        BuildGuestTeam(characterData, npcPosition);
    }

    /// <summary>
    /// 恶棍事件
    /// </summary>
    public void RascalEvent()
    {
        Vector3 npcPosition = GetRandomStartPosition();
        List<CharacterBean> listData = npcInfoManager.GetCharacterDataByType(21);
        BuildRascal(RandomUtil.GetRandomDataByList(listData), npcPosition);
    }

    /// <summary>
    /// 创建恶棍
    /// </summary>
    /// <param name="characterData"></param>
    /// <param name="npcPosition"></param>
    public void BuildRascal(CharacterBean characterData, Vector3 npcPosition)
    {
        //如果大于构建上线则不再创建新NPC
        if (objContainer.transform.childCount > buildMaxNumber)
            return;
        //生成NPC
        GameObject npcObj = BuildNpc(objRascalModel, characterData, npcPosition);
        //设置意图
        NpcAIRascalCpt rascalCpt = npcObj.GetComponent<NpcAIRascalCpt>();
        CharacterFavorabilityBean characterFavorability = gameDataManager.gameData.GetCharacterFavorability(long.Parse(characterData.baseInfo.characterId));
        rascalCpt.SetFavorabilityData(characterFavorability);
        rascalCpt.StartEvil();
    }

    /// <summary>
    /// 创建团队顾客
    /// </summary>
    /// <param name="characterData"></param>
    /// <param name="npcPosition"></param>
    public void BuildGuestTeam(CharacterBean characterData, Vector3 npcPosition)
    {
        List<NpcShowConditionBean> listConditionData = NpcShowConditionTools.GetListConditionData(characterData.npcInfoData.condition);
        int npcNumber = 1;
        foreach (NpcShowConditionBean itemCondition in listConditionData)
        {
            NpcShowConditionTools.GetConditionDetails(itemCondition);
            switch (itemCondition.dataType)
            {
                case NpcShowConditionEnum.NpcNumber:
                    npcNumber = itemCondition.npcNumber;
                    break;
            }
        }
        for (int i = 0; i < npcNumber; i++)
        {
            //随机生成身体数据
            CharacterBodyBean.CreateRandomBodyByManager(characterData.body, characterBodyManager);
            GameObject npcObj = Instantiate(objContainer, objNormalModel);

            npcObj.transform.localScale = new Vector3(1, 1);
            npcObj.transform.position = npcPosition + new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));

            BaseNpcAI baseNpcAI = npcObj.GetComponent<BaseNpcAI>();
            baseNpcAI.SetCharacterData(characterData);

            NpcAICustomerCpt npcAICustomer = baseNpcAI.GetComponent<NpcAICustomerCpt>();
            npcAICustomer.SetIntent(NpcAICustomerCpt.CustomerIntentEnum.Walk);
        }
    }

    #region 时间回调通知
    public void ObserbableUpdate<T>(T observable, int type, params System.Object[] obj) where T : UnityEngine.Object
    {
        if ((GameTimeHandler.NotifyTypeEnum)type == GameTimeHandler.NotifyTypeEnum.NewDay)
        {
            ClearNpc();
        }
        else if ((GameTimeHandler.NotifyTypeEnum)type == GameTimeHandler.NotifyTypeEnum.EndDay)
        {
            ClearNpc();
        }
        else if ((GameTimeHandler.NotifyTypeEnum)type == GameTimeHandler.NotifyTypeEnum.TimePoint)
        {
            int hour = Convert.ToInt32(obj[0]);
            if (hour > 9 && hour <= 20)
            {
                StartEvent();
            }
        }
    }
    #endregion
}