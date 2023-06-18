using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioUnityWebGL : MonoBehaviour
{
    [SerializeField] private AudioSource m_audioSource;

    private void Update()
    {
        // �܂� Audio ���Đ�����Ă��炸
        // �����炩�̃L�[���}�E�X�{�^���������ꂽ�ꍇ
        if (!m_audioSource.isPlaying && Input.anyKeyDown)
        {
            // Audio ���Đ�����
            m_audioSource.Play();
        }
    }
}
