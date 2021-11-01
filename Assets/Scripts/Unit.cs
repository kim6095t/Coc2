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
    [SerializeField] float Hp;


    private Tower targetTower = null;
    private Wall targetWall = null;
    private float nextAttackTime = 0.0f;
    float maxSearchRadius = 100f;
    float distanceBetween;

    Animator anim;
    NavMeshAgent navi;
    NavMeshPath path;


    LineRenderer line;

    bool isMove;
    bool destTower;
    bool destWall;

    private void Start()
    {
        anim = GetComponent<Animator>();
        navi = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
        line = GetComponent<LineRenderer>();

        navi.enabled = false;
        navi.enabled = true;

    }

    void Update()
    {
        //벽이나 타워간의 거리 측정
        if (targetTower != null)
        {
            if (destWall == true){
                if (targetWall != null)
                    distanceBetween = Vector3.Distance(targetWall.transform.position, transform.position);
            }
            else
                distanceBetween = Vector3.Distance(targetTower.transform.position, transform.position);
        }
  
        if (targetWall == null)
            destWall = false;

        if (targetTower == null){
            destTower = false;
            SearchTower();
        }
        //공격범위내에 공격대상이 있을 때
        else if (distanceBetween <= attackRadius){
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
        
        //가장 가까이 있는 타워를 타겟으로 지정
        if (targets.Length > 0)
        {
            Collider pick = targets[0];
            Tower tower = pick.GetComponent<Tower>();
            if (tower != null)
                targetTower = tower;
        }
        else
            searchTowerRadius = Mathf.Clamp(searchTowerRadius *= 1.2f, attackRate, maxSearchRadius);
    }

    //수정 필요
    //현재-> 탐지된 벽들 중 가장 가까이 있는 벽을 탐색한다
    //바꾸고자하는 방향-> 길을 방해하는 벽을 탐색하여 효율적으로 제거
    private void SearchWall()
    {
        //가장 가까이 있는 벽을 타겟으로 지정
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
    }

    private void MoveTo()
    {
        isMove = true;

        anim.SetBool("isMove", isMove);
        navi.CalculatePath(targetTower.transform.position, path);    //타워까지 길이 유효한지 검사

        //처음 도착지를 타워로 지정
        if (destTower == false && destWall == false)
        {
            navi.SetPath(path);
            destTower = true;
            destWall = false;
        }
        //타워가 도착지일 때 타워로 가라고 길을 잡아준다
        if (destTower && targetTower!=null)     
        {
            navi.CalculatePath(targetTower.transform.position, path);
            navi.SetPath(path);

            //타워탐색 범위보다 더 넓게 길을 돌아 갈 때.
        //    for (int i = 0; i < navi.path.corners.Length; i++)
        //    {
        //        if (Vector3.Distance(transform.position, navi.path.corners[i]) > searchTowerRadius)
        //        {
        //            SearchWall();
        //            destWall = true;
        //            destTower = false;
        //        }
        //    }
        //
        }

        //벽이 도착지일 때 벽으로 가라고 길을 잡아준다
        if (destWall &&targetWall!=null)        
        {
            navi.SetDestination(targetWall.transform.position);
        }
        //타워까지 가는 길 찾지 못하였을 때 근처까지 와서 부술 벽탐색
        if (path.status != NavMeshPathStatus.PathComplete && !destWall)     
        {
            //Length가 2일 때 [0]은 오브젝트 위치 [1]은 도착지
            if (navi.path.corners.Length == 2) {      
                if (Vector3.Distance(navi.path.corners[1], transform.position) < attackRadius 
                    && Vector3.Distance(navi.path.corners[1], transform.position)>1f)
                {
                    destTower = false;
                    destWall = true;
                    SearchWall();       //부술 벽 탐색
                    navi.CalculatePath(targetWall.transform.position, path);
                    navi.SetPath(path);
                }
            }
        }
        DrawLine();
    }

    // 라인 렌더러 그리기.  
    private void DrawLine()
    {
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
