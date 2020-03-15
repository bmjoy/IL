﻿using UnityEngine;
using UnityEditor;

public class BaseSceneInit : BaseMonoBehaviour
{

    protected UIGameManager uiGameManager;
    protected GameDataManager gameDataManager;
    protected GameItemsManager gameItemsManager;
    protected NpcInfoManager npcInfoManager;
    protected NpcTeamManager npcTeamManager;
    protected DialogManager dialogManager;
    protected StoryInfoManager storyInfoManager;

    protected WeatherHandler weatherHandler;
    protected ControlHandler controlHandler;


    public virtual void Awake()
    {
        npcTeamManager = Find<NpcTeamManager>(ImportantTypeEnum.NpcManager);
        weatherHandler = Find<WeatherHandler>( ImportantTypeEnum.WeatherHandler);
        gameItemsManager = Find<GameItemsManager>(ImportantTypeEnum.GameItemsManager);
        uiGameManager = Find<UIGameManager>(ImportantTypeEnum.GameUI);
        gameDataManager = Find<GameDataManager>(ImportantTypeEnum.GameDataManager);
        npcInfoManager = Find<NpcInfoManager>(ImportantTypeEnum.NpcManager);
        dialogManager = Find<DialogManager>(ImportantTypeEnum.DialogManager);
        storyInfoManager = Find<StoryInfoManager>(ImportantTypeEnum.StoreInfoManager);
        controlHandler = Find<ControlHandler>(ImportantTypeEnum.ControlHandler);
    }

    public void Start()
    {
        if (gameDataManager != null)
        {
            if (GameCommonInfo.GameData != null)
            {
                gameDataManager.gameData = GameCommonInfo.GameData;
            }
            else
            {
                gameDataManager.GetGameDataByUserId(GameCommonInfo.GameUserId);
            }
        }
    }

}