﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SceneMainInit : BaseSceneInit
{
    private new void Start()
    {
        base.Start();
        if (gameItemsManager != null)
            gameItemsManager.itemsInfoController.GetAllItemsInfo();
        audioHandler.PlayMusicForLoop( AudioMusicEnum.LangTaoSha,0.5f);
    }
}