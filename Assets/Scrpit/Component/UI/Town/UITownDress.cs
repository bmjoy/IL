﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using DG.Tweening;
using System.Collections;

public class UITownDress : UIBaseOne, IRadioGroupCallBack, StoreInfoManager.ICallBack
{
    public GameObject objGroceryContent;
    public GameObject objGroceryModel;

    public RadioGroupView rgStyleType;
    public RadioGroupView rgPartType;

    private List<StoreInfoBean> mClothesListData;

    private StoreForDressTypeEnum mStyleType = 0;
    private GeneralEnum mPartType = 0;


    public new void Start()
    {
        base.Start();
        if (rgStyleType != null)
            rgStyleType.SetCallBack(this);
        if (rgPartType != null)
            rgPartType.SetCallBack(this);
    }

    public override void OpenUI()
    {
        base.OpenUI();

        uiGameManager.storeInfoManager.SetCallBack(this);
        uiGameManager.storeInfoManager.GetStoreInfoForDress();

        rgStyleType.SetPosition(0, false);
        rgPartType.SetPosition(0, false);
    }

    /// <summary>
    /// 根据类型和部位初始化数据
    /// </summary>
    /// <param name="styleType"></param>
    /// <param name="partType"></param>
    public void InitDataByType(StoreForDressTypeEnum styleType, GeneralEnum partType)
    {
        List<StoreInfoBean> createListData = new List<StoreInfoBean>();
        switch (partType)
        {
            case GeneralEnum.Null:
                createListData = mClothesListData;
                break;
            case GeneralEnum.Mask:
            case GeneralEnum.Hat:
            case GeneralEnum.Clothes:
            case GeneralEnum.Shoes:
                createListData = GetClothesListDataByPart(partType);
                break;
        }
        List<StoreInfoBean> listData = GetClothesListDataByType(styleType, createListData);
        CreateClothesData(listData);
    }

    /// <summary>
    ///  根据类型获取数据
    /// </summary>
    /// <param name="mark"></param>
    /// <returns></returns>
    public List<StoreInfoBean> GetClothesListDataByType(StoreForDressTypeEnum styleType, List<StoreInfoBean> listStoreData)
    {
        if (styleType == 0)
        {
            return listStoreData;
        }
        List<StoreInfoBean> listData = new List<StoreInfoBean>();
        if (listStoreData == null)
            return listData;

        for (int i = 0; i < listStoreData.Count; i++)
        {
            StoreInfoBean itemData = listStoreData[i];
            if (itemData.store_goods_type == (int)styleType)
            {
                listData.Add(itemData);
            }
        }
        return listData;
    }

    /// <summary>
    ///  根据备注获取数据
    /// </summary>
    /// <param name="mark"></param>
    /// <returns></returns>
    public List<StoreInfoBean> GetClothesListDataByPart(GeneralEnum partType)
    {
        List<StoreInfoBean> listData = new List<StoreInfoBean>();
        if (mClothesListData == null)
            return listData;
        for (int i = 0; i < mClothesListData.Count; i++)
        {
            StoreInfoBean itemData = mClothesListData[i];
            ItemsInfoBean itemsInfo = uiGameManager.gameItemsManager.GetItemsById(itemData.mark_id);
            if (itemsInfo.items_type == (int)partType)
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
    public void CreateClothesData(List<StoreInfoBean> listData)
    {
        CptUtil.RemoveChildsByActive(objGroceryContent.transform);
        if (listData == null || objGroceryContent == null || objGroceryModel == null)
            return;
        for (int i = 0; i < listData.Count; i++)
        {
            StoreInfoBean itemData = listData[i];
            GameObject itemObj = Instantiate(objGroceryContent, objGroceryModel);
            ItemTownStoreForGoodsCpt goodsCpt = itemObj.GetComponent<ItemTownStoreForGoodsCpt>();
            goodsCpt.SetData(itemData);
        }
    }


    #region 商店数据回调
    public void GetStoreInfoSuccess(StoreTypeEnum type, List<StoreInfoBean> listData)
    {
        mClothesListData = listData;
        InitDataByType(StoreForDressTypeEnum.Fashion, GeneralEnum.Null);
    }
    #endregion

    #region 类型选择回调
    public void RadioButtonSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        if (rgView == rgStyleType)
        {
            mStyleType = EnumUtil.GetEnum<StoreForDressTypeEnum>(rbview.name);
        }
        else if (rgView == rgPartType)
        {
            mPartType = EnumUtil.GetEnum<GeneralEnum>(rbview.name);

        }
        InitDataByType(mStyleType, mPartType);
    }

    public void RadioButtonUnSelected(RadioGroupView rgView, int position, RadioButtonView view)
    {

    }
    #endregion
}