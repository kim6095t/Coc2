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
    Animator anim;

    bool isMove;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if(target!=null)
            distanceBetween = Vector3.Distance(target.transform.position, transform.position);

        if (target == null)
        {
            Debug.Log("1");
            SearchTower();
        }
        else if (distanceBetween <= attackRadius)
        {
            Debug.Log("3");
            AttackTower();
        }
        else
        {
            MoveTo();
        }
    }


    private void SearchTower()
    {

        Collider[] targets = Physics.OverlapSphere(transform.position, searchRadius, searchMask);
        if (targets.Length > 0)
        {
            Collider pick = targets[0];
            Tower tower = pick.GetComponent<Tower>();

            if (tower != null)
                target = tower;
            Debug.Log($"타겟: {target}");
        }
        else
        {
            searchRadius += 0.1f;
        }


    }

    private void MoveTo()
    {
        isMove = true;

        Vector3 direction = (target.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = lookRotation;

        transform.position = Vector3.MoveTowards(transform.position, 
            target.transform.position, moveSpeed * Time.deltaTime);

        anim.SetBool("isMove",isMove);
    }

    private void AttackTower()
    {
        isMove = false;
        anim.SetBool("isMove", isMove);
        searchRadius = attackRadius;

        if (nextAttackTime <= Time.time && target != null)
        {
            anim.SetTrigger("onAttack");
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
        anim.SetTrigger("onDie");
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
