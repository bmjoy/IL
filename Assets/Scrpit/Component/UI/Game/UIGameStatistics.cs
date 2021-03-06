﻿using UnityEngine;
using UnityEditor;

public class UIGameStatistics : UIBaseOne,IRadioGroupCallBack
{
    public UIGameStatisticsForInn innUI;
    public UIGameStatisticsForRevenue revenueUI;
    public UIGameStatisticsForAch achUI;
    public UIGameStatisticsForCustomer customerUI;
    public RadioGroupView rgType;

    public RadioButtonView rbTypeInn;
    public RadioButtonView rbTypeRevenue;
    public RadioButtonView rbTypeAch;
    public RadioButtonView rbTypeCustomer;

    public override void Start()
    {
        base.Start();
    }

    public override void OpenUI()
    {
        base.OpenUI();
        rgType.SetCallBack(this);
        rgType.SetPosition(0, true);
    }

    #region 类型选择回调
    public void RadioButtonSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);

        innUI.Close();
        revenueUI.Close();
        achUI.Close();
        customerUI.Close();
        if (rbview== rbTypeInn)
        {
            innUI.Open();
        }
        else if (rbview == rbTypeRevenue)
        {
            revenueUI.Open();
        }
        else if (rbview == rbTypeAch)
        {
            achUI.Open();
        }
        else if (rbview == rbTypeCustomer)
        {
            customerUI.Open();
        }
    }

    public void RadioButtonUnSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {

    }
    #endregion
}