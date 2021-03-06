﻿using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIGameSetting : UIGameComponent, DropdownView.ICallBack, ProgressView.ICallBack, DialogView.IDialogCallBack, IRadioButtonCallBack
{
    public Button btExitGame;
    public Button btGoMain;

    public Button btBack;
    public DropdownView dvLanguage;
    public ProgressView pvMusic;
    public ProgressView pvSound;

    public RadioButtonView rbKeyTip;

    public void Start()
    {
        if (btBack != null)
        {
            btBack.onClick.AddListener(OnClickBack);
        }
        //语言选择初始化
        if (dvLanguage != null)
        {
            dvLanguage.SetCallBack(this);
            List<Dropdown.OptionData> listLanguage = new List<Dropdown.OptionData>
            {
                new Dropdown.OptionData("简体中文")
               //new Dropdown.OptionData("English")
            };
            dvLanguage.SetData(listLanguage);
            switch (GameCommonInfo.GameConfig.language)
            {
                case "cn":
                    dvLanguage.SetPosition("简体中文");
                    break;
            }

        }
        //音乐选择初始化
        if (pvMusic != null)
        {
            pvMusic.SetData(GameCommonInfo.GameConfig.musicVolume);
            pvMusic.SetCallBack(this);
        }
        //音效选择初始化
        if (pvSound != null)
        {
            pvSound.SetData(GameCommonInfo.GameConfig.soundVolume);
            pvSound.SetCallBack(this);
        }
        //按键提示初始化
        if (rbKeyTip != null)
        {
            rbKeyTip.SetCallBack(this);
            if (GameCommonInfo.GameConfig.statusForKeyTip==0)
            {
                rbKeyTip.ChangeStates(RadioButtonView.RadioButtonStatus.Unselected);
            }
            else if (GameCommonInfo.GameConfig.statusForKeyTip == 1)
            {
                rbKeyTip.ChangeStates(RadioButtonView.RadioButtonStatus.Selected);
            }   
        }

        //离开游戏回到主菜单初始化
        if (SceneUtil.GetCurrentScene() == ScenesEnum.MainScene)
        {
            btExitGame.gameObject.SetActive(false);
            btGoMain.gameObject.SetActive(false);
        }
        else
        {
            btExitGame.gameObject.SetActive(true);
            btGoMain.gameObject.SetActive(true);
        }
        btExitGame.onClick.AddListener(OnClickExitGame);
        btGoMain.onClick.AddListener(OnClickGoMain);

    }

    public override void OpenUI()
    {
        base.OpenUI();
    }

    /// <summary>
    /// 退出点击
    /// </summary>
    public void OnClickBack()
    {
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForBack);
        if (SceneUtil.GetCurrentScene() == ScenesEnum.MainScene)
        {
            uiManager.OpenUIAndCloseOther(UIEnum.MainStart);
        }
        else
        {
            uiManager.OpenUIAndCloseOther(UIEnum.GameMain);
        }
        GameCommonInfo.SaveGameConfig();
    }

    /// <summary>
    /// 点击离开游戏
    /// </summary>
    public void OnClickExitGame()
    {
        DialogBean dialogBean = new DialogBean();
        dialogBean.dialogPosition = 1;
        dialogBean.content = GameCommonInfo.GetUITextById(3081);
        uiGameManager.dialogManager.CreateDialog(DialogEnum.Normal, this, dialogBean);
    }

    /// <summary>
    /// 点击前往主界面
    /// </summary>
    public void OnClickGoMain()
    {
        DialogBean dialogBean = new DialogBean();
        dialogBean.dialogPosition = 2;
        dialogBean.content = GameCommonInfo.GetUITextById(3082);
        uiGameManager.dialogManager.CreateDialog(DialogEnum.Normal, this, dialogBean);
    }

    #region 下拉回调
    public void OnDropDownValueChange(DropdownView view, int position, Dropdown.OptionData optionData)
    {
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        string languageStr = "cn";
        if (view == dvLanguage)
        {
            switch (optionData.text)
            {
                case "简体中文":
                    languageStr = "cn";
                    break;
            }
        }
        GameCommonInfo.GameConfig.language = languageStr;
    }
    #endregion


    #region 进度条回调
    public void OnProgressViewValueChange(ProgressView progressView, float value)
    {
        if (progressView == pvMusic)
        {
            GameCommonInfo.GameConfig.musicVolume = value;
        }
        else if (progressView == pvSound)
        {
            GameCommonInfo.GameConfig.soundVolume = value;
        }
        uiGameManager.audioHandler.InitAudio();
    }


    #endregion

    #region 弹窗确认回调
    public void Submit(DialogView dialogView, DialogBean dialogBean)
    {
        if (dialogBean.dialogPosition == 1)
        {
            //离开游戏
            GameUtil.ExitGame();
        }
        else if (dialogBean.dialogPosition == 2)
        {
            //回调主菜单
            SceneUtil.SceneChange(ScenesEnum.MainScene);
            GameCommonInfo.ClearData();
        }
    }

    public void Cancel(DialogView dialogView, DialogBean dialogBean)
    {
    }

    #endregion


    #region checkBox回调
    public void RadioButtonSelected(RadioButtonView view, RadioButtonView.RadioButtonStatus buttonStates)
    {
        if (view == rbKeyTip)
        {
            //按键提示
            if (buttonStates == RadioButtonView.RadioButtonStatus.Selected)
            {
                GameCommonInfo.GameConfig.statusForKeyTip = 1;
            }
            else if (buttonStates == RadioButtonView.RadioButtonStatus.Unselected)
            {
                GameCommonInfo.GameConfig.statusForKeyTip = 0;
            }
        }
    }
    #endregion
}