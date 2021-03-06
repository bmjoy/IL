﻿using UnityEngine;
using UnityEditor;

public class AudioHandler : BaseHandler
{
    protected AudioListener audioListener;
    protected AudioSource audioSourceForMusic;
    protected AudioSource audioSourceForSound;
    protected AudioSource audioSourceForEnvironment;
    protected AudioManager audioManager;

    public virtual void Awake()
    {
        audioListener = Find<AudioListener>(ImportantTypeEnum.MainCamera);
        audioManager = Find<AudioManager>(ImportantTypeEnum.AudioManager);
        audioSourceForMusic = CptUtil.GetCptInChildrenByName<AudioSource>(Camera.main.gameObject, "Music");
        audioSourceForSound = CptUtil.GetCptInChildrenByName<AudioSource>(Camera.main.gameObject, "Sound");
        audioSourceForEnvironment = CptUtil.GetCptInChildrenByName<AudioSource>(Camera.main.gameObject, "Environment");
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public void InitAudio()
    {
        audioSourceForMusic.volume = GameCommonInfo.GameConfig.musicVolume;
        audioSourceForEnvironment.volume = GameCommonInfo.GameConfig.soundVolume;
    }



    /// <summary>
    ///  循环播放音乐
    /// </summary>
    /// <param name="audioMusic"></param>
    public void PlayMusicForLoop(AudioMusicEnum audioMusic)
    {
        PlayMusicForLoop(audioMusic, GameCommonInfo.GameConfig.musicVolume);
    }

    /// <summary>
    /// 循环播放音乐
    /// </summary>
    /// <param name="audioMusic"></param>
    /// <param name="volumeScale"></param>
    public void PlayMusicForLoop(AudioMusicEnum audioMusic, float volumeScale)
    {
        AudioClip audioClip = null;
        switch (audioMusic)
        {
            case AudioMusicEnum.LangTaoSha:
                audioClip = audioManager.GetMusicClip("music_langtaosha_1");
                break;
            case AudioMusicEnum.YangChunBaiXue:
                audioClip = audioManager.GetMusicClip("music_yangchunbaixue_1");
                break;
        }
        if (audioClip != null)
        {
            audioSourceForMusic.clip = audioClip;
            audioSourceForMusic.volume = volumeScale;
            audioSourceForMusic.loop = true;
            audioSourceForMusic.Play();
        }     
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="sound">音效</param>
    /// <param name="volumeScale">音量大小</param>
    public void PlaySound(AudioSoundEnum sound, Vector3 soundPosition, float volumeScale)
    {
        AudioClip audioClip = null;
        switch (sound)
        {
            case AudioSoundEnum.ButtonForNormal:
                audioClip = audioManager.GetSoundClip("sound_btn_3");
                break;
            case AudioSoundEnum.ButtonForBack:
                audioClip = audioManager.GetSoundClip("sound_btn_2");
                break;
            case AudioSoundEnum.ButtonForHighLight:
                audioClip = audioManager.GetSoundClip("sound_btn_1");
                break;
            case AudioSoundEnum.ButtonForShow:
                audioClip = audioManager.GetSoundClip("sound_btn_6");
                break;
            case AudioSoundEnum.PayMoney:
                audioClip = audioManager.GetSoundClip("sound_pay_1");
                break;
            case AudioSoundEnum.Reward:
                audioClip = audioManager.GetSoundClip("sound_reward_2");
                break;
            case AudioSoundEnum.Thunderstorm:
                audioClip = audioManager.GetSoundClip("sound_thunderstorm_1");
                break;
            case AudioSoundEnum.Damage:
                audioClip = audioManager.GetSoundClip("sound_damage_1");
                break;
            case AudioSoundEnum.CountDownStart:
                audioClip = audioManager.GetSoundClip("sound_countdown_2");
                break;
            case AudioSoundEnum.CountDownEnd:
                audioClip = audioManager.GetSoundClip("sound_countdown_1");
                break;
            case AudioSoundEnum.HitWall:
                audioClip = audioManager.GetSoundClip("sound_hit_1");
                break;
            case AudioSoundEnum.HitCoin:
               // audioClip = audioManager.GetSoundClip("sound_hit_2");
                audioClip = audioManager.GetSoundClip("sound_pay_3");
                break;
            case AudioSoundEnum.ChangeSelect:
                audioClip = audioManager.GetSoundClip("sound_btn_5");
                break;
            case AudioSoundEnum.Shot:
                audioClip = audioManager.GetSoundClip("sound_shot_1");
                break;
            case AudioSoundEnum.GetCard:
                audioClip = audioManager.GetSoundClip("sound_card_1");
                break;
            case AudioSoundEnum.SetCard:
                audioClip = audioManager.GetSoundClip("sound_btn_3");
                break;
            case AudioSoundEnum.CardDraw:
                audioClip = audioManager.GetSoundClip("sound_btn_5");
                break;
            case AudioSoundEnum.CardWin:
                audioClip = audioManager.GetSoundClip("sound_btn_7");
                break;
            case AudioSoundEnum.CardLose:
                audioClip = audioManager.GetSoundClip("sound_hit_1");
                break;
            case AudioSoundEnum.Fight:
                audioClip = audioManager.GetSoundClip("sound_fight_1");
                break;
            case AudioSoundEnum.FightForKnife:
                audioClip = audioManager.GetSoundClip("sound_fight_knife_1");
                break;
            case AudioSoundEnum.UseMedicine:
                audioClip = audioManager.GetSoundClip("sound_medicine_1");
                break;
            case AudioSoundEnum.Show:
                audioClip = audioManager.GetSoundClip("sound_show_1");
                break;
            case AudioSoundEnum.Correct:
                audioClip = audioManager.GetSoundClip("sound_btn_5");
                break;
            case AudioSoundEnum.Error:
                audioClip = audioManager.GetSoundClip("sound_error_1");
                break;
            case AudioSoundEnum.Set:
                audioClip = audioManager.GetSoundClip("sound_set_1");
                break;
            case AudioSoundEnum.Cook:
                audioClip = audioManager.GetSoundClip("sound_cook_1");
                break;
            case AudioSoundEnum.Clean:
                audioClip = audioManager.GetSoundClip("sound_clean_1");
                break;
            case AudioSoundEnum.Eat:
               int eatRandom= Random.Range(0, 2);
                if (eatRandom == 0)
                {
                    audioClip = audioManager.GetSoundClip("sound_eat_1");
                }
                else if (eatRandom == 1)
                {
                    audioClip = audioManager.GetSoundClip("sound_eat_2");
                }
                break;
            case AudioSoundEnum.Lock:
                audioClip = audioManager.GetSoundClip("sound_lock_1");
                break;
            case AudioSoundEnum.Passive:
                audioClip = audioManager.GetSoundClip("sound_passive_1");
                break;
            case AudioSoundEnum.Dice:
                audioClip = audioManager.GetSoundClip("sound_dice_1");
                break;
        }
        if (audioClip != null)
            audioSourceForSound.PlayOneShot(audioClip, volumeScale);
        // AudioSource.PlayClipAtPoint(soundClip, soundPosition,volumeScale);
    }
    public void PlaySound(AudioSoundEnum sound)
    {
        PlaySound(sound, Camera.main.transform.position, GameCommonInfo.GameConfig.soundVolume);
    }
    public void PlaySound(AudioSoundEnum sound, float volumeScale)
    {
        PlaySound(sound, Camera.main.transform.position, volumeScale);
    }
    public void PlaySound(AudioSoundEnum sound, Vector3 soundPosition)
    {
        PlaySound(sound, soundPosition, 1);
    }

    /// <summary>
    /// 播放环境音乐
    /// </summary>
    /// <param name="audioEnvironment"></param>
    public void PlayEnvironment(AudioEnvironmentEnum audioEnvironment)
    {
        AudioClip audioClip = null;
        switch (audioEnvironment)
        {
            case AudioEnvironmentEnum.Rain:
                audioClip = audioManager.GetEnvironmentClip("environment_rain_1");
                break;
            case AudioEnvironmentEnum.Wind:
                audioClip = audioManager.GetEnvironmentClip("environment_wind_1");
                break;
        }
        audioSourceForEnvironment.clip = audioClip;
        audioSourceForEnvironment.Play();
    }
    /// <summary>
    /// 停止播放
    /// </summary>
    public void StopEnvironment()
    {
        audioSourceForEnvironment.clip = null;
        audioSourceForEnvironment.Stop();
    }
    /// <summary>
    /// 暂停
    /// </summary>
    public void PauseEnvironment()
    {
        audioSourceForEnvironment.Pause();
    }
    /// <summary>
    /// 恢复
    /// </summary>
    public void RestoreEnvironment()
    {
        audioSourceForEnvironment.Play();
    }

}