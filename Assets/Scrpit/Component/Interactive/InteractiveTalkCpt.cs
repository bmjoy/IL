﻿using UnityEngine;
using UnityEditor;

public class InteractiveTalkCpt : BaseInteractiveCpt
{
    private BaseNpcAI mNpcAI;
    public string interactiveContent;

    private EventHandler mEventHandler;

    private void Start()
    {
        mNpcAI = GetComponent<BaseNpcAI>();
        mEventHandler = FindObjectOfType<EventHandler>();
    }

    public override void InteractiveDetection(CharacterInteractiveCpt characterInt)
    {
        if (Input.GetButtonDown(InputInfo.Interactive_E) && mEventHandler != null)
        {
            if (mEventHandler.GetEventStatus() == EventHandler.EventStatusEnum.EventEnd)
            {
                //先改变人物面向
                if (characterInt.transform.position.x>transform.position.x)
                    mNpcAI.SetCharacterFace(2);
                else
                    mNpcAI.SetCharacterFace(1);
                //获取人物信息
                NpcInfoBean npcInfo = mNpcAI.characterData.npcInfoData;
                mEventHandler.EventTriggerForTalk(npcInfo,true);
            }
        }
    }

    public override void InteractiveEnd(CharacterInteractiveCpt characterInt)
    {
        characterInt.CloseInteractive();
    }

    public override void InteractiveStart(CharacterInteractiveCpt characterInt)
    {
        characterInt.ShowInteractive(interactiveContent);
    }
}