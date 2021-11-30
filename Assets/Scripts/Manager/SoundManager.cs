using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singletone<SoundManager>
{
    public enum BGM
    {
        Forest1,
    }

    [SerializeField] AudioSource audioSource;                   // 스피커.
    [SerializeField] AudioClip[] bgms;                          // 배경음악 오디오 파일.
    [SerializeField] float bgmVolume;                           // 배경음 크기.

    private void Start()
    {
        audioSource.volume = bgmVolume;             // 스피커 사운드 조절.
    }

    public void PlayBGM(BGM bgm)
    {
        PlayBGM(bgm.ToString());
    }
    public void PlayBGM(string bgmName)
    {
        for (int i = 0; i < bgms.Length; i++)       // 모든 BGM 배열을 순회.
        {
            if (bgms[i].name == bgmName)             // i번째 BGM의 이름이 같다면
            {
                audioSource.clip = bgms[i];         // audioSource(스피커)에 clip(CD)를 대입.
                audioSource.Play();                 // 재생 버튼.
                break;
            }
        }
    }
}