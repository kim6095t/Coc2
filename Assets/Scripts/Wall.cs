using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Wall : MonoBehaviour
{
    [SerializeField] float Hp;

    public bool OnDamaged(float damaged)
    {
        Hp -= damaged;

        if (Hp <= 0)
        {
            Destroy(transform.parent.gameObject);
            return false;
        }
        else
            return true;
    }

    private void OnDestroy()
    {
        //예외처리
        if (MapManager.Instance)
        {
            MapManager.Instance.ReBake();
            RePathManager.Instance.RePath();
        }
    }
}
