﻿using UnityEngine;
using UnityEditor;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections.Generic;

public class UITownArena : UIBaseOne, IRadioGroupCallBack, StoreInfoManager.ICallBack
{
    public ScrollRect scrollRectContainer;
    public GameObject objArenaContainer;
    public GameObject objArenaModel;

    public RadioGroupView rgType;
    private List<StoreInfoBean> listArenaInfo;
    public Text tvNull;
    public override void Start()
    {
        base.Start();
        if (rgType != null)
        {
            rgType.SetCallBack(this);
        }
    }

    public override void OpenUI()
    {
        base.OpenUI();
        rgType.SetPosition(0, false);

        uiGameManager.storeInfoManager.SetCallBack(this);
        uiGameManager.storeInfoManager.GetStoreInfoForArenaInfo();
    }

    /// <summary>
    /// 初始化游戏数据
    /// </summary>
    /// <param name="type"></param>
    public void InitData(TrophyTypeEnum type)
    {
        CptUtil.RemoveChildsByActive(objArenaContainer);
        List<MiniGameBaseBean> listMiniGameData = GameCommonInfo.DailyLimitData.GetArenaDataByType(type);
        if (listMiniGameData == null)
        {
            listMiniGameData = CreateMiniGameData(type);
            GameCommonInfo.DailyLimitData.AddArenaDataByType(type, listMiniGameData);
        }
        bool hasData = false;
        for (int i = 0; i < listMiniGameData.Count; i++)
        {
            MiniGameBaseBean itemMiniGameData = listMiniGameData[i];
            GameObject objItem = Instantiate(objArenaContainer, objArenaModel);
            ItemTownArenaCpt arenaItem = objItem.GetComponent<ItemTownArenaCpt>();
            arenaItem.SetData(type, itemMiniGameData);
            //GameUtil.RefreshRectViewHight((RectTransform)objItem.transform, true);
            objItem.transform.DOScale(new Vector3(0, 0, 0), 0.5f).From().SetEase(Ease.OutBack);
            hasData = true;
        }
        if (hasData)
            tvNull.gameObject.SetActive(false);
        else
            tvNull.gameObject.SetActive(true);
        GameUtil.RefreshRectViewHight((RectTransform)objArenaContainer.transform, true);
        scrollRectContainer.content.localPosition = Vector2.zero;
    }

    /// <summary>
    /// 创建迷你游戏数据
    /// </summary>
    /// <returns></returns>
    private List<MiniGameBaseBean> CreateMiniGameData(TrophyTypeEnum type)
    {
        List<MiniGameBaseBean> listMiniGameData = new List<MiniGameBaseBean>();
        int arenaNumber = Random.Range(1, 10);
        for (int i = 0; i < arenaNumber; i++)
        {
            MiniGameEnum gameType = RandomUtil.GetRandomEnum<MiniGameEnum>();
            StoreInfoBean storeInfo = null;

            MiniGameBaseBean miniGameData = MiniGameEnumTools.GetMiniGameData(gameType);
            miniGameData.gameReason = MiniGameReasonEnum.Fight;
            switch (gameType)
            {
                case MiniGameEnum.Cooking:
                    storeInfo = GetStoreInfoByTypeAndWorker(type, WorkerEnum.Chef);
                    miniGameData = CreateCookingGameData((MiniGameCookingBean)miniGameData, storeInfo, type);
                    break;
                case MiniGameEnum.Barrage:
                    storeInfo = GetStoreInfoByTypeAndWorker(type, WorkerEnum.Waiter);
                    miniGameData = CreateBarrageGameData((MiniGameBarrageBean)miniGameData, storeInfo, type);
                    break;
                case MiniGameEnum.Account:
                    storeInfo = GetStoreInfoByTypeAndWorker(type, WorkerEnum.Accountant);
                    miniGameData = CreateAccountGameData((MiniGameAccountBean)miniGameData, storeInfo, type);
                    break;
                case MiniGameEnum.Debate:
                    storeInfo = GetStoreInfoByTypeAndWorker(type, WorkerEnum.Accost);
                    miniGameData = CreateDebateGameData((MiniGameDebateBean)miniGameData, storeInfo, type);
                    break;
                case MiniGameEnum.Combat:
                    storeInfo = GetStoreInfoByTypeAndWorker(type, WorkerEnum.Beater);
                    miniGameData = CreateCombatGameData((MiniGameCombatBean)miniGameData, storeInfo, type);
                    break;
            }
            PreTypeForMiniGameEnumTools.GetMiniGameData(miniGameData, storeInfo.pre_data_minigame, uiGameManager.gameItemsManager, uiGameManager.npcInfoManager);
            //奖励添加
            miniGameData.listReward = new List<RewardTypeBean>();
            List<RewardTypeBean> listReward = RewardTypeEnumTools.GetListRewardData(storeInfo.reward_data);
            //固定奖励
            List<RewardTypeBean> listFixedReward = new List<RewardTypeBean>();
            //随机奖励
            List<RewardTypeBean> listRandomReward = new List<RewardTypeBean>();
            foreach (RewardTypeBean itemReward in listReward)
            {
                switch (itemReward.GetRewardType())
                {
                    case RewardTypeEnum.AddMoneyL:
                    case RewardTypeEnum.AddMoneyM:
                    case RewardTypeEnum.AddMoneyS:
                    case RewardTypeEnum.AddArenaTrophyElementary:
                    case RewardTypeEnum.AddArenaTrophyIntermediate:
                    case RewardTypeEnum.AddArenaTrophyAdvanced:
                    case RewardTypeEnum.AddArenaTrophyLegendary:
                        listFixedReward.Add(itemReward);
                        break;
                    default:
                        listRandomReward.Add(itemReward);
                        break;
                }
            }
            miniGameData.listReward.AddRange(listFixedReward);
            if (!CheckUtil.ListIsNull(listRandomReward))
            {
                RewardTypeBean randomReward = RandomUtil.GetRandomDataByList(listRandomReward);
                miniGameData.listReward.Add(randomReward);
            }

            //添加对应的奖杯
            switch (type)
            {
                case TrophyTypeEnum.Elementary:
                    break;
                case TrophyTypeEnum.Intermediate:
                    break;
                case TrophyTypeEnum.Advanced:
                    break;
                case TrophyTypeEnum.Legendary:
                    break;
            }
            //设置游戏时间
            if (storeInfo.mark != null)
                miniGameData.preGameTime = int.Parse(storeInfo.mark);
            //设置前置金钱
            miniGameData.preMoneyL = storeInfo.price_l;
            miniGameData.preMoneyM = storeInfo.price_m;
            miniGameData.preMoneyS = storeInfo.price_s;
            //添加游戏数据
            listMiniGameData.Add(miniGameData);
        }
        return listMiniGameData;
    }

    /// <summary>
    /// 根据职业和类型获取竞技场数据
    /// </summary>
    /// <param name="type"></param>
    /// <param name="workerEnum"></param>
    /// <returns></returns>
    private StoreInfoBean GetStoreInfoByTypeAndWorker(TrophyTypeEnum type, WorkerEnum workerEnum)
    {
        foreach (StoreInfoBean storeInfo in listArenaInfo)
        {
            if (storeInfo.mark_type == (int)type && EnumUtil.GetEnum<WorkerEnum>(storeInfo.pre_data) == workerEnum)
            {
                return storeInfo;
            }
        }
        return null;
    }

    /// <summary>
    /// 创建烹饪游戏数据
    /// </summary>
    /// <param name="miniGameData"></param>
    /// <param name="storeInfo"></param>
    /// <returns></returns>
    private MiniGameCookingBean CreateCookingGameData(MiniGameCookingBean miniGameData, StoreInfoBean storeInfo, TrophyTypeEnum type)
    {
        switch (type)
        {
            case TrophyTypeEnum.Elementary:
                break;
            case TrophyTypeEnum.Intermediate:
                break;
            case TrophyTypeEnum.Advanced:
                break;
            case TrophyTypeEnum.Legendary:
                break;
        }
        //随机生成敌人
        List<CharacterBean> listEnemyData = new List<CharacterBean>();
        for (int i = 0; i < 15; i++)
        {
            CharacterBean randomEnemy = CharacterBean.CreateRandomWorkerData(uiGameManager.characterBodyManager);
            listEnemyData.Add(randomEnemy);
        }
        //评审人员
        List<CharacterBean> listTownNpc = uiGameManager.npcInfoManager.GetCharacterDataByType(NpcTypeEnum.Town);
        PreTypeForMiniGameEnumTools.GetAllCharacter(storeInfo.pre_data_minigame, out string allCharacterIds);
        for (int i = 0; i < listTownNpc.Count; i++)
        {
            CharacterBean itemNpc = listTownNpc[i];
            if (allCharacterIds.Contains(itemNpc.baseInfo.characterId))
            {
                listTownNpc.Remove(itemNpc);
                i--;
            }
        }
        List<CharacterBean> listAuditerData = RandomUtil.GetRandomDataByListForNumberNR(listTownNpc, 5);
        miniGameData.InitData(uiGameManager.gameItemsManager, null, listEnemyData, listAuditerData, null);
        return miniGameData;
    }

    /// <summary>
    /// 创建弹幕游戏数据
    /// </summary>
    /// <param name="miniGameData"></param>
    /// <param name="storeInfo"></param>
    /// <returns></returns>
    private MiniGameBarrageBean CreateBarrageGameData(MiniGameBarrageBean miniGameData, StoreInfoBean storeInfo, TrophyTypeEnum type)
    {
        switch (type)
        {
            case TrophyTypeEnum.Elementary:
                break;
            case TrophyTypeEnum.Intermediate:
                break;
            case TrophyTypeEnum.Advanced:
                break;
            case TrophyTypeEnum.Legendary:
                break;
        }
        return miniGameData;
    }

    /// <summary>
    /// 创建算账游戏数据
    /// </summary>
    /// <param name="miniGameData"></param>
    /// <param name="storeInfo"></param>
    /// <returns></returns>
    private MiniGameAccountBean CreateAccountGameData(MiniGameAccountBean miniGameData, StoreInfoBean storeInfo, TrophyTypeEnum type)
    {
        switch (type)
        {
            case TrophyTypeEnum.Elementary:
                break;
            case TrophyTypeEnum.Intermediate:
                break;
            case TrophyTypeEnum.Advanced:
                break;
            case TrophyTypeEnum.Legendary:
                break;
        }
        return miniGameData;
    }

    /// <summary>
    /// 创建斗魅游戏数据
    /// </summary>
    /// <param name="miniGameData"></param>
    /// <param name="storeInfo"></param>
    /// <returns></returns>
    private MiniGameDebateBean CreateDebateGameData(MiniGameDebateBean miniGameData, StoreInfoBean storeInfo, TrophyTypeEnum type)
    {
        CharacterBean randomEnemy = null;
        switch (type)
        {
            case TrophyTypeEnum.Elementary:
                randomEnemy = CharacterBean.CreateRandomEnemyData(uiGameManager.characterBodyManager, 10);
                break;
            case TrophyTypeEnum.Intermediate:
                break;
            case TrophyTypeEnum.Advanced:
                break;
            case TrophyTypeEnum.Legendary:
                break;
        }
        miniGameData.InitData(uiGameManager.gameItemsManager, null, randomEnemy);
        return miniGameData;
    }

    /// <summary>
    /// 创建战斗游戏数据
    /// </summary>
    /// <param name="miniGameData"></param>
    /// <param name="storeInfo"></param>
    /// <returns></returns>
    private MiniGameCombatBean CreateCombatGameData(MiniGameCombatBean miniGameData, StoreInfoBean storeInfo, TrophyTypeEnum type)
    {
        int enemyBaseAttribute = 10;
        switch (type)
        {
            case TrophyTypeEnum.Elementary:
                enemyBaseAttribute = 10;
                break;
            case TrophyTypeEnum.Intermediate:
                break;
            case TrophyTypeEnum.Advanced:
                break;
            case TrophyTypeEnum.Legendary:
                break;
        }
        miniGameData.winBringDownNumber = Random.Range(1, 6);
        //生成敌人
        List<CharacterBean> listEnemyData = new List<CharacterBean>();
        for (int i = 0; i < miniGameData.winBringDownNumber; i++)
        {
            CharacterBean enemyData = CharacterBean.CreateRandomEnemyData(uiGameManager.characterBodyManager, enemyBaseAttribute);
            listEnemyData.Add(enemyData);
        }
        miniGameData.InitData(uiGameManager.gameItemsManager, new List<CharacterBean>(), listEnemyData);
        return miniGameData;
    }


    #region 等级选择回调
    public void RadioButtonSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {
        TrophyTypeEnum trophyType = EnumUtil.GetEnum<TrophyTypeEnum>(rbview.name);
        InitData(trophyType);
    }

    public void RadioButtonUnSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {

    }
    #endregion

    #region 数据回调
    public void GetStoreInfoSuccess(StoreTypeEnum type, List<StoreInfoBean> listData)
    {
        listArenaInfo = listData;
        InitData(TrophyTypeEnum.Elementary);
    }
    #endregion
}