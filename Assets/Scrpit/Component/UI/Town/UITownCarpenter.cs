﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using DG.Tweening;

public class UITownCarpenter : UIBaseOne, IRadioGroupCallBack, StoreInfoManager.ICallBack
{
    public RadioGroupView rgCerpenterType;

    public GameObject objCarpenterContent;
    public GameObject objCarpenterModel;

    private List<StoreInfoBean> mCarpenterListData;
    protected StoreForCarpenterTypeEnum selectType;
    public override void Awake()
    {
        base.Awake();
        if (rgCerpenterType != null)
            rgCerpenterType.SetCallBack(this);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        rgCerpenterType.SetPosition(0, false);

        uiGameManager.storeInfoManager.SetCallBack(this);
        uiGameManager.storeInfoManager.GetStoreInfoForCarpenter();
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        InitDataByType(selectType);
    }

    public void InitDataByType(StoreForCarpenterTypeEnum type)
    {
        List<StoreInfoBean> listData = GetCerpenterListDataByType(type);
        CreateCarpenterData(listData, type);
    }

    /// <summary>
    ///  根据备注类型获取数据
    /// </summary>
    /// <param name="mark"></param>
    /// <returns></returns>
    public List<StoreInfoBean> GetCerpenterListDataByType(StoreForCarpenterTypeEnum type)
    {
        List<StoreInfoBean> listData = new List<StoreInfoBean>();
        if (mCarpenterListData == null)
            return listData;
        for (int i = 0; i < mCarpenterListData.Count; i++)
        {
            StoreInfoBean itemData = mCarpenterListData[i];
            if (itemData.store_goods_type == (int)type)
            {
                listData.Add(itemData);
            }
        }
        return listData;
    }

    /// <summary>
    /// 创建商品列表
    /// </summary>
    /// <param name="listData"></param>
    public void CreateCarpenterData(List<StoreInfoBean> listData, StoreForCarpenterTypeEnum type)
    {
        CptUtil.RemoveChildsByActive(objCarpenterContent.transform);
        if (listData == null || objCarpenterContent == null)
            return;
        for (int i = 0; i < listData.Count; i++)
        {
            StoreInfoBean itemData = listData[i];
            //如果扩建 
            if (itemData.store_goods_type == (int)StoreForCarpenterTypeEnum.Expansion)
            {
                if (uiGameManager.gameDataManager.gameData.GetInnBuildData().buildLevel + 1 != int.Parse(itemData.mark))
                {
                    continue;
                }
            }
            GameObject itemObj = Instantiate(objCarpenterContent, objCarpenterModel);
            ItemTownCerpenterCpt cerpenterCpt = itemObj.GetComponent<ItemTownCerpenterCpt>();
            cerpenterCpt.SetData(itemData);
            //itemObj.transform.DOScale(new Vector3(0, 0, 0), 0.5f).SetEase(Ease.OutBack).SetDelay(i * 0.05f).From();
        }
    }

    #region 获取商店物品回调
    public void GetStoreInfoSuccess(StoreTypeEnum type, List<StoreInfoBean> listData)
    {
        mCarpenterListData = listData;
        selectType = StoreForCarpenterTypeEnum.Expansion;
        InitDataByType(selectType);
    }
    #endregion

    #region 类型选择回调
    public void RadioButtonSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);

        selectType = EnumUtil.GetEnum<StoreForCarpenterTypeEnum>(rbview.name);
        InitDataByType(selectType);
    }

    public void RadioButtonUnSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {

    }
    #endregion
}