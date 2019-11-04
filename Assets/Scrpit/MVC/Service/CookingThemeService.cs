﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class CookingThemeService : BaseMVCService<CookingThemeBean>
{
    public CookingThemeService() : base("cooking_theme", "cooking_theme_details_" + GameCommonInfo.GameConfig.language)
    {

    }

    /// <summary>
    /// 查询所有主题
    /// </summary>
    /// <returns></returns>
    public List<CookingThemeBean> QueryAllTheme()
    {
        return BaseQueryAllData("theme_id");
    }
}