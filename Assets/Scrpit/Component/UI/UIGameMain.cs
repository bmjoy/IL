﻿using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class UIGameMain : BaseUIComponent
{
    public Button btWorker;
    public Button btBuild;
    public Button btMenu;
    public Button btSave;
    public Button btSleep;

    public Text tvInnStatus;
    public Text tvMoneyS;
    public Text tvMoneyM;
    public Text tvMoneyL;

    public GameDataManager gameDataManager;
    public InnHandler innHandler;
    public InnWallBuilder innWall;

    private void Update()
    {
        if (tvInnStatus != null)
            if (innHandler.innStatus == InnHandler.InnStatusEnum.Close)
            {
                tvInnStatus.text = GameCommonInfo.GetUITextById(2002);
            }
            else
            {
                tvInnStatus.text = GameCommonInfo.GetUITextById(2001);
            }
        if (tvMoneyS != null)
            tvMoneyS.text = gameDataManager.gameData.moneyS + "";
        if (tvMoneyM != null)
            tvMoneyM.text = gameDataManager.gameData.moneyM + "";
        if (tvMoneyL != null)
            tvMoneyL.text = gameDataManager.gameData.moneyL + "";

    }

    public void Start()
    {
        if (btWorker != null)
            btWorker.onClick.AddListener(OpenWorkerUI);

        if (btBuild != null)
            btBuild.onClick.AddListener(OpenBuildUI);

        if (btMenu != null)
            btMenu.onClick.AddListener(OpenMenuUI);

        if (btSave != null)
            btSave.onClick.AddListener(SaveData);

        if (btSleep != null)
            btSleep.onClick.AddListener(EndDay);
    }

    public void SaveData()
    {
        gameDataManager.SaveGameData();
    }

    public void OpenBuildUI()
    {
        uiManager.OpenUIAndCloseOtherByName("Build");
    }

    public void OpenWorkerUI()
    {
        uiManager.OpenUIAndCloseOtherByName("Worker");
    }

    public void OpenMenuUI()
    {
        uiManager.OpenUIAndCloseOtherByName("Menu");
    }

    public void EndDay()
    {
        uiManager.OpenUIAndCloseOtherByName("Start");
        innHandler.CloseInn();
    }
}