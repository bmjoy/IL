﻿using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class UIGameBuild : BaseUIComponent
{
    //返回按钮
    public Button btBack;
    //类型按钮
    public Button btTypeTable;

    public GameObject listBuildContent;
    public GameObject itemBuildModel;

    //数据管理
    public GameDataManager gameDataManager;
    public InnBuildManager innBuildManager;

    //控制
    public ControlForBuildCpt controlForBuild;
    public ControlForMoveCpt controlForMove;

    //游戏进程处理
    public InnHandler innHandler;

    public void Start()
    {
        if (btBack != null)
            btBack.onClick.AddListener(OpenMainUI);
       gameDataManager.gameData.buildItemList.Add(new ItemBean(30001,3));
       CreateBuildList(BuildItemBean.BuildType.Table);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        controlForBuild.StartControl();
        controlForMove.EndControl();
    }

    public override void CloseUI()
    {
        base.CloseUI();
        controlForMove.StartControl();
        controlForBuild.EndControl();
        controlForBuild.DestoryBuild();
        if (innHandler != null)
            innHandler.InitInn();
    }

    /// <summary>
    /// 创建建筑列表
    /// </summary>
    /// <param name="type"></param>
    public void CreateBuildList(BuildItemBean.BuildType type)
    {
        if (listBuildContent == null)
            return;
        if (itemBuildModel == null)
            return;
        if (gameDataManager == null)
            return;
        if (gameDataManager.gameData == null)
            return;
        if (gameDataManager.gameData.buildItemList == null)
            return;
        CptUtil.RemoveChildsByActive(listBuildContent.transform);

        for (int i = 0; i < gameDataManager.gameData.buildItemList.Count; i++)
        {
            ItemBean itemData = gameDataManager.gameData.buildItemList[i];
            BuildItemBean buildData = innBuildManager.GetTableDataById(itemData.itemId);
            CreateBuildItem(itemData, buildData);
        }
    }

    /// <summary>
    /// 创建单个数据
    /// </summary>
    /// <param name="itemData"></param>
    /// <param name="buildData"></param>
    public void CreateBuildItem(ItemBean itemData, BuildItemBean buildData)
    {
        GameObject itemBuildObj = Instantiate(itemBuildModel, itemBuildModel.transform);
        itemBuildObj.SetActive(true);
        itemBuildObj.transform.SetParent(listBuildContent.transform);
        ItemGameBuildCpt itemCpt= itemBuildObj.GetComponent<ItemGameBuildCpt>();
        itemCpt.SetData(itemData, buildData);
    }

    public void CreateTableList()
    {
        CreateBuildList(BuildItemBean.BuildType.Table);
    }

    /// <summary>
    /// 返回游戏主UI
    /// </summary>
    public void OpenMainUI()
    {
        uiManager.OpenUIAndCloseOtherByName("Main");
    }
}