﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainStart : BaseUIComponent
{
    //开始按钮
    public Button btStart;
    public Text tvStart;
    //继续按钮
    public Button btContinue;
    public Text tvContinue;
    //设置按钮
    public Button btSetting;
    public Text tvSetting;
    //离开按钮
    public Button btExit;
    public Text tvExit;

    private void Start()
    {
        if (btStart != null)
        {
            btStart.onClick.AddListener(OpenCreateUI);
        }
        if (btContinue != null)
        {
            btContinue.onClick.AddListener(OpenContinueUI);
        }
        if (btExit != null)
        {
            btExit.onClick.AddListener(ExitGame);
        }

        if (tvStart != null)
            tvStart.text = GameCommonInfo.GetUITextById(4011);
        if (tvContinue != null)
            tvContinue.text = GameCommonInfo.GetUITextById(4012);
        if (tvSetting != null)
            tvSetting.text = GameCommonInfo.GetUITextById(4013);
        if (tvExit != null)
            tvExit.text = GameCommonInfo.GetUITextById(4014);
    }

    public override void RefreshUI()
    {
        base.RefreshUI();

    }

    /// <summary>
    /// 打开继续页面
    /// </summary>
    public void OpenContinueUI()
    {
        uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.MainContinue));
    }

    /// <summary>
    /// 打开创建页面
    /// </summary>
    public void OpenCreateUI()
    {
        uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.MainCreate));
    }

    /// <summary>
    /// 离开游戏
    /// </summary>
    public void ExitGame()
    {
        GameUtil.ExitGame();
    }

}
