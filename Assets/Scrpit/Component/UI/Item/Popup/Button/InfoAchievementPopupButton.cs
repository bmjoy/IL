﻿using UnityEngine;
using UnityEditor;

public class InfoAchievementPopupButton : PopupButtonView
{
    public AchievementInfoBean achievementInfo;
    public ItemGameGuildAchievementCpt.AchievementStatusEnum status;

    public void SetData(ItemGameGuildAchievementCpt.AchievementStatusEnum status, AchievementInfoBean achievementInfo)
    {
        this.achievementInfo = achievementInfo;
        this.status = status;
    }

    public override void ClosePopup()
    {

    }

    public override void OpenPopup()
    {
        if (status == 0)
        {
            popupShow.gameObject.SetActive(false);
        }
        else
        {
            ((InfoAchievementPopupShow)popupShow).SetData(status,achievementInfo);
        }
       
    }

}