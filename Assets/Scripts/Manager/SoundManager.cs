using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singletone<SoundManager>
{
    public enum BGM
    {
        BGM_01,
    }

    [SerializeField] public AudioSource audioSource;                   // ����Ŀ.
    [SerializeField] AudioClip[] bgms;                          // ������� ����� ����.
    [SerializeField] float bgmVolume;                           // ����� ũ��.

    private void Start()
    {
        audioSource.volume = bgmVolume;             // ����Ŀ ���� ����.
    }

    public void PlayBGM(BGM bgm)
    {
        PlayBGM(bgm.ToString());
    }
    public void PlayBGM(string bgmName)
    {
        for (int i = 0; i < bgms.Length; i++)       // ��� BGM �迭�� ��ȸ.
        {
            if (bgms[i].name == bgmName)             // i��° BGM�� �̸��� ���ٸ�
            {
                audioSource.clip = bgms[i];         // audioSource(����Ŀ)�� clip(CD)�� ����.
                audioSource.Play();                 // ��� ��ư.
                break;
            }
        }
    }
}