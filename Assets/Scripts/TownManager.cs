using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TownManager : MonoBehaviour
{
    [SerializeField] Slider volumnSlider;
    private void Start()
    {
        SoundManager.Instance.PlayBGM(SoundManager.BGM.BGM_01);        // 배경음 재생.
        volumnSlider.value = SoundManager.Instance.audioSource.volume;
    }

    public void VolumnChange()
    {
        SoundManager.Instance.audioSource.volume = volumnSlider.value;
        Debug.Log(volumnSlider.value);
    }
}
