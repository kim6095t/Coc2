using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    [Header("Search Tower")]
    [SerializeField] float searchTowerRadius;
    [SerializeField] LayerMask searchTowerMask;

    [Header("Search Wall")]
    [SerializeField] float searchWallRadius;
    [SerializeField] LayerMask searchWallMask;

    [Header("Attack")]
    [SerializeField] float attackRadius;
    [SerializeField] float attackRate;
    [SerializeField] float attackPower;

    [Header("etc")]
    [SerializeField] float moveSpeed;
    [SerializeField] float Hp;


    private Tower targetTower = null;
    private Wall targetWall = null;
    private float nextAttackTime = 0.0f;
    float distanceBetween;
    Animator anim;
    NavMeshAgent agent;
    LineRenderer line;

    bool isMove;
    bool destTower;
    bool destWall;

    private void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        line = GetComponent<LineRenderer>();
        agent.enabled = false;
        agent.enabled = true;
    }

    void Update()
    {
        if (targetTower != null && destTower)
        {
            distanceBetween = Vector3.Distance(targetTower.transform.position, transform.position);
            Debug.Log($"Tower: {agent.SetDestination(targetTower.transform.position)}");
            Debug.Log($"Wall: {agent.SetDestination(targetWall.transform.position)}");
        }
        if (targetWall != null && destWall)
            distanceBetween = Vector3.Distance(targetWall.transform.position, transform.position);

        if (targetTower == null)
        {
            SearchTower(); 
            SearchWall();
        }
        else if (distanceBetween <= attackRadius)
        {
            Debug.Log("1");
            if (agent.SetDestination(targetTower.transform.position))
            {
                Debug.Log("2");
                AttackTower();
            }
            if (agent.SetDestination(targetWall.transform.position))
            {
                Debug.Log("3");
                AttackWall();
            }
        }
        else
        {
            MoveTo();
        }

    }

    private void SearchTower()
    {

        Collider[] targets = Physics.OverlapSphere(transform.position, searchTowerRadius, searchTowerMask);
        if (targets.Length > 0)
        {
            Collider pick = targets[0];
            Tower tower = pick.GetComponent<Tower>();

            if (tower != null)
                targetTower = tower;
        }
        else
        {
            searchTowerRadius = Mathf.Clamp(searchTowerRadius *= 1.2f, attackRate, 50);
        }

        anim.SetBool("isMove", isMove);
    }

    private void SearchWall()
    {

        Collider[] targets = Physics.OverlapSphere(transform.position, searchWallRadius, searchWallMask);
        if (targets.Length > 0)
        {
            Collider pick = targets[0];
            Wall Wall = pick.GetComponent<Wall>();

            if (Wall != null)
                targetWall = Wall;
        }
        else
        {
            searchWallRadius = Mathf.Clamp(searchWallRadius *= 1.2f, attackRate, 50);
        }
        anim.SetBool("isMove", isMove);
    }


    private void MoveTo()
    {
        isMove = true;
        anim.SetBool("isMove", isMove);
   
        agent.SetDestination(targetTower.transform.position);

        if (agent.path.corners.Length > 1)      //자기 위치를 제외하고 코너의값을 들고있을 시
        {
            for (int i = 0; i < agent.path.corners.Length; i++)
            {
                if (Vector3.Distance(transform.position, agent.path.corners[i]) > searchTowerRadius)
                    agent.SetDestination(targetWall.transform.position);
            }
        }

        // 라인 렌더러 그리기.        
        line.positionCount = agent.path.corners.Length;
        line.SetPositions(agent.path.corners);
        line.startColor = (targetTower == null) ? Color.green : Color.red;
        line.endColor = (targetTower == null) ? Color.green : Color.red;  
    }

    private void AttackTower()
    {
        isMove = false;
        anim.SetBool("isMove", isMove);
        searchTowerRadius = attackRadius;

        if (nextAttackTime <= Time.time && targetTower != null)
        {
            anim.SetTrigger("onAttack");
            nextAttackTime = Time.time + attackRate;
            targetTower.OnDamaged(attackPower);
        }

    }
    private void AttackWall()
    {
        isMove = false;
        anim.SetBool("isMove", isMove);
        searchWallRadius = attackRadius;

        if (nextAttackTime <= Time.time && targetWall != null)
        {
            anim.SetTrigger("onAttack");
            nextAttackTime = Time.time + attackRate;
            targetWall.OnDamaged(attackPower);
        }
    }

    public void OnDamaged(float damaged)
    {
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
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, searchTowerRadius);

        UnityEditor.Handles.color = Color.blue;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, searchWallRadius);

        UnityEditor.Handles.color = Color.green;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, attackRadius);
    }
#endif

}
