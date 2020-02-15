﻿using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;

[Serializable]
public class NpcTeamBean : BaseBean
{
    public long team_id;
    public int team_type;
    public string team_leader;
    public string team_members;
    public int team_number;
    public string condition;//出现条件

    public string name;

    /// <summary>
    /// 获取小队所有成员信息
    /// </summary>
    /// <param name="npcInfoManager"></param>
    /// <returns></returns>
    public void GetTeamCharacterData(NpcInfoManager npcInfoManager, out List<CharacterBean> listLeader, out List<CharacterBean> listMembers)
    {
        listLeader = new List<CharacterBean>();
        listMembers = new List<CharacterBean>();
        foreach (long itemId in GetTeamLeaderId())
        {
            CharacterBean characterData = npcInfoManager.GetCharacterDataById(itemId);
            if (characterData != null)
                listLeader.Add(characterData);
        }
        foreach (long itemId in GetTeamMembersId())
        {
            CharacterBean characterData = npcInfoManager.GetCharacterDataById(itemId);
            if (characterData != null)
                listMembers.Add(characterData);
        }
    }

    public long[] GetTeamLeaderId()
    {
        if (team_leader == null)
            return new long[0];
        return StringUtil.SplitBySubstringForArrayLong(team_leader, ',');
    }

    public long[] GetTeamMembersId()
    {
        if (team_members == null)
            return new long[0];
        return StringUtil.SplitBySubstringForArrayLong(team_members, ',');
    }
}