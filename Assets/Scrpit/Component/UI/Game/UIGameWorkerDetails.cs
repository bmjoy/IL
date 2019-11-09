﻿using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
public class UIGameWorkerDetails : BaseUIComponent, IRadioGroupCallBack
{
    [Header("控件")]
    public CharacterUICpt characterUICpt;

    public InfoItemsPopupButton pbHand;
    public Image ivHand;
    public InfoItemsPopupButton pbHat;
    public Image ivHat;
    public InfoItemsPopupButton pbClothes;
    public Image ivClothes;
    public InfoItemsPopupButton pbShoes;
    public Image ivShoes;

    public Text tvLoyal;
    public CharacterAttributeView characterAttributeView;
    public Text tvCook;
    public Text tvSpeed;
    public Text tvAccount;
    public Text tvCharm;
    public Text tvForce;
    public Text tvLucky;

    public ItemGameWorkerDetailsWorkerCpt detailsForChef;
    public ItemGameWorkerDetailsWorkerCpt detailsForWaiter;
    public ItemGameWorkerDetailsWorkerCpt detailsForAccounting;
    public ItemGameWorkerDetailsWorkerCpt detailsForAccost;
    public ItemGameWorkerDetailsWorkerCpt detailsForBeater;

    public RadioGroupView rgWorkerTitle;
    public UIGameWorkerDetailsChefInfo workerChefInfo;
    public UIGameWorkerDetailsWaiterInfo workerWaiterInfo;
    public UIGameWorkerDetailsAccountingInfo workerAccountingInfo;
    public UIGameWorkerDetailsAccostInfo workerAccostInfo;
    public UIGameWorkerDetailsBeaterInfo workerBeaterInfo;

    public Button btBack;

    [Header("数据")]
    public CharacterBean characterData;

    public void SetCharacterData(CharacterBean characterData)
    {
        this.characterData = characterData;
    }

    private void Start()
    {
        InfoItemsPopupShow infoItemsPopupShow = GetUIMananger<UIGameManager>().infoItemsPopup;
        if (btBack != null)
            btBack.onClick.AddListener(OpenWorkUI);

        if (pbHand != null)
            pbHand.SetPopupShowView(infoItemsPopupShow);
        if (pbHat != null)
            pbHat.SetPopupShowView(infoItemsPopupShow);
        if (pbClothes != null)
            pbClothes.SetPopupShowView(infoItemsPopupShow);
        if (pbShoes != null)
            pbShoes.SetPopupShowView(infoItemsPopupShow);
        if (rgWorkerTitle != null)
        {
            rgWorkerTitle.SetCallBack(this);
            rgWorkerTitle.SetPosition(0, true);
        } 
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    /// <param name="workerType"></param>
    public void InitDataByWorker(WorkerEnum workerType)
    {
        if (characterData == null)
            return;
        workerChefInfo.gameObject.SetActive(false);
        workerWaiterInfo.gameObject.SetActive(false);
        workerAccountingInfo.gameObject.SetActive(false);
        workerAccostInfo.gameObject.SetActive(false);
        workerBeaterInfo.gameObject.SetActive(false);
        switch (workerType)
        {
            case WorkerEnum.Chef:
                InnFoodManager innFoodManager = GetUIMananger<UIGameManager>().innFoodManager;
                workerChefInfo.gameObject.SetActive(true);
                workerChefInfo.SetData(innFoodManager,characterData.baseInfo.chefInfo);
                break;
            case WorkerEnum.Waiter:
                workerWaiterInfo.gameObject.SetActive(true);
                workerWaiterInfo.SetData(characterData.baseInfo.waiterInfo);
                break;
            case WorkerEnum.Accounting:
                workerAccountingInfo.gameObject.SetActive(true);
                workerAccountingInfo.SetData(characterData.baseInfo.accountingInfo);
                break;
            case WorkerEnum.Accost:
                workerAccostInfo.gameObject.SetActive(true);
                workerAccostInfo.SetData(characterData.baseInfo.accostInfo);
                break;
            case WorkerEnum.Beater:
                workerBeaterInfo.gameObject.SetActive(true);
                break;
        }
    }


    public override void OpenUI()
    {
        base.OpenUI();
        if (characterData == null)
            return;
        SetLoyal(characterData.attributes.loyal);
        SetAttributes(characterData);
        SetEquip(characterData.equips);
        SetWorkerInfo(characterData.baseInfo);
        characterUICpt.SetCharacterData(characterData.body, characterData.equips);
        rgWorkerTitle.SetPosition(0 , true);
    }

    public void OpenWorkUI()
    {
        uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameWorker));
    }

    /// <summary>
    /// 设置忠诚
    /// </summary>
    /// <param name="loyal"></param>
    public void SetLoyal(int loyal)
    {
        if (tvLoyal != null)
        {
            tvLoyal.text = loyal + "";
        }
    }

    /// <summary>
    /// 设置装备
    /// </summary>
    /// <param name="characterEquip"></param>
    public void SetEquip(CharacterEquipBean characterEquip)
    {
        GameItemsManager gameItemsManager = GetUIMananger<UIGameManager>().gameItemsManager;
        CharacterDressManager characterDressManager = GetUIMananger<UIGameManager>().characterDressManager;
        Sprite spHand = null;
        Sprite spHat = null;
        Sprite spClothes = null;
        Sprite spShoes = null;
        if (characterData.equips.handId != 0)
        {
            //查询装备数据
            ItemsInfoBean itemHand = gameItemsManager.GetItemsById(characterData.equips.handId);
            if (itemHand != null && !CheckUtil.StringIsNull(itemHand.icon_key))
            {
                //获取装备图标
                spHand = gameItemsManager.GetItemsSpriteByName(itemHand.icon_key);
                pbHand.SetData(itemHand, spHand);
            }
        }
        if (characterData.equips.hatId != 0)
        {
            //查询装备数据
            ItemsInfoBean itemsHat = gameItemsManager.GetItemsById(characterData.equips.hatId);
            if (itemsHat != null && !CheckUtil.StringIsNull(itemsHat.icon_key))
            {
                //获取装备图标
                spHat = characterDressManager.GetHatSpriteByName(itemsHat.icon_key);
                pbHat.SetData(itemsHat, spHat);
            }
        }
        if (characterData.equips.clothesId != 0)
        {
            //查询装备数据
            ItemsInfoBean itemsClothes = gameItemsManager.GetItemsById(characterData.equips.clothesId);
            if (itemsClothes != null && !CheckUtil.StringIsNull(itemsClothes.icon_key))
            {
                //获取装备图标
                spClothes = characterDressManager.GetClothesSpriteByName(itemsClothes.icon_key);
                pbClothes.SetData(itemsClothes, spClothes);
            }
        }
        if (characterData.equips.shoesId != 0)
        {
            //查询装备数据
            ItemsInfoBean itemShoes = gameItemsManager.GetItemsById(characterData.equips.shoesId);
            if (itemShoes != null && !CheckUtil.StringIsNull(itemShoes.icon_key))
            {
                //获取装备图标
                spShoes = characterDressManager.GetShoesSpriteByName(itemShoes.icon_key);
                pbShoes.SetData(itemShoes, spShoes);
            }
        }
        SetEquipSprite(ivHand, spHand);
        SetEquipSprite(ivHat, spHat);
        SetEquipSprite(ivClothes, spClothes);
        SetEquipSprite(ivShoes, spShoes);
    }

    /// <summary>
    /// 设置属性
    /// </summary>
    /// <param name="characterAttributes"></param>
    /// <param name="characterEquip"></param>
    public void SetAttributes(CharacterBean characterData)
    {
        characterData.GetAttributes(GetUIMananger<UIGameManager>().gameItemsManager,
            out CharacterAttributesBean totalAttributes, out CharacterAttributesBean selfAttributes, out CharacterAttributesBean equipAttributes);
        if (tvCook != null)
            tvCook.text = GameCommonInfo.GetUITextById(1) + "：" + selfAttributes.cook + (equipAttributes.cook == 0 ? "" : "+" + equipAttributes.cook);
        if (tvSpeed != null)
            tvSpeed.text = GameCommonInfo.GetUITextById(2) + "：" + selfAttributes.speed + (equipAttributes.speed == 0 ? "" : "+" + equipAttributes.speed);
        if (tvAccount != null)
            tvAccount.text = GameCommonInfo.GetUITextById(3) + "：" + selfAttributes.account + (equipAttributes.account == 0 ? "" : "+" + equipAttributes.account);
        if (tvCharm != null)
            tvCharm.text = GameCommonInfo.GetUITextById(4) + "：" + selfAttributes.charm + (equipAttributes.charm == 0 ? "" : "+" + equipAttributes.charm);
        if (tvForce != null)
            tvForce.text = GameCommonInfo.GetUITextById(5) + "：" + selfAttributes.force + (equipAttributes.force == 0 ? "" : "+" + equipAttributes.force);
        if (tvLucky != null)
            tvLucky.text = GameCommonInfo.GetUITextById(6) + "：" + selfAttributes.lucky + (equipAttributes.lucky == 0 ? "" : "+" + equipAttributes.lucky);
        if (characterAttributeView != null)
            characterAttributeView.SetData(totalAttributes.cook, totalAttributes.speed, totalAttributes.account, totalAttributes.charm, totalAttributes.force);
    }

    /// <summary>
    /// 设置职业数据
    /// </summary>
    public void SetWorkerInfo(CharacterBaseBean characterBase)
    {
        if (characterBase == null)
            return;
        if (detailsForChef != null)
            detailsForChef.SetData(WorkerEnum.Chef, characterBase.chefInfo);
        if (detailsForWaiter != null)
            detailsForWaiter.SetData(WorkerEnum.Waiter, characterBase.waiterInfo);
        if (detailsForAccounting != null)
            detailsForAccounting.SetData(WorkerEnum.Accounting, characterBase.accountingInfo);
        if (detailsForAccost != null)
            detailsForAccost.SetData(WorkerEnum.Accost, characterBase.accostInfo);
        if (detailsForBeater != null)
            detailsForBeater.SetData(WorkerEnum.Beater, characterBase.beaterInfo);
    }

    /// <summary>
    /// 设置装备图标
    /// </summary>
    /// <param name="iv"></param>
    /// <param name="spIcon"></param>
    private void SetEquipSprite(Image iv, Sprite spIcon)
    {
        if (spIcon != null)
        {
            iv.sprite = spIcon;
            iv.color = new Color(1, 1, 1, 1);
        }
        else
            iv.color = new Color(1, 1, 1, 0);
    }


    #region 数据类型选择回调
    public void RadioButtonSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {
        InitDataByWorker((WorkerEnum)(position + 1));
    }

    public void RadioButtonUnSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {

    }
    #endregion
}