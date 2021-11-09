using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RePathManager : Singletone<RePathManager>
{
    public delegate void RePathEvent();                  // 델리게이트 정의.
    event RePathEvent OnRePath;                           // 이벤트 함수 선언.
    
    public void RegestedPath(RePathEvent OnRePath)
    {
        this.OnRePath += OnRePath;
    }
    public void RemovePath(RePathEvent OnRePath)
    {
        this.OnRePath -= OnRePath;
    }

    public void RePath()
    {
        OnRePath?.Invoke();
    }
}
