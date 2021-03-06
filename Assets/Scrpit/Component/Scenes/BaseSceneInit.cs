﻿using UnityEngine;
using UnityEditor;

public class BaseSceneInit : BaseMonoBehaviour
{
    protected InnBuildManager innBuildManager;
    protected UIGameManager uiGameManager;
    protected GameDataManager gameDataManager;
    protected GameItemsManager gameItemsManager;
    protected NpcInfoManager npcInfoManager;
    protected NpcTeamManager npcTeamManager;
    protected DialogManager dialogManager;
    protected StoryInfoManager storyInfoManager;
    protected GameTimeHandler gameTimeHandler;

    protected WeatherHandler weatherHandler;
    protected ControlHandler controlHandler;
    protected AudioHandler audioHandler;


    public virtual void Awake()
    {
        npcTeamManager = Find<NpcTeamManager>(ImportantTypeEnum.NpcManager);
        weatherHandler = Find<WeatherHandler>( ImportantTypeEnum.WeatherHandler);
        gameItemsManager = Find<GameItemsManager>(ImportantTypeEnum.GameItemsManager);
        uiGameManager = Find<UIGameManager>(ImportantTypeEnum.GameUI);
        gameDataManager = Find<GameDataManager>(ImportantTypeEnum.GameDataManager);
        npcInfoManager = Find<NpcInfoManager>(ImportantTypeEnum.NpcManager);
        dialogManager = Find<DialogManager>(ImportantTypeEnum.DialogManager);
        storyInfoManager = Find<StoryInfoManager>(ImportantTypeEnum.StoryManager);
        controlHandler = Find<ControlHandler>(ImportantTypeEnum.ControlHandler);
        audioHandler = Find<AudioHandler>(ImportantTypeEnum.AudioHandler);
        gameTimeHandler = Find<GameTimeHandler>(ImportantTypeEnum.TimeHandler);
        innBuildManager = Find<InnBuildManager>(ImportantTypeEnum.BuildManager);
    }

    public virtual void Start()
    {
        if (gameDataManager != null)
        {
            if (GameCommonInfo.GameData==null|| CheckUtil.StringIsNull(GameCommonInfo.GameData.userId))
            {
                gameDataManager.GetGameDataByUserId(GameCommonInfo.GameUserId);
            }
            else
            {
                gameDataManager.gameData = GameCommonInfo.GameData;
            }
        }
        //获取相关数据
        if (gameItemsManager != null)
        {
            gameItemsManager.itemsInfoController.GetAllItemsInfo();
        }  
        if (npcInfoManager != null)
        {
            npcInfoManager.npcInfoController.GetAllNpcInfo();
        }  
        
        if (innBuildManager != null)
        {
            innBuildManager.buildDataController.GetAllBuildItemsData();
        }  
    }

}