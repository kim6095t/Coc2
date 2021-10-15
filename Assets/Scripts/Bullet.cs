using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform pivot;
    private Unit target;
    private float moveSpeed;
    private float power;

    public void Shoot(Unit target, float moveSpeed, float attackPower)
    {
        pivot = transform;

        this.target = target;
        this.moveSpeed = moveSpeed;
        this.power = attackPower;
    }

    private void Update()
    {
        if (target == null)
        {
            Crushed();
            return;
        }

        MoveTo();
    }

    void MoveTo()
    {
        Vector3 direction = target.transform.position - pivot.position;
        Quaternion lookAt = Quaternion.LookRotation(direction);

        Vector3 hitPoint= target.transform.position;
        hitPoint.y = pivot.transform.position.y;

        pivot.position = Vector3.MoveTowards(pivot.position, hitPoint, moveSpeed * Time.deltaTime);
        pivot.rotation = Quaternion.Lerp(pivot.rotation, lookAt, 10f * Time.deltaTime);

        // 타겟과 나의 거리가 (많이) 가까워 졌다면.
        if (Vector3.Distance(pivot.position, hitPoint) <= float.Epsilon)
        {
            HitTarget();
            Crushed();
        }
    }

    void HitTarget()
    {
        target.OnDamaged(power);
    }
    void Crushed()
    {
        Destroy(gameObject);
    }
}
