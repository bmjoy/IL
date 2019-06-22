﻿using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIGameAttendance : BaseUIComponent, ItemGameAttendanceCpt.CallBack
{
    public Text tvPriceL;
    public Text tvPriceM;
    public Text tvPirceS;
    public Text tvNumber;

    public Button btSubmit;

    public GameObject objListContent;
    public GameObject objItemWorkModle;

    public ToastView toastView;

    public GameDataManager gameDataManager;
    public GameTimeHandler gameTimeHandler;
    public InnHandler innHandler;
    public ControlHandler controlHandler;

    //出勤金钱
    public long attendancePriceL;
    public long attendancePriceM;
    public long attendancePriceS;

    public int attendanceNumber;//出勤人数

    private void Start()
    {
        if (btSubmit != null)
            btSubmit.onClick.AddListener(StartWork);
        InitData();
    }

    public void StartWork()
    {
        if (attendanceNumber <= 0)
        {
            toastView.ToastHint("至少需要选择1人出勤");
            return;
        }
        if (!gameDataManager.gameData.HasEnoughMoney(attendancePriceL, attendancePriceM, attendancePriceS))
        {
            toastView.ToastHint("没有足够的金钱支付出勤");
            return;
        }
        gameDataManager.gameData.PayMoney(attendancePriceL, attendancePriceM, attendancePriceS);
        gameTimeHandler.dayStauts = GameTimeHandler.DayEnum.Work;
        gameTimeHandler.StartNewDay(false);
        uiManager.OpenUIAndCloseOtherByName("Main");
        innHandler.OpenInn();
        controlHandler.StartControl(ControlHandler.ControlEnum.Work);
    }

    public void InitData()
    {
        if (gameDataManager == null)
            return;
        List<CharacterBean> listData = new List<CharacterBean>();
        listData.Add(gameDataManager.gameData.userCharacter);
        listData.AddRange(gameDataManager.gameData.workCharacterList);
        for (int i = 0; i < listData.Count; i++)
        {
            CharacterBean itemData = listData[i];
            CreateWorkerItem(itemData);
        }
    }

    public void CreateWorkerItem(CharacterBean characterData)
    {
        if (objListContent == null || objItemWorkModle == null)
            return;
        GameObject objWorkerItem = Instantiate(objItemWorkModle, objListContent.transform);
        objWorkerItem.SetActive(true);
        ItemGameAttendanceCpt workerItem = objWorkerItem.GetComponent<ItemGameAttendanceCpt>();
        if (workerItem != null)
        {
            workerItem.SetCallBack(this);
            workerItem.SetData(characterData);
        }
    }

    public void SetTotalData()
    {
        tvPriceL.text = attendancePriceL + "";
        tvPriceM.text = attendancePriceM + "";
        tvPirceS.text = attendancePriceS + "";
        tvNumber.text = "出勤人数：" + attendanceNumber;
    }

    #region  出勤回调
    public void AttendanceChange(ItemGameAttendanceCpt itemView, bool isAttendance, CharacterBean characterBean)
    {
        if (isAttendance)
        {
            attendancePriceL += characterBean.baseInfo.priceL;
            attendancePriceM += characterBean.baseInfo.priceM;
            attendancePriceS += characterBean.baseInfo.priceS;
            attendanceNumber += 1;
        }
        else
        {
            attendancePriceL -= characterBean.baseInfo.priceL;
            attendancePriceM -= characterBean.baseInfo.priceM;
            attendancePriceS -= characterBean.baseInfo.priceS;
            attendanceNumber -= 1;
        }
        if (attendancePriceL < 0)
            attendancePriceL = 0;
        if (attendancePriceM < 0)
            attendancePriceM = 0;
        if (attendancePriceS < 0)
            attendancePriceS = 0;
        if (attendanceNumber < 0)
            attendanceNumber = 0;
        SetTotalData();
    }
    #endregion
}