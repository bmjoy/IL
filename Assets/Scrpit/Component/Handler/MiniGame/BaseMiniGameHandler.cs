﻿using UnityEngine;
using DG.Tweening;
using UnityEditor;
using System.Collections.Generic;

public class BaseMiniGameHandler<B,D> : BaseHandler, UIMiniGameCountDown.ICallBack, UIMiniGameEnd.ICallBack 
    where D : MiniGameBaseBean
    where B : BaseMiniGameBuilder
{
    public enum MiniGameStatusEnum
    {
        GamePre = 0,//游戏准备中
        Gameing = 1,//游戏进行中
        GameEnd = 2,//游戏结束
        GameClose = 3,//游戏关闭
    }

    //UI管理
    public UIGameManager uiGameManager;
    //控制器
    public ControlHandler controlHandler;
    //游戏构建器
    public B miniGameBuilder;
    //游戏数据
    public D miniGameData;

    //迷你游戏状态
    private MiniGameStatusEnum mMiniGameStatus = MiniGameStatusEnum.GamePre;

    /// <summary>
    /// 设置游戏状态
    /// </summary>
    /// <param name="status"></param>
    public void SetMiniGameStatus(MiniGameStatusEnum status)
    {
        mMiniGameStatus = status;
    }

    /// <summary>
    /// 获取游戏状态
    /// </summary>
    /// <returns></returns>
    public MiniGameStatusEnum GetMiniGameStatus()
    {
        return mMiniGameStatus;
    }

    /// <summary>
    /// 初始化游戏
    /// </summary>
    public virtual void InitGame(D miniGameData)
    {
        this.miniGameData = miniGameData;
        SetMiniGameStatus(MiniGameStatusEnum.GamePre);
    }

    /// <summary>
    /// 游戏开始
    /// </summary>
    public virtual void StartGame()
    {
        SetMiniGameStatus(MiniGameStatusEnum.Gameing);
        //通知 游戏开始
        NotifyAllObserver((int)MiniGameStatusEnum.Gameing);
    }

    /// <summary>
    /// 游戏结束
    /// </summary>
    /// <param name="isWinGame"></param>
    public virtual void EndGame(bool isWinGame)
    {
        if (GetMiniGameStatus() == MiniGameStatusEnum.Gameing)
        {
            SetMiniGameStatus(MiniGameStatusEnum.GameEnd);
            StopAllCoroutines();
            //拉近尽头
            BaseControl baseControl = controlHandler.GetControl();
            baseControl.SetCameraOrthographicSize(6);
            //开启慢镜头
            Time.timeScale = 0.1f;
            transform.DOScale(new Vector3(1, 1, 1), 0.3f).OnComplete(delegate ()
            {
                Time.timeScale = 1f;
                baseControl.SetCameraOrthographicSize();

                miniGameBuilder.DestroyAll();
                //设置游戏数据
                if (isWinGame)
                    miniGameData.gameResult = 1;
                else
                    miniGameData.gameResult = 0;
                //打开游戏结束UI
                UIMiniGameEnd uiMiniGameEnd = (UIMiniGameEnd)uiGameManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.MiniGameEnd));
                uiMiniGameEnd.SetData(miniGameData);
                uiMiniGameEnd.SetCallBack(this);
            });
            //通知 游戏结束
            NotifyAllObserver((int)MiniGameStatusEnum.GameEnd);
        }
    }

    /// <summary>
    /// 打开倒计时UI
    /// </summary>
    /// <param name="miniGameData"></param>
    public void OpenCountDownUI(MiniGameBaseBean miniGameData)
    {
        //打开游戏准备倒计时UI
        UIMiniGameCountDown uiCountDown = (UIMiniGameCountDown)uiGameManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.MiniGameCountDown));
        uiCountDown.SetCallBack(this);
        //设置胜利条件
        List<string> listWinConditions = miniGameData.GetListWinConditions();
        string targetTitleStr = "???";
        switch (miniGameData.gameType)
        {
            case MiniGameEnum.Barrage:
                targetTitleStr = GameCommonInfo.GetUITextById(202);
                break;
            case MiniGameEnum.Combat:
                targetTitleStr = GameCommonInfo.GetUITextById(205);
                break;
        }
        //设置准备UI的数据
        uiCountDown.SetData(targetTitleStr, listWinConditions);
    }


    #region 倒计时UI回调
    public virtual void GamePreCountDownStart()
    {

    }

    public virtual void GamePreCountDownEnd()
    {

    }
    #endregion

    #region 游戏结束按钮回调
    public void OnClickClose()
    {
        //通知 关闭游戏
        NotifyAllObserver((int)MiniGameStatusEnum.GameClose);
    }
    #endregion
}