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
            OnDestroy();
            return false;
        }
        else
            return true;
    }

    private void OnDestroy()
    {
        Destroy(transform.parent.gameObject);
        MapManager.Instance.ReBake();
    }
}
