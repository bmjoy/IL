﻿using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class ItemTownBankLoansCpt : ItemGameBaseCpt, DialogView.IDialogCallBack
{
    public UserLoansBean loansData;

    public Text tvMoneyS;
    public Text tvRate;
    public Text tvDays;
    public Text tvMoneyForDay;
    public Button btSubmit;

    private void Start()
    {
        if (btSubmit != null)
            btSubmit.onClick.AddListener(OnClickForSubmit);
    }

    public void SetData(UserLoansBean loansData)
    {
        this.loansData = loansData;
        SetLoansMoney(loansData.moneyS);
        SetLoansRate(loansData.loansRate);
        SetLoansDays(loansData.loansDays);
        SetMoneyForDay(loansData.moneySForDay);

    }

    public void SetLoansMoney(long moneys)
    {
        if (tvMoneyS != null)
            tvMoneyS.text = moneys + "";
    }

    public void SetLoansRate(float rate)
    {
        if (tvRate != null)
            tvRate.text = rate * 100 + "%";
    }

    public void SetLoansDays(int day)
    {
        if (tvDays != null)
            tvDays.text = day + GameCommonInfo.GetUITextById(31);
    }

    public void SetMoneyForDay(long moneys)
    {
        if (tvMoneyForDay != null)
            tvMoneyForDay.text = moneys + "";
    }

    public void OnClickForSubmit()
    {
        UIGameManager uiGameManager = uiComponent.GetUIManager<UIGameManager>();
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        GameDataBean gameData = uiGameManager.gameDataManager.gameData;
        if (gameData.listLoans.Count >= gameData.loansNumberLimit)
        {
            uiGameManager.toastManager.ToastHint(GameCommonInfo.GetUITextById(1091));
            return;
        }

        DialogBean dialogData = new DialogBean
        {
            content = string.Format(GameCommonInfo.GetUITextById(3091), tvMoneyS.text, tvDays.text)
        };
        uiGameManager.dialogManager.CreateDialog(DialogEnum.Normal, this, dialogData);

    }

    #region 确认回调
    public void Submit(DialogView dialogView, DialogBean dialogBean)
    {
        UIGameManager uiGameManager = uiComponent.GetUIManager<UIGameManager>();
        GameDataBean gameData = uiGameManager.gameDataManager.gameData;
        if (gameData.AddLoans(loansData))
        {
            gameData.AddMoney(0, 0, loansData.moneyS);
            uiGameManager.toastManager.ToastHint(string.Format(GameCommonInfo.GetUITextById(1092), tvMoneyS.text));
        }
        else
        {
            uiGameManager.toastManager.ToastHint(GameCommonInfo.GetUITextById(1091));
        }
    }

    public void Cancel(DialogView dialogView, DialogBean dialogBean)
    {

    }
    #endregion
}