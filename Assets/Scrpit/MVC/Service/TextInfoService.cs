﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class TextInfoService : ScriptableObject
{
    private string mTableName;
    private string mLeftDetailsTableName;

    public List<TextInfoBean> QueryDataByMarkId(TextEnum textEnum, long markId)
    {
        switch (textEnum)
        {
            case TextEnum.Look:
                mTableName = "text_look";
                mLeftDetailsTableName = "text_look_details_" + GameCommonInfo.gameConfig.language;
                break;
            case TextEnum.Talk:
                mTableName = "text_talk";
                mLeftDetailsTableName = "text_talk_details_" + GameCommonInfo.gameConfig.language;
                break;
            case TextEnum.Story:
                mTableName = "text_story";
                mLeftDetailsTableName = "text_story_details_" + GameCommonInfo.gameConfig.language;
                break;
            default:
                return null;
        }
        string[] leftTable = new string[] { mLeftDetailsTableName };
        string[] mainKey = new string[] { "id" };
        string[] leftKey = new string[] { "text_id" };
        string[] colName = new string[] { mTableName + ".mark_id" };
        string[] operations = new string[] { "=" };
        string[] colValue = new string[] { markId + "" };
        return SQliteHandle.LoadTableData<TextInfoBean>(ProjectConfigInfo.DATA_BASE_INFO_NAME, mTableName, leftTable, mainKey, leftKey, colName, operations, colValue);
    }
}