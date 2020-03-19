﻿using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using DG.Tweening;

public class ItemGameBackpackCpt : ItemGameBaseCpt, IPointerClickHandler, PopupItemsSelection.ICallBack, DialogView.IDialogCallBack
{
    public Text tvName;
    public Text tvNumber;
    public RectTransform rtIcon;
    public Image ivIcon;
    public InfoItemsPopupButton infoItemsPopup;

    public ItemsInfoBean itemsInfoBean;
    public ItemBean itemBean;

    public UnityEvent leftClick;
    public UnityEvent rightClick;

    protected UIGameManager uiGameManager;
    protected PopupItemsSelection popupItemsSelection;

    public virtual void Awake()
    {
        uiGameManager = Find<UIGameManager>(ImportantTypeEnum.GameUI);
        popupItemsSelection = uiGameManager.popupItemsSelection;
    }

    public void Start()
    {
        leftClick.AddListener(new UnityAction(ButtonClick));
        rightClick.AddListener(new UnityAction(ButtonClick));
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="infoBean"></param>
    /// <param name="itemBean"></param>
    public void SetData(ItemsInfoBean infoBean, ItemBean itemBean)
    {
        this.itemsInfoBean = infoBean;
        this.itemBean = itemBean;
        if (infoBean != null)
        {
            SetName(infoBean.name);
        }
        else
        {
            SetName("");
        }
        SetIcon(infoBean);
        if (itemBean != null)
        {
            SetNumber(itemBean.itemNumber);
        }
        infoItemsPopup.SetData(itemsInfoBean, ivIcon.sprite);
    }

    /// <summary>
    /// 设置Icon
    /// </summary>
    /// <param name="iconKey"></param>
    /// <param name="itemType"></param>
    public void SetIcon(ItemsInfoBean itemsInfo)
    {
        CharacterDressManager characterDressManager = uiGameManager.characterDressManager;
        GameItemsManager gameItemsManager = uiGameManager.gameItemsManager;
        IconDataManager iconDataManager = uiGameManager.iconDataManager;

        Vector2 offsetMin = new Vector2(0, 0);
        Vector2 offsetMax = new Vector2(0, 0);
        Sprite spIcon = null;
        if (itemsInfo != null)
        {
            switch (itemsInfo.items_type)
            {
                case (int)GeneralEnum.Hat:
                    offsetMin = new Vector2(-30, -60);
                    offsetMax = new Vector2(30, 0);
                    break;
                case (int)GeneralEnum.Clothes:
                    offsetMin = new Vector2(-30, -20);
                    offsetMax = new Vector2(30, 40);
                    break;
                case (int)GeneralEnum.Shoes:
                    offsetMin = new Vector2(-30, 0);
                    offsetMax = new Vector2(30, 60);
                    break;
                default:
                    break;
            }
            spIcon = GeneralEnumTools.GetGeneralSprite(itemsInfo, iconDataManager, gameItemsManager, characterDressManager, false);
        }
        if (spIcon != null)
            ivIcon.color = new Color(1, 1, 1, 1);
        else
            ivIcon.color = new Color(1, 1, 1, 0);
        ivIcon.sprite = spIcon;

        if (rtIcon != null)
        {
            rtIcon.offsetMin = offsetMin;
            rtIcon.offsetMax = offsetMax;
        }
    }

    /// <summary>
    /// 设置名字
    /// </summary>
    /// <param name="name"></param>
    public void SetName(string name)
    {
        if (tvName != null)
            tvName.text = name;
    }

    /// <summary>
    /// 设置数量
    /// </summary>
    /// <param name="number"></param>
    public void SetNumber(long number)
    {
        if (tvNumber != null)
            tvNumber.text = "x" + number;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            leftClick.Invoke();
        else if (eventData.button == PointerEventData.InputButton.Right)
            rightClick.Invoke();
    }

    public virtual void ButtonClick()
    {
        if (itemsInfoBean == null)
            return;
        if (popupItemsSelection != null)
            popupItemsSelection.SetCallBack(this);
        switch (itemsInfoBean.GetItemsType())
        {
            case GeneralEnum.Menu:
                popupItemsSelection.Open(PopupItemsSelection.SelectionTypeEnum.UseAndDiscard);
                break;
            default:
                popupItemsSelection.Open(PopupItemsSelection.SelectionTypeEnum.Discard);
                break;
        }
    }

    #region 选择回调
    public virtual void SelectionUse(PopupItemsSelection view)
    {
        GameDataManager gameDataManager = uiGameManager.gameDataManager;
        ToastManager toastManager = uiGameManager.toastManager;

        if (itemsInfoBean == null || itemBean == null || gameDataManager == null)
            return;
        switch (itemsInfoBean.GetItemsType())
        {
            case GeneralEnum.Menu:
                //添加菜谱
                if (gameDataManager.gameData.AddFoodMenu(itemsInfoBean.add_id))
                {
                    RefreshItems(itemsInfoBean.id,-1);
                    toastManager.ToastHint(ivIcon.sprite, GameCommonInfo.GetUITextById(1006));
                }
                else
                {
                    toastManager.ToastHint(GameCommonInfo.GetUITextById(1007));
                };
                break;
            default:
                break;
        }
    }

    public virtual void SelectionDiscard(PopupItemsSelection view)
    {
        DialogManager dialogManager = uiGameManager.dialogManager;
        if (dialogManager == null || itemsInfoBean == null)
            return;
        DialogBean dialogBean = new DialogBean
        {
            content = string.Format(GameCommonInfo.GetUITextById(3001), itemsInfoBean.name)
        };
        dialogManager.CreateDialog(DialogEnum.Normal, this, dialogBean);
    }

    public virtual void SelectionEquip(PopupItemsSelection view)
    {

    }

    public virtual void SelectionUnload(PopupItemsSelection view)
    {

    }

    public virtual void SelectionGift(PopupItemsSelection view)
    {

    }
    #endregion

    #region 删除确认回调
    public void Submit(DialogView dialogView, DialogBean dialogData)
    {
        RefreshItems(itemsInfoBean.id, -1);
    }

    public void Cancel(DialogView dialogView, DialogBean dialogData)
    {

    }
    #endregion

    /// <summary>
    /// 删除物品
    /// </summary>
    public void RefreshItems(long id, int changeNumber)
    {
        GameDataManager gameDataManager = uiGameManager.gameDataManager;
        ItemBean itemData = gameDataManager.gameData.AddItemsNumber(id, changeNumber);
        SetNumber(itemData.itemNumber);
        if (itemData.itemNumber <= 0)
        {
            //gameObject.transform.DOLocalMove(new Vector3(0, 0), 0.1f).SetEase(Ease.InCirc).OnComplete(delegate
            //{
            //    Destroy(gameObject);
            //});
            Destroy(gameObject);
        }
    }

}