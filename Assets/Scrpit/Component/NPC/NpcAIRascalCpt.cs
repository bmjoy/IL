﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;

public class NpcAIRascalCpt : BaseNpcAI, ITextInfoView, UIGameText.ICallBack
{
    public enum RascalIntentEnum
    {
        Idle = 0,
        GoToInn = 1,//前往客栈
        WaitingForReply = 2,//等待回复
        MakeTrouble = 3,//闹事
        Fighting = 4,//打架中
        ContinueMakeTrouble=5,//继续闹事
        Leave = 10,//离开
    }

    public RascalIntentEnum rascalIntent = RascalIntentEnum.Idle;

    //检测范围展示
    public GameObject objRascalSpaceShow;
    //打架特效
    public GameObject objFightShow;
    //生命条
    public GameObject objLife;
    public TextMesh tvLife;
    public SpriteRenderer ivLife;

    //下一个移动点
    public Vector3 movePosition;
    //战斗对象
    public BaseNpcAI npcFight;
    //客栈处理
    public InnHandler innHandler;
    //客栈区域数据管理
    public SceneInnManager sceneInnManager;
    private TextInfoController mTextInfoController;

    //想要说的对话
    public List<TextInfoBean> listTextInfoBean;
    //累计增加的好感
    public int addFavorability = 0;

    //角色生命值
    public int characterMaxLife = 10;
    public int characterLife = 10;

    //制造麻烦的时间
    public float timeMakeTrouble = 60;

    private void Start()
    {
        mTextInfoController = new TextInfoController(this, this);
    }

    /// <summary>
    /// 开始作恶
    /// </summary>
    public void StartEvil()
    {
        SetIntent(RascalIntentEnum.GoToInn);
    }

    /// <summary>
    /// 修改生命值
    /// </summary>
    /// <param name="life"></param>
    public int AddLife(int life)
    {
        characterLife += life;
        if (characterLife <= 0)
        {
            characterLife = 0;
            SetIntent(RascalIntentEnum.Leave);
            //随机获取一句喊话
            int shoutId = Random.Range(13201, 13206);
            characterShoutCpt.Shout(GameCommonInfo.GetUITextById(shoutId));
            //快速离开
            characterMoveCpt.SetMoveSpeed(3);
        }
        else if (characterLife > characterMaxLife)
        {
            characterLife = characterMaxLife;
        }
        tvLife.text = characterLife + "/" + characterMaxLife;
        ivLife.transform.localScale = new Vector3((float)characterLife / (float)characterMaxLife, 1, 1);
        return characterLife;
    }

    private void Update()
    {
        switch (rascalIntent)
        {
            case RascalIntentEnum.GoToInn:
                //是否到达目的地
                if (characterMoveCpt.IsAutoMoveStop())
                {
                    //判断是否关门
                    if (innHandler.GetInnStatus() == InnHandler.InnStatusEnum.Close)
                    {
                        SetIntent(RascalIntentEnum.Leave);
                    }
                    else
                    {
                        SetIntent(RascalIntentEnum.WaitingForReply);
                    }
                }
                break;
            case RascalIntentEnum.Leave:
                //到底目的地删除对象
                if (characterMoveCpt.IsAutoMoveStop())
                    Destroy(gameObject);
                break;
        }
    }

    /// <summary>
    /// 设置意图
    /// </summary>
    /// <param name="intentEnum"></param>
    public void SetIntent(RascalIntentEnum intentEnum)
    {
        StopAllCoroutines();
        this.rascalIntent = intentEnum;
        switch (intentEnum)
        {
            case RascalIntentEnum.GoToInn:
                SetIntentForGoToInn();
                break;
            case RascalIntentEnum.WaitingForReply:
                SetIntentForWaitingForReply();
                break;
            case RascalIntentEnum.MakeTrouble:
                SetIntentForMakeTrouble();
                break;
            case RascalIntentEnum.Fighting:
                SetIntentForFighting();
                break;
            case RascalIntentEnum.ContinueMakeTrouble:
                SetIntentForContinueMakeTrouble();
                break;
            case RascalIntentEnum.Leave:
                SetIntentForLeave();
                break;
        }
    }

    /// <summary>
    /// 意图-前往客栈
    /// </summary>
    public void SetIntentForGoToInn()
    {
        //移动到门口附近
        movePosition = innHandler.GetRandomEntrancePosition();
        if (movePosition == null)
            movePosition = Vector3.zero;
        //前往门
        characterMoveCpt.SetDestination(movePosition);
    }

    /// <summary>
    /// 意图-等待恢复
    /// </summary>
    public void SetIntentForWaitingForReply()
    {
        //获取文本信息
        if (characterFavorabilityData.firstMeet)
        {
            //获取第一次对话的文本
            mTextInfoController.GetTextForTalkByFirst(characterFavorabilityData.characterId);
        }
        else
        {
            mTextInfoController.GetTextForTalkByFavorability(characterFavorabilityData.characterId, characterFavorabilityData.favorability);
        }
    }

    /// <summary>
    /// 意图-制造麻烦
    /// </summary>
    public void SetIntentForMakeTrouble()
    {
        //展示生命条
        AddLife(characterMaxLife);

        objLife.SetActive(true);
        objLife.transform.DOScale(new Vector3(0.2f, 0.2f), 0.5f).From().SetEase(Ease.OutBack);
        //展示范围
        objRascalSpaceShow.SetActive(true);
        objRascalSpaceShow.transform.DOScale(new Vector3(0.2f, 0.2f), 0.5f).From().SetEase(Ease.OutBack);
        //闹事人员添加
        innHandler.rascalrQueue.Add(this);
        StartCoroutine(StartMakeTrouble());
    }

    /// <summary>
    /// 意图-打架
    /// </summary>
    public void SetIntentForFighting()
    {
        StopAllCoroutines();
        characterMoveCpt.StopAutoMove();
        objFightShow.SetActive(true);
    }

    /// <summary>
    /// 意图-继续闹事
    /// </summary>
    public void SetIntentForContinueMakeTrouble()
    {
        npcFight = null;
        objFightShow.SetActive(false);
        StartCoroutine(StartMakeTrouble());
    }

    /// <summary>
    /// 意图-离开
    /// </summary>
    public void SetIntentForLeave()
    {
        innHandler.rascalrQueue.Remove(this);
        npcFight = null;
        objLife.SetActive(false);
        objFightShow.SetActive(false);
        objRascalSpaceShow.SetActive(false);
        //随机获取一个退出点
        movePosition = sceneInnManager.GetRandomSceneExportPosition();
        characterMoveCpt.SetDestination(movePosition);
    }

    /// <summary>
    /// 检测
    /// </summary>
    /// <param name="collision"></param>
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (objRascalSpaceShow.activeSelf)
        {
            if (collision.name.Contains("Customer"))
            {
                NpcAICustomerCpt customerCpt = collision.GetComponent<NpcAICustomerCpt>();
                if (customerCpt.customerIntent != NpcAICustomerCpt.CustomerIntentEnum.Leave
                    && customerCpt.customerIntent != NpcAICustomerCpt.CustomerIntentEnum.Want)
                    customerCpt.ChangeMood(-100);
            }
        }
    }

    /// <summary>
    /// 开始制造麻烦
    /// </summary>
    /// <returns></returns>
    public IEnumerator StartMakeTrouble()
    {
        while (rascalIntent == RascalIntentEnum.MakeTrouble|| rascalIntent == RascalIntentEnum.ContinueMakeTrouble)
        {
            movePosition = innHandler.GetRandomInnPositon();
            characterMoveCpt.SetDestination(movePosition);
            //随机获取一句喊话
            int shoutId = Random.Range(13101, 13106);
            characterShoutCpt.Shout(GameCommonInfo.GetUITextById(shoutId));
            yield return new WaitForSeconds(5);
            //时间到了就离开
            timeMakeTrouble-=5;
            if (timeMakeTrouble<=0)
            {
                SetIntent(RascalIntentEnum.Leave);
            }
        }
    }

    #region 对话信息回调
    public void GetTextInfoForLookSuccess(List<TextInfoBean> listData)
    {

    }

    public void GetTextInfoForTalkSuccess(List<TextInfoBean> listData)
    {
        this.listTextInfoBean = listData;
        if (CheckUtil.ListIsNull(listTextInfoBean))
        {
            SetIntent(RascalIntentEnum.Leave);
            return;
        }
        TextInfoBean textInfo = RandomUtil.GetRandomDataByList(listTextInfoBean);
        EventHandler.Instance.EventTriggerForTalk(textInfo.mark_id, this);
    }

    public void GetTextInfoForStorySuccess(List<TextInfoBean> listData)
    {

    }

    public void GetTextInfoFail()
    {

    }
    #endregion

    #region
    public void TextEnd()
    {
        if (addFavorability >= 0)
        {
            SetIntent(RascalIntentEnum.Leave);
        }
        else
        {
            SetIntent(RascalIntentEnum.MakeTrouble);
        }
    }

    public void AddFavorability(long characterId, int favorability)
    {
        addFavorability += favorability;
    }
    #endregion


}