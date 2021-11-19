using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyResourceData : Singletone<EnemyResourceData>
{
    [SerializeField] EnemyResourceUI enemyResource;

    public delegate void EnemyResourceDataEvent();         // 델리게이트 정의.
    event EnemyResourceDataEvent DlEnemyResourceData;           // 이벤트 함수 선언.

    [HideInInspector] public float enemyGold;
    [HideInInspector] public float enemyJelly;
    [HideInInspector] public float getGold;
    [HideInInspector] public float getJelly;

    public void RegestedResource(EnemyResourceDataEvent DlEnemyResourceData)
    {
        this.DlEnemyResourceData += DlEnemyResourceData;

        if (MapManager.Instance)
        {
            DlEnemyResourceData?.Invoke();
            enemyResource.OnUpdateResource();
        }
    }
    public void RemoveResource(EnemyResourceDataEvent DlEnemyResourceData)
    {
        this.DlEnemyResourceData -= DlEnemyResourceData;

        if (MapManager.Instance)
        {
            DlEnemyResourceData?.Invoke();
            enemyResource.OnUpdateResource();
        }
    }
}
