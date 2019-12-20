﻿using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using DG.Tweening;
public class PopupItemsSelection : BaseMonoBehaviour
{
    public enum SelectionTypeEnum
    {
        Discard,//只有丢弃
        UseAndDiscard,//使用和丢弃
        EquipAndDiscard,//装备和丢弃
        Unload,//卸除 用于装备界面
        Gift,//赠送
    }

    public ICallBack callBack;
    public Button btBack;

    public Button btUse;
    public Button btDiscard;
    public Button btEquip;
    public Button btUnload;
    public Button btGift;
    public GameObject objContent;

    //屏幕(用来找到鼠标点击的相对位置)
    public RectTransform screenRTF;
    public RectTransform popupRTF;
    //鼠标位置和弹窗偏移量
    public float offsetX = 0;
    public float offsetY = 0;

    private void Start()
    {
        if (btBack != null)
            btBack.onClick.AddListener(Close);
        if (btUse != null)
            btUse.onClick.AddListener(UseItems);
        if (btDiscard != null)
            btDiscard.onClick.AddListener(DiscardItems);
        if (btEquip != null)
            btEquip.onClick.AddListener(EquipItems);
        if (btUnload != null)
            btUnload.onClick.AddListener(UnloadItems);
        if (btGift != null)
            btGift.onClick.AddListener(GiftItems);
    }

    public void SetCallBack(ICallBack callBack)
    {
        this.callBack = callBack;
    }


    public void Close()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    public void Open(SelectionTypeEnum type)
    {
        btUse.gameObject.SetActive(false);
        btDiscard.gameObject.SetActive(false);
        btEquip.gameObject.SetActive(false);
        btUnload.gameObject.SetActive(false);
        btGift.gameObject.SetActive(false);
        switch (type)
        {
            case SelectionTypeEnum.Discard:
                btDiscard.gameObject.SetActive(true);
                break;
            case SelectionTypeEnum.UseAndDiscard:
                btUse.gameObject.SetActive(true);
                btDiscard.gameObject.SetActive(true);
                break;
            case SelectionTypeEnum.EquipAndDiscard:
                btEquip.gameObject.SetActive(true);
                btDiscard.gameObject.SetActive(true);
                break;
            case SelectionTypeEnum.Unload:
                btUnload.gameObject.SetActive(true);
                break;
            case SelectionTypeEnum.Gift:
                btGift.gameObject.SetActive(true);
                break;
        }
        gameObject.SetActive(true);
        objContent.transform.DOScale(new Vector3(0, 0, 0), 0.2f).SetEase(Ease.OutBack).From();
        SetLocation();
    }

    public void SetLocation()
    {
        //如果显示Popup 则调整位置为鼠标位置
        if (gameObject.activeSelf)
        {
            Vector2 outPosition;
            //屏幕坐标转换为UI坐标
            RectTransformUtility.ScreenPointToLocalPointInRectangle(screenRTF, Input.mousePosition, Camera.main, out outPosition);
            float moveX = outPosition.x;
            float moveY = outPosition.y;
            //Vector3 newPosition= Vector3.Lerp(transform.localPosition, new Vector3(moveX + offsetX, moveY + offsetY, transform.localPosition.z),0.5f);
            objContent.transform.localPosition = new Vector3(moveX + offsetX, moveY + offsetY, transform.localPosition.z);
        }
    }

    /// <summary>
    /// 使用物品
    /// </summary>
    public void UseItems()
    {
        if (callBack != null)
            callBack.SelectionUse(this);
        Close();
    }

    /// <summary>
    /// 丢弃物品
    /// </summary>
    public void DiscardItems()
    {
        if (callBack != null)
            callBack.SelectionDiscard(this);
        Close();
    }

    /// <summary>
    /// 装备
    /// </summary>
    public void EquipItems()
    {
        if (callBack != null)
            callBack.SelectionEquip(this);
        Close();
    }

    /// <summary>
    /// 卸下
    /// </summary>
    public void UnloadItems()
    {
        if (callBack != null)
            callBack.SelectionUnload(this);
        Close();
    }

    public void GiftItems()
    {
        if (callBack != null)
            callBack.SelectionUnload(this);
        Close();
    }

    public interface ICallBack
    {
        /// <summary>
        /// 选择使用
        /// </summary>
        /// <param name="view"></param>
        void SelectionUse(PopupItemsSelection view);

        /// <summary>
        /// 选择丢弃
        /// </summary>
        /// <param name="view"></param>
        void SelectionDiscard(PopupItemsSelection view);

        /// <summary>
        /// 选择装备
        /// </summary>
        /// <param name="view"></param>
        void SelectionEquip(PopupItemsSelection view);

        /// <summary>
        /// 选择卸下
        /// </summary>
        /// <param name="view"></param>
        void SelectionUnload(PopupItemsSelection view);

        /// <summary>
        /// 选择赠送
        /// </summary>
        /// <param name="view"></param>
        void SelectionGift(PopupItemsSelection view);
    }
}