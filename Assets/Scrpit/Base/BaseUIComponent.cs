﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUIComponent : BaseMonoBehaviour 
{ 
    //UI管理
    public BaseUIManager uiManager;
    //UI动画
    public Animator uiAnimator;
    //备注数据
    public string remarkData;

    protected DialogManager dialogManager;
    public virtual void Awake()
    {
        dialogManager = Find<DialogManager>(ImportantTypeEnum.DialogManager);
        if (uiManager == null)
            uiManager = GetComponentInParent<BaseUIManager>();
        if (uiAnimator == null)
            uiAnimator = GetComponent<Animator>();
    }

    /// <summary>
    /// 开启UI
    /// </summary>
    public virtual void OpenUI()
    {
        if (this.gameObject.activeSelf)
            return;
        this.gameObject.SetActive(true);
        if (uiAnimator != null)
            uiAnimator.SetInteger("UIStates", 1);
        dialogManager.CloseAllDialog();
    }

    /// <summary>
    /// 关闭UI
    /// </summary>
    public virtual void CloseUI()
    {
        if (!this.gameObject.activeSelf)
            return;
        this.gameObject.SetActive(false);
        if (uiAnimator != null)
            uiAnimator.SetInteger("UIStates", 0);
    }

    /// <summary>
    /// 刷新UI
    /// </summary>
    public virtual void RefreshUI()
    {

    }

    /// <summary>
    /// 设置备用数据
    /// </summary>
    /// <param name="remarkData"></param>
    public virtual void SetRemarkData(string remarkData)
    {
        this.remarkData = remarkData;
    }

    public T GetUIManager<T>() where T : BaseUIManager
    {
        return uiManager as T;
    }
}
