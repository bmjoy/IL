﻿using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public class UIMainCreate : BaseUIComponent,
    IRadioGroupCallBack,
    ColorView.CallBack,
    SelectView.CallBack,
    DialogView.IDialogCallBack

{
    public GameObject objContent;

    //返回按钮
    public Button btBack;
    public Text tvBack;
    //开始按钮
    public Button btCreate;
    public Text tvCreate;

    public InputField etInnName;
    public InputField etUserName;

    //性别选择
    public RadioGroupView rgSex;
    //皮肤颜色
    public ColorView colorSkin;
    //发型
    public ColorView colorHair;
    public SelectView selectHair;
    //眼睛
    public ColorView colorEye;
    public SelectView selectEye;
    //嘴
    public ColorView colorMouth;
    public SelectView selectMouth;
    //帽子
    public SelectView selectHat;
    //衣服
    public SelectView selectClothes;
    //鞋子
    public SelectView selectShoes;

    //角色身体控制
    public CharacterBodyCpt characterBodyCpt;
    //角色着装控制
    public CharacterDressCpt characterDressCpt;

    public List<IconBean> listSelectHair;
    public List<IconBean> listSelectEye;
    public List<IconBean> listSelectMouth;

    public List<ItemsInfoBean> listSelectHat;
    public List<ItemsInfoBean> listSelectClothes;
    public List<ItemsInfoBean> listSelectShoes;

    private void Start()
    {
        InitData();

        if (btBack != null)
            btBack.onClick.AddListener(OpenStartUI);
        if (btCreate != null)
            btCreate.onClick.AddListener(CreateNewGame);
        if (rgSex != null)
            rgSex.SetCallBack(this);
        if (colorSkin != null)
            colorSkin.SetCallBack(this);
        if (colorHair != null)
            colorHair.SetCallBack(this);
        if (selectHair != null)
        {
            selectHair.SetSelectNumber(listSelectHair.Count);
            selectHair.SetCallBack(this);
        }
        if (colorEye != null)
            colorEye.SetCallBack(this);
        if (selectEye != null)
        {
            selectEye.SetSelectNumber(listSelectEye.Count);
            selectEye.SetCallBack(this);
        }
        if (colorMouth != null)
            colorMouth.SetCallBack(this);
        if (selectMouth != null)
        {
            selectMouth.SetSelectNumber(listSelectMouth.Count);
            selectMouth.SetCallBack(this);
        }
        if (selectHat != null)
        {
            selectHat.SetSelectNumber(listSelectHat.Count);
            selectHat.SetCallBack(this);
        }
        if (selectClothes != null)
        {
            selectClothes.SetSelectNumber(listSelectClothes.Count);
            selectClothes.SetCallBack(this);
        }
        if (selectShoes != null)
        {
            selectShoes.SetSelectNumber(listSelectShoes.Count);
            selectShoes.SetCallBack(this);
        }
    }

    public override void OpenUI()
    {
        base.OpenUI();
        AnimForInit();
    }

    public void InitData()
    {
        CharacterBodyManager characterBodyManager = GetUIManager<UIGameManager>().characterBodyManager;
        GameItemsManager gameItemsManager = GetUIManager<UIGameManager>().gameItemsManager;

        //初始化可选择头型数据
        listSelectHair = TypeConversionUtil.IconBeanDictionaryToList(characterBodyManager.listIconBodyHair);
        ChangeSelectPosition(selectHair, 0);
        //初始化可选择眼睛
        listSelectEye = TypeConversionUtil.IconBeanDictionaryToList(characterBodyManager.listIconBodyEye);
        ChangeSelectPosition(selectEye, 0);
        //初始化可选择嘴巴
        listSelectMouth = TypeConversionUtil.IconBeanDictionaryToList(characterBodyManager.listIconBodyMouth);
        ChangeSelectPosition(selectMouth, 0);
        //初始化帽子
        listSelectHat = gameItemsManager.GetItemsById(new long[] { 100001 });
        listSelectHat.Insert(0, new ItemsInfoBean());
        //初始化衣服
        listSelectClothes = gameItemsManager.GetItemsById(new long[] { 200001 });
        listSelectClothes.Insert(0, new ItemsInfoBean());
        //初始化鞋子
        listSelectShoes = gameItemsManager.GetItemsById(new long[] { 300001 });
        listSelectShoes.Insert(0, new ItemsInfoBean());
    }

    /// <summary>
    /// 初始化动画
    /// </summary>
    public void AnimForInit()
    {
        if (objContent != null)
            objContent.transform.DOScaleX(0, 0.5f).From().SetEase(Ease.OutExpo);
        if (btCreate != null)
            btCreate.transform.DOScaleX(0, 0.5f).From().SetEase(Ease.OutBack);
        if (btBack != null)
            btBack.transform.DOScaleX(0, 0.5f).From().SetEase(Ease.OutBack).SetDelay(0.1f);
    }

    /// <summary>
    /// 创建新游戏
    /// </summary>
    public void CreateNewGame()
    {
        //按键音效
        AudioHandler audioHandler = GetUIManager<UIGameManager>().audioHandler;
        audioHandler.PlaySound(SoundEnum.ButtonForNormal);

        ToastManager toastManager = GetUIManager<UIGameManager>().toastManager;
        DialogManager dialogManager = GetUIManager<UIGameManager>().dialogManager;
        if (CheckUtil.StringIsNull(etInnName.text))
        {
            toastManager.ToastHint(GameCommonInfo.GetUITextById(1000));
            return;
        }
        if (CheckUtil.StringIsNull(etUserName.text))
        {
            toastManager.ToastHint(GameCommonInfo.GetUITextById(1001));
            return;
        }
        DialogBean dialogData = new DialogBean();
        dialogData.content = GameCommonInfo.GetUITextById(3012);
        dialogManager.CreateDialog(DialogEnum.Normal, this, dialogData);
    }

    /// <summary>
    /// 返回开始菜单
    /// </summary>
    public void OpenStartUI()
    {
        //按键音效
        AudioHandler audioHandler = GetUIManager<UIGameManager>().audioHandler;
        audioHandler.PlaySound(SoundEnum.ButtonForBack);

        uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.MainStart));
    }

    #region 性别回调
    public void RadioButtonSelected(RadioGroupView rgView, int position, RadioButtonView view)
    {
        if (position == 0)
        {
            characterBodyCpt.SetSex(1);
        }
        else
        {
            characterBodyCpt.SetSex(2);
        }
    }

    public void RadioButtonUnSelected(RadioGroupView rgView, int position, RadioButtonView view)
    {

    }
    #endregion

    #region 颜色回调
    public void ColorChange(ColorView colorView, float r, float g, float b)
    {
        if (colorView == colorSkin)
        {
            characterBodyCpt.SetSkin(colorSkin.GetColor());
        }
        else if (colorView == colorHair)
        {
            characterBodyCpt.SetHair(colorHair.GetColor());
        }
        else if (colorView == colorEye)
        {
            characterBodyCpt.SetEye(colorEye.GetColor());
        }
        else if (colorView == colorMouth)
        {
            characterBodyCpt.SetMouth(colorMouth.GetColor());
        }

    }
    #endregion

    #region 选择回调
    public void ChangeSelectPosition(SelectView selectView, int position)
    {
        GameItemsManager gameItemsManager = GetUIManager<UIGameManager>().gameItemsManager;
        if (selectView == selectHair)
        {
            characterBodyCpt.SetHair(listSelectHair[position].key);
        }
        else if (selectView == selectEye)
        {
            characterBodyCpt.SetEye(listSelectEye[position].key);
        }
        else if (selectView == selectMouth)
        {
            characterBodyCpt.SetMouth(listSelectMouth[position].key);
        }
        else if (selectView == selectHat)
        {
            characterDressCpt.SetHat(listSelectHat[position]);
        }
        else if (selectView == selectClothes)
        {
            characterDressCpt.SetClothes(listSelectClothes[position]);
        }
        else if (selectView == selectShoes)
        {
            characterDressCpt.SetShoes(listSelectShoes[position]);
        }
    }
    #endregion

    #region 确认回调
    public void Submit(DialogView dialogView, DialogBean dialogBean)
    {
        GameDataManager gameDataManager = GetUIManager<UIGameManager>().gameDataManager;
        GameDataBean gameData = new GameDataBean();
        gameData.innAttributes.innName = etInnName.text;

        gameData.userCharacter = new CharacterBean();
        gameData.userCharacter.baseInfo.name = etUserName.text;
        gameData.userCharacter.body = characterBodyCpt.GetCharacterBodyData();
        gameData.userCharacter.equips = characterDressCpt.GetCharacterEquipData();
        gameDataManager.CreateGameData(gameData);

        SceneUtil.SceneChange(ScenesEnum.GameInnScene);
    }

    public void Cancel(DialogView dialogView, DialogBean dialogBean)
    {

    }
    #endregion
}