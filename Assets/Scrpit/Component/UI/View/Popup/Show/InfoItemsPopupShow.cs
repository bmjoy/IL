﻿using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class InfoItemsPopupShow : PopupShowView
{
    public Image ivIcon;
    public Text tvName;
    public Text tvContent;
    public Text tvType;

    public GameObject objAttributeContainer;
    public GameObject objAttributeModel;

    public Color colorForAttribute;

    public ItemsInfoBean itemsInfoData;

    protected IconDataManager iconDataManager;

    private void Awake()
    {
        iconDataManager = Find<IconDataManager>( ImportantTypeEnum.UIManager);
    }

    /// <summary>
    /// 设置文本内容
    /// </summary>
    /// <param name="content"></param>
    public void SetData(Sprite spIcon, ItemsInfoBean data)
    {
        if (data == null)
            return;
        this.itemsInfoData = data;
        SetIcon(spIcon);
        SetName(data.name);
        SetContent(data.content);
        SetType(data.items_type);
        SetAttributes(data);
    }

    public void SetName(string name)
    {
        if (tvName != null)
            tvName.text = name;
    }

    public void SetContent(string content)
    {
        if (tvContent != null)
            tvContent.text = content;
    }

    public void SetIcon(Sprite spIcon)
    {
        if (ivIcon != null)
            ivIcon.sprite = spIcon;
    }

    public void SetType(int type)
    {
        string typeStr = "类型：";
        switch (type)
        {
            case 1:
            case 2:
            case 3:
                typeStr += "装备";
                break;
            case 11:
                typeStr += "书籍";
                break;
            case 12:
                typeStr += "料理";
                break;
        }
        if (tvType != null)
            tvType.text = typeStr;
    }

    /// <summary>
    /// 设置属性
    /// </summary>
    /// <param name="data"></param>
    public void SetAttributes(ItemsInfoBean data)
    {
        CptUtil.RemoveChildsByActive(objAttributeContainer);
        CreateItemAttributes("ui_ability_cook",data.add_cook, GameCommonInfo.GetUITextById(1));
        CreateItemAttributes("ui_ability_speed", data.add_speed, GameCommonInfo.GetUITextById(2));
        CreateItemAttributes("ui_ability_account", data.add_account, GameCommonInfo.GetUITextById(3));
        CreateItemAttributes("ui_ability_charm", data.add_charm, GameCommonInfo.GetUITextById(4));
        CreateItemAttributes("ui_ability_force", data.add_force, GameCommonInfo.GetUITextById(5));
        CreateItemAttributes("ui_ability_lucky", data.add_lucky, GameCommonInfo.GetUITextById(6));
    }


    /// <summary>
    /// 创建属性信息
    /// </summary>
    /// <param name="attributes"></param>
    /// <param name="attributesStr"></param>
    private void CreateItemAttributes(string iconKey ,int attributes, string attributesStr)
    {
        if (attributes == 0)
            return;
        GameObject objItem = Instantiate(objAttributeContainer, objAttributeModel);
        ItemBaseTextCpt itemAttributes = objItem.GetComponent<ItemBaseTextCpt>();
        Sprite spIcon = iconDataManager.GetIconSpriteByName(iconKey);
        itemAttributes.SetData(spIcon, colorForAttribute, attributesStr + "+" + attributes, colorForAttribute,"");
    }
}