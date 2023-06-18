using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SingletonMonoBehaviourFast<AudioManager>
{
    public AudioSource BGM_Audio;//BGM��pAudio�`�����l��
    public AudioSource SE_Audio;//���ʉ���pAudio�`�����l��

    public AudioClip[] BGM_List;//BGM
    public AudioClip[] SE_List;//���ʉ�

    //�Ȃ�炷
    public void Play_BGM(int num)
    {
        BGM_Audio.clip = BGM_List[num];
        BGM_Audio.Play();
    }

    //�Ȃ��~�߂�
    public void Stop_BGM()
    {
        BGM_Audio.Stop();
    }

    //���ʉ���炷�i�d������j
    public void Play_SE(int num)
    {
        SE_Audio.PlayOneShot(SE_List[num]);
    }

    //���ʉ���炷�i�d���Ȃ��j
    public void PlayWithNotDuplication_SE(int num)
    {
        SE_Audio.clip = SE_List[num];
        SE_Audio.Play();
    }
}
