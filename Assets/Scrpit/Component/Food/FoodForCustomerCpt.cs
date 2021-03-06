﻿using UnityEngine;
using UnityEditor;
using DG.Tweening;

public class FoodForCustomerCpt : BaseMonoBehaviour
{
    //食物样式
    public SpriteRenderer srFood;

    public GameObject objBadFood;
    public GameObject objGoodFood;
    public GameObject objPrefectFood;

    //食物数据管理
    protected InnFoodManager innFoodManager;

    public void Awake()
    {
        innFoodManager = Find<InnFoodManager>( ImportantTypeEnum.FoodManager);
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="foodData"></param>
    public void SetData(InnFoodManager innFoodManager,MenuInfoBean foodData,int foodLevel)
    {
        this.innFoodManager = innFoodManager;
        if (foodData != null && innFoodManager != null)
            srFood.sprite = innFoodManager.GetFoodSpriteByName(foodData.icon_key);

        objBadFood.SetActive(false);
        objGoodFood.SetActive(false);
        objPrefectFood.SetActive(false);
        switch (foodLevel)
        {
            case -1:
                objBadFood.SetActive(true);
                break;
            case 0:
                break;
            case 1:
                objGoodFood.SetActive(true);
                break;
            case 2:
                objPrefectFood.SetActive(true);
                break;
        }
    }

    /// <summary>
    /// 吃完食物
    /// </summary>
    public void FinishFood(MenuInfoBean foodData)
    {
        if (foodData != null && innFoodManager != null)
        {
            srFood.sprite = innFoodManager.GetFoodLastSpriteByName(foodData.icon_key);
        }
   
        objBadFood.SetActive(false);
        objGoodFood.SetActive(false);
        objPrefectFood.SetActive(false);

        //食物完结动画
        transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), 0.3f, 10, 1f);
    }

    /// <summary>
    /// 食材创建动画
    /// </summary>
    public void CreateAnim()
    {
        transform.DOKill();
        transform.localScale = new Vector3(1, 1, 1);
        transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f), 0.5f).From().SetEase(Ease.OutBack);
    }
}