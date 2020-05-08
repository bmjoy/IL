﻿using UnityEngine;
using UnityEditor;

public abstract class BaseNormalSceneInit : BaseSceneInit,IBaseObserver, DialogView.IDialogCallBack
{

    public override void Start()
    {
        base.Start();
        //获取相关数据
        if (gameItemsManager != null)
            gameItemsManager.itemsInfoController.GetAllItemsInfo();
        if (npcInfoManager != null)
            npcInfoManager.npcInfoController.GetAllNpcInfo();
        if (storyInfoManager != null)
            storyInfoManager.storyInfoController.GetStoryInfoByScene(ScenesEnum.GameTownScene);

        //设置时间
        if (gameTimeHandler != null && gameDataManager != null)
        {
            TimeBean timeData = gameDataManager.gameData.gameTime;
            gameTimeHandler.SetTime(timeData.hour, timeData.minute);
            gameTimeHandler.SetTimeStatus(false);
            //增加回调
            gameTimeHandler.AddObserver(this);
        }

        //设置天气
        if (weatherHandler != null)
        {
            weatherHandler.SetWeahter(GameCommonInfo.CurrentDayData.weatherToday);
        }
        //设置角色位置
        InitUserPosition();
    }

    /// <summary>
    /// 结束一天
    /// </summary>
    public virtual void EndDay()
    {
        //停止时间
        if (gameTimeHandler != null)
            gameTimeHandler.SetTimeStatus(true);
        //停止控制
        if (controlHandler != null)
            controlHandler.StopControl();
        //重置游戏时间
        GameCommonInfo.GameData.gameTime.hour = 0;
        GameCommonInfo.GameData.gameTime.minute = 0;

        if (dialogManager != null)
        {
            DialogBean dialogBean = new DialogBean();
            dialogBean.content = GameCommonInfo.GetUITextById(3006);
            dialogManager.CreateDialog(DialogEnum.Text, this, dialogBean, 5);
        }
        else
        {
            SceneUtil.SceneChange(ScenesEnum.GameInnScene);
        }
    }

    /// <summary>
    /// 初始化角色位置
    /// </summary>
    public virtual ControlForMoveCpt InitUserPosition() {
        //开始角色控制
        ControlForMoveCpt moveControl = (ControlForMoveCpt)controlHandler.StartControl(ControlHandler.ControlEnum.Normal);
        return moveControl;
    }


    #region  时间通知回调
    public void ObserbableUpdate<T>(T observable, int type, params System.Object[] obj) where T : UnityEngine.Object
    {
        if (observable == gameTimeHandler)
        {
            if (type == (int)GameTimeHandler.NotifyTypeEnum.EndDay)
            {
                EndDay();
            }
        }
    }
    #endregion

    #region  弹窗通知回调
    public void Submit(DialogView dialogView, DialogBean dialogData)
    {
        SceneUtil.SceneChange(ScenesEnum.GameInnScene);
    }

    public void Cancel(DialogView dialogView, DialogBean dialogData)
    {

    }
    #endregion

}