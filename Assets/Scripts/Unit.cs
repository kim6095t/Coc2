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
    //NavMeshAgent naviCal;
    NavMeshPath pathToTower;


    LineRenderer line;

    bool isMove;
    bool destTower;
    bool destWall;


    private void Start()
    {
        anim = GetComponent<Animator>();
        navi = GetComponent<NavMeshAgent>();
        //naviCal = GetComponentInChildren<NavMeshAgent>();
        pathToTower = new NavMeshPath();
        line = GetComponent<LineRenderer>();

        navi.enabled = false;
        navi.enabled = true;
       //naviCal.enabled = false;
        //naviCal.enabled = true;
    }

    void Update()
    {
        if (targetTower != null)
        {
            distanceBetween = Vector3.Distance(targetTower.transform.position, transform.position);
            //if (destWall != true)
            //    distanceBetween = Vector3.Distance(targetTower.transform.position, transform.position);
            //else
            //    distanceBetween = Vector3.Distance(targetWall.transform.position, transform.position);
        }
        if (targetTower == null)
            SearchTower();
//        else if (targetWall == null)
//            SearchWall();
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
            searchTowerRadius = Mathf.Clamp(searchTowerRadius *= 1.2f, attackRate, 100);
        anim.SetBool("isMove", isMove);
    }

    private void SearchWall()
    {
        // 1.SetDestination으로 최대한 접근한다.
        // 2 corners(?)의 카운터가 0이 되었다 (=더이상 이동할 수 없다) 라면 대상까지 갈수있는지 Calculate시켜서 확인한다.
        // 3-1. 갈수있는 상태면 공격한다.
        // 3-2. 갈수 없는 상태면 가장 가까운 벽을 부순다. -> 1번

        searchWallRadius = searchTowerRadius;
        Collider[] targets = Physics.OverlapSphere(transform.position, searchWallRadius, searchWallMask);
        Collider pick = targets[0];
        Wall wall = pick.GetComponent<Wall>();
        if (wall != null)
            targetWall = wall;

       
        //for (int i=0; i<targets.Length; i++)
        //{
        //    navi.CalculatePath(targetWall.transform.position, pathToTower);
        //    if (pathToTower.status == NavMeshPathStatus.PathComplete)
        //    {
        //        Debug.Log(targetWall.transform.position);
        //        targetWall = wall;
        //        break;
        //    }
        //    pick = targets[i];
        //    wall = pick.GetComponent<Wall>();
        //}
        anim.SetBool("isMove", isMove);
    }

    private void MoveTo()
    {
        isMove = true;
        anim.SetBool("isMove", isMove);

        navi.SetDestination(targetTower.transform.position);
        Debug.Log(targetTower.transform.position);

        //naviCal.CalculatePath(targetTower.transform.position, pathToTower);           // 타워에게 가는길이 유효한지 검사
        //if (pathToTower.status == NavMeshPathStatus.PathComplete)                  // 유효하다면.
        //{
        //    Debug.Log("유효");
        //    navi.SetDestination(targetTower.transform.position);
        //    destTower = true;
        //    destWall = false;
        //}
        //else{                                                                      // 유효하지 않다면.
        //    Debug.Log("유효하지 않음");
        //    navi.SetDestination(targetWall.transform.position);
        //    destTower = false;
        //    destWall = true;
        //}


        //if (navi.path.corners.Length > 1 && destWall == false)                      //타워검색범위를 벗어나 타워에게 다가갈 때 벽부터 부수게 한다.
        //{
        //    for (int i = 0; i < navi.path.corners.Length; i++)
        //    {
        //        if (Vector3.Distance(transform.position, navi.path.corners[i]) > searchTowerRadius + 0.5)   //미세한 값의 오차를 없애기 위해 0.5를 더한다
        //        {
        //            navi.SetDestination(targetWall.transform.position);
        //            destTower = false;
        //            destWall = true;
        //        }
        //    }
        //}
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
