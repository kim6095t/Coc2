using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RePathManager : Singletone<RePathManager>
{
    public delegate void RePathEvent();                  // ��������Ʈ ����.
    event RePathEvent OnRePath;                           // �̺�Ʈ �Լ� ����.
    
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
