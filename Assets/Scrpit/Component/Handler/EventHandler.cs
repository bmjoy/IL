﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class EventHandler : BaseSingleton<EventHandler>
{
    public GameDataManager gameDataManager;
    public BaseUIManager uiManager;
    public StoryInfoManager storyInfoManager;
    public StoryBuilder storyBuilder;
    public ControlHandler controlHandler;

    private bool mIsEventing = false;//事件是否进行中

    /// <summary>
    /// 调查事件触发
    /// </summary>
    /// <param name="markId"></param>
    public void EventTriggerForLook(long markId)
    {
        ChangeEventStatus(true);
        //控制模式修改
        if (controlHandler != null)
            controlHandler.StopControl();
        BaseUIComponent baseUIComponent = uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameText));
        ((UIGameText)baseUIComponent).SetData(TextEnum.Look, markId);
    }

    /// <summary>
    /// 对话时间触发
    /// </summary>
    /// <param name="markId"></param>
    public void EventTriggerForTalk(long markId, UIGameText.ICallBack callBack)
    {
        ChangeEventStatus(true);
        //控制模式修改
        if (controlHandler != null)
            controlHandler.StopControl();
        BaseUIComponent baseUIComponent = uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameText));
        ((UIGameText)baseUIComponent).SetData(TextEnum.Talk, markId);
        ((UIGameText)baseUIComponent).SetCallBack(callBack);
    }

    /// <summary>
    /// 剧情触发
    /// </summary>
    /// <param name="markId"></param>
    public void EventTriggerForStory(StoryInfoBean storyInfo)
    {
        ChangeEventStatus(true);
        //控制模式修改
        if (controlHandler != null)
            controlHandler.StartControl(ControlHandler.ControlEnum.Story);
        uiManager.CloseAllUI();
        storyBuilder.BuildStory(storyInfo);
    }

    public void EventTriggerForStory(long id)
    {
        if (storyInfoManager == null)
            return;
        StoryInfoBean storyInfo = storyInfoManager.GetStoryInfoDataById(id);
        if (storyInfo != null)
            EventTriggerForStory(storyInfo);
    }

    /// <summary>
    /// 检测故事 自动触发剧情
    /// </summary>
    public bool EventTriggerForStory()
    {
        if (storyInfoManager == null)
            return false;
        StoryInfoBean storyInfo = storyInfoManager.CheckStory(gameDataManager.gameData);
        if (storyInfo != null)
        {
            EventTriggerForStory(storyInfo);
            return true;
        }
        else
            return false;
    }

    /// <summary>
    /// 改变事件状态
    /// </summary>
    /// <param name="isEvent"></param>
    public void ChangeEventStatus(bool isEvent)
    {
        mIsEventing = isEvent;
        if (controlHandler != null)
            if (!isEvent)
            {
                //事件结束 操作回复
                //如果是故事模式 则恢复普通控制状态
                if (controlHandler.GetControl() == controlHandler.GetControl(ControlHandler.ControlEnum.Story))
                {
                    controlHandler.StartControl(ControlHandler.ControlEnum.Normal);
                }
                else
                {
                    controlHandler.RestoreControl();
                }
            }
    }

    /// <summary>
    /// 获取时间状态
    /// </summary>
    /// <returns></returns>
    public bool GetEventStatus()
    {
        return mIsEventing;
    }
}