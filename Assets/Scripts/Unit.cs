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
    NavMeshAgent navi;
    NavMeshPath pathToTower;


    LineRenderer line;

    bool isMove;
    bool destTower;
    bool destWall;


    private void Start()
    {
        anim = GetComponent<Animator>();
        navi = GetComponent<NavMeshAgent>();
        pathToTower = new NavMeshPath();
        line = GetComponent<LineRenderer>();

        navi.enabled = false;
        navi.enabled = true;

    }

    void Update()
    {
        if (targetTower != null)
        {
            if (destWall != true)
                distanceBetween = Vector3.Distance(targetTower.transform.position, transform.position);
            else
                distanceBetween = Vector3.Distance(targetWall.transform.position, transform.position);
        }

        if (targetTower == null)
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
        destTower = false;
        destWall = false;

        float maxSearchRadius = 100f;
        Collider[] targets = Physics.OverlapSphere(transform.position, searchTowerRadius, searchTowerMask);
        if (targets.Length > 0)
        {
            Collider pick = targets[0];
            Tower tower = pick.GetComponent<Tower>();

            if (tower != null)
                targetTower = tower;
        }
        else
            searchTowerRadius = Mathf.Clamp(searchTowerRadius *= 1.2f, attackRate, maxSearchRadius);
        anim.SetBool("isMove", isMove);
    }

    private void SearchWall()
    {
        float maxSearchRadius = 100f;
        while (searchWallRadius<= maxSearchRadius)
        {
            Collider[] targets = Physics.OverlapSphere(transform.position, searchWallRadius, searchWallMask);
            if (targets.Length > 0)
            {
                Collider pick = targets[0];
                Wall wall = pick.GetComponent<Wall>();

                if (wall != null){
                    targetWall = wall;
                    break;
                }
            }
            else
                searchWallRadius = Mathf.Clamp(searchWallRadius += 0.1f, attackRate, maxSearchRadius);
        }
        anim.SetBool("isMove", isMove);
    }

    private void MoveTo()
    {
        isMove = true;
        anim.SetBool("isMove", isMove);

        //처음 도착지를 타워로 지정
        if (destTower == false && destWall == false)
        {
            navi.SetDestination(targetTower.transform.position);
            destTower = true;
            destWall = false;
        }
        navi.CalculatePath(targetTower.transform.position, pathToTower);    //타워까지 길이 유효한지 검사
        Debug.Log(targetTower.gameObject);
        if (pathToTower.status != NavMeshPathStatus.PathComplete && !destWall)     //타워까지 가는 길 못찾았을 때 근처까지 왔을때 벽탐색
        {
            if (navi.path.corners.Length == 2) {        //[0]은 오브젝트 위치 [1]은 도착지
                if (Vector3.Distance(navi.path.corners[1], transform.position) < attackRadius 
                    && Vector3.Distance(navi.path.corners[1], transform.position)>1f)
                {
                    destTower = false;
                    destWall = true;
                    SearchWall();
                    navi.SetDestination(targetWall.transform.position);
                    Debug.Log("공격");
                }
            }
        }
  
        if (navi.path.corners.Length > 1 && destTower != true)                      //타워검색범위를 벗어나 타워에게 다가갈 때 벽부터 부수게 한다.
        {
            for (int i = 0; i < navi.path.corners.Length; i++)
            {
                if (Vector3.Distance(transform.position, navi.path.corners[i]) > searchTowerRadius + 1f)   //미세한 값의 오차를 없애기 위해 0.5를 더한다
                {
                    Debug.Log(Vector3.Distance(transform.position, navi.path.corners[i]));
                    Debug.Log(searchTowerRadius);

                    SearchWall();
                    navi.SetDestination(targetWall.transform.position);
                    destTower = false;
                    destWall = true;
                }
            }
        }
        DrawLine();
    }

    private void DrawLine()
    {
        // 라인 렌더러 그리기.        
        line.positionCount = navi.path.corners.Length;
        line.SetPositions(navi.path.corners);
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
                destTower = false;
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
