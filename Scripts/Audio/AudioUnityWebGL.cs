using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioUnityWebGL : MonoBehaviour
{
    [SerializeField] private AudioSource m_audioSource;

    private void Update()
    {
        // まだ Audio が再生されておらず
        // かつ何らかのキーかマウスボタンが押された場合
        if (!m_audioSource.isPlaying && Input.anyKeyDown)
        {
            // Audio を再生する
            m_audioSource.Play();
        }
    }
}
