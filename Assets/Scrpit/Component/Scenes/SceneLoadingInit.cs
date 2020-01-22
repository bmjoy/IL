﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadingInit : MonoBehaviour
{
    private float targetValue;
    private AsyncOperation mOperation;
    
    void Start()
    {
        //启动协程
       StartCoroutine(CoroutineForAsyncLoading());
       // StartCoroutine(prepareTime());
    }
    
    void Update()
    {
        targetValue = mOperation.progress;

        if (mOperation.progress >= 0.9f)
        {
            //mOperation.progress的值最大为0.9
            targetValue = 1.0f;
        }
        if (targetValue.Equals(1))
        {
            //允许异步加载完毕后自动切换场景
            mOperation.allowSceneActivation = true;
        }
    }

    /// <summary>
    /// 异步加载场景
    /// </summary>
    /// <returns></returns>
    IEnumerator CoroutineForAsyncLoading()
    {
        mOperation = SceneManager.LoadSceneAsync(EnumUtil.GetEnumName(GameCommonInfo.ScenesChangeData.loadingScene));
        //阻止当加载完成自动切换
        mOperation.allowSceneActivation = false;
        yield return mOperation;
    }
}



