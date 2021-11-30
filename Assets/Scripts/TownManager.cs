using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownManager : MonoBehaviour
{
    private void Start()
    {
        SoundManager.Instance.PlayBGM(SoundManager.BGM.Forest1);        // 배경음 재생.
    }
}
