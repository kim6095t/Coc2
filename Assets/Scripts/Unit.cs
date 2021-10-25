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

    Vector3 fstPosition;
    Vector3 lstPosition;
    
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
        if (targetTower != null && targetWall != null)
        {
            if (destWall != true)
                distanceBetween = Vector3.Distance(targetTower.transform.position, transform.position);
            else
                distanceBetween = Vector3.Distance(targetWall.transform.position, transform.position);
        }

        if (targetWall == null)
            SearchWall();
        else if (targetTower == null)
            SearchTower();
        else if (distanceBetween <= attackRadius)
        {
            if (destTower)
                AttackTower();
            if (destWall)
                AttackWall();
        }
        else
            MoveTo();
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
            searchTowerRadius = Mathf.Clamp(searchTowerRadius *= 1.2f, attackRate, 100);
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
            searchWallRadius = Mathf.Clamp(searchWallRadius *= 1.2f, attackRate, 100);
        }
        
        anim.SetBool("isMove", isMove);
    }


    private void MoveTo()
    {
        isMove = true;
        anim.SetBool("isMove", isMove);

        //검색된 타워위치로 목적지 설정
        //목적지가 wall이면 넘어간다
        if (destWall == false)
        {
            destTower = true;
            destWall = false;
            agent.SetDestination(targetTower.transform.position);
        }

        //타워경로중 탐색범위를 벗어나면 벽을 부순다.
        if (agent.path.corners.Length > 1 && destWall == false)
        {
            for (int i = 0; i < agent.path.corners.Length; i++)
            {
                if (Vector3.Distance(transform.position, agent.path.corners[i]) > searchTowerRadius)
                {
                    Debug.Log("1");
                    agent.SetDestination(targetWall.transform.position);
                    destTower = false;
                    destWall = true;
                }
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
        searchTowerRadius = 5f;
        searchWallRadius = 0f;

        if (nextAttackTime <= Time.time && targetTower != null)
        {
            anim.SetTrigger("onAttack");
            nextAttackTime = Time.time + attackRate;
            destTower = targetTower.OnDamaged(attackPower);
            if (!destTower)
            {
                Debug.Log("2");
                destTower = false;
                destWall = false;
                targetTower = null;
                targetWall = null;
            }
        }

    }
    private void AttackWall()
    {
        isMove = false;
        anim.SetBool("isMove", isMove);
        searchTowerRadius = 5f;
        searchWallRadius = 0f;

        if (nextAttackTime <= Time.time && targetWall != null)
        {
            anim.SetTrigger("onAttack");
            nextAttackTime = Time.time + attackRate;
            destWall = targetWall.OnDamaged(attackPower);
            if (!destWall)
            {
                Debug.Log("3");
                destTower = false;
                destWall = false;
                targetTower = null;
                targetWall = null;
            }
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
