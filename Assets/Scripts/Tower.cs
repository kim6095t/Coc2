using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : ObjectProperty
{
    [Header("Search")]
    [SerializeField] float searchRadius;
    [SerializeField] LayerMask searchMask;


    [Header("Bullet")]
    [SerializeField] float attackPower;
    [SerializeField] float moveSpeed;
    [SerializeField] Bullet bullet;
    [SerializeField] Transform bulletPivot;

    [Header("etc")]
    [SerializeField] float attackRate;
    [SerializeField] Transform pivot;
    [SerializeField] float Hp;


    private float gold;
    private float jelly;
  
    private Unit target = null;
    private float nextAttackTime = 0.0f;

    public void Start()
    {
        base.Start();

        gold = 100f;
        jelly = 100f;
        prefabName = "Tower";

        if (!sceneName.Equals("TownScene"))
            EnemyResourceData.Instance.RegestedResource(DlAddResource);
    }


    void Update()
    {
        if (target == null)
            SearchEnemy();
        else
            AttackEnemy();
    }

    private void SearchEnemy()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, searchRadius, searchMask);
        if (targets.Length > 0)
        {
            Collider pick = targets[Random.Range(0, targets.Length - 1)];
            Unit enemy = pick.GetComponent<Unit>();

            if (enemy != null)
                target = enemy;
        }
    }

    private void AttackEnemy()
    {
        // 공격 직전에 타겟이 없을(죽었을) 경우.
        if (target == null)
            return;

        // 공격 직전 적과 나의 거리가 공격 사거리보다 길 경우.
        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance > searchRadius)
        {
            target = null;
            return;
        }

        Vector3 direction = target.transform.position - pivot.position;     // (회전) 오일러 방향
        direction.Normalize();                                              // 0.0 ~ 1.0f 사이 값으로 정규화.
        Quaternion lookAt = Quaternion.LookRotation(direction);             // (회전) 오일러 -> 쿼터니언
        lookAt.x = 0f;
        lookAt.z = 0f;

        // Lerp : 현재 -> 목적지 값까지 시간의 경과에 따른 사이 값을 준다.
        pivot.rotation = Quaternion.Lerp(pivot.rotation, lookAt, 10f * Time.deltaTime);

        // 타워의 공격.
        if (nextAttackTime <= Time.time)
        {
            nextAttackTime = Time.time + attackRate;

            Bullet newBullet = Instantiate(bullet);
            newBullet.transform.position = bulletPivot.position;
            newBullet.transform.rotation = bulletPivot.rotation;
            newBullet.Shoot(target, moveSpeed, attackPower);
        }
    }
    public bool OnDamaged(float damaged)
    {
        Hp -= damaged;

        if (Hp <= 0)
        {
            Destroy(gameObject);
            return false;
        }
        else
            return true;
    }
    
    private void OnDestroy()
    {
        if (!sceneName.Equals("TownScene"))
        {
            StageClearPersent.Instance.OnDestroyTarget();

            EnemyResourceData.Instance.getGold += gold;
            EnemyResourceData.Instance.getJelly += jelly;

            MyResourceData.Instance.myGold += gold;
            MyResourceData.Instance.myJelly += jelly;
            EnemyResourceData.Instance.RemoveResource(DlRemoveResource);
        }
    }

    public void DlAddResource()
    {
        EnemyResourceData.Instance.enemyGold += gold;
        EnemyResourceData.Instance.enemyJelly += jelly;
    }

    public void DlRemoveResource()
    {
        EnemyResourceData.Instance.enemyGold -= gold;
        EnemyResourceData.Instance.enemyJelly -= jelly;
    }


#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, searchRadius);
    }
#endif
}
