using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [Header("Search")]
    [SerializeField] float searchRadius;
    [SerializeField] LayerMask searchMask;
    [SerializeField] float attackRadius;

    [Header("Attack")]
    [SerializeField] float attackRate;
    [SerializeField] float attackPower;

    [Header("etc")]
    [SerializeField] float moveSpeed;
    [SerializeField] float Hp;

    private Tower target = null;
    private float nextAttackTime = 0.0f;
    float distanceBetween;

    void Update()
    {
        if(target!=null)
            distanceBetween = Vector3.Distance(target.transform.position, transform.position);

        if (target == null)
            SearchTower();
        else if (distanceBetween > attackRadius && distanceBetween <= searchRadius)
            MoveTo();
        else if (distanceBetween <= attackRadius)
            AttackTower();
    }


    private void SearchTower()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, searchRadius, searchMask);
        if (targets.Length > 0)
        {
            Collider pick = targets[Random.Range(0, targets.Length - 1)];
            Tower tower = pick.GetComponent<Tower>();

            if (tower != null)
                target = tower;
            Debug.Log($"타겟: {target}");
        }
    }

    private void MoveTo()
    {
        transform.position = Vector3.MoveTowards(transform.position, 
            target.transform.position, moveSpeed * Time.deltaTime);
    }

    private void AttackTower()
    {
        if (nextAttackTime <= Time.time)
        {
            nextAttackTime = Time.time + attackRate;
            target.OnDamaged(attackPower);
        }
    }

    public void OnDamaged(float damaged)
    {
        Debug.Log($"타워데미지: {damaged}");
        Hp -= damaged;

        if (Hp <= 0)
            OnDead();
    }

    private void OnDead()
    {
        Destroy(gameObject);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        UnityEditor.Handles.color = Color.green;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, searchRadius);

        UnityEditor.Handles.color = Color.green;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, attackRadius);
    }
#endif

}
