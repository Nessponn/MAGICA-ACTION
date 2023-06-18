using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SingletonMonoBehaviourFast<AudioManager>
{
    public AudioSource BGM_Audio;//BGM専用Audioチャンネル
    public AudioSource SE_Audio;//効果音専用Audioチャンネル

    public AudioClip[] BGM_List;//BGM
    public AudioClip[] SE_List;//効果音

    //曲を鳴らす
    public void Play_BGM(int num)
    {
        BGM_Audio.clip = BGM_List[num];
        BGM_Audio.Play();
    }

    //曲を止める
    public void Stop_BGM()
    {
        BGM_Audio.Stop();
    }

    //効果音を鳴らす（重複あり）
    public void Play_SE(int num)
    {
        SE_Audio.PlayOneShot(SE_List[num]);
    }

    //効果音を鳴らす（重複なし）
    public void PlayWithNotDuplication_SE(int num)
    {
        SE_Audio.clip = SE_List[num];
        SE_Audio.Play();
    }
}
