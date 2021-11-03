using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//기존바닥 깔기
//벽타일은 다른걸로
//벽은 빌드에서 무시

public class Unit : MonoBehaviour
{
    [Header("Search Tower")]
    [SerializeField] float searchTowerRadius;
    [SerializeField] LayerMask searchTowerMask;

    [Header("Search Wall")]
    [SerializeField] float searchWallDistance;
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
    float speed;            //초기값 저장

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

        speed = navi.speed;

        RePathManager.Instance.RegestedPath(MoveTo);
    }

    private void OnDestroy()
    {
        RePathManager.Instance.RemovePath(MoveTo);
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

        //길이 검색되었을 때 벽 탐색할수 있게 설정
        //Length 1이상이란 것은 최소한 현재위치와 도착지가 설정된 것
        if(navi.path.corners.Length > 1)
            SearchWall();

        //타워탐색
        if (targetTower == null)
        {
            destWall = false;
            destTower = false;
            SearchTower();
        }
        //공격범위내에 공격대상이 있을 때
        else if (distanceBetween <= attackRadius)
        {
            if (destTower)
                AttackTower();
            if (destWall)
                AttackWall();
        }
        //아닐경우 기존의 스피드를 재갱신
        //공격한다고 멈춰있을수도 있기 때문
        else
        {
            navi.speed = speed;
            DrawLine();
        }
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
            {
                targetTower = tower;
                MoveTo();
            }
        }
        else
            searchTowerRadius = Mathf.Clamp(searchTowerRadius *= 1.2f, attackRate, maxSearchRadius);
    }

    private void SearchWall()
    {
        Vector3 rot;
        Vector3 pivot;
        RaycastHit hit;

        //가는길 전체 레이를 발사하여 가로막는 벽 탐색
        for(int i=0; i< navi.path.corners.Length - 1; i++)
        {
            rot = navi.path.corners[i+1] - navi.path.corners[i];
            pivot = navi.path.corners[i];
            float pathDistance = Vector3.Distance(navi.path.corners[i + 1], navi.path.corners[i]);

            //벽이 검색되었을 떄는 공격타겟을 벽으로 설정
            if (Physics.Raycast(pivot, rot.normalized, out hit, pathDistance, searchWallMask))
            {
                Wall wall = hit.collider.gameObject.GetComponent<Wall>();
                if (wall != null)
                {
                    targetWall = wall;
                    destWall = true;
                    destTower = false;
                }
                break;
            }
            //검색안될 시 벽이 없는 것으로 판단 
            else
            {
                destTower = true;
                destWall = false;
            }
            Debug.DrawRay(pivot, rot.normalized * pathDistance, Color.red);
        }
    }

    //타워로 이동하기 위한 함수
    private void MoveTo()
    {
        isMove = true;
        anim.SetBool("isMove", isMove);
        navi.speed = speed;

        destTower = true;
        destWall = false;

        StartCoroutine(SetDest());
    }

    //while문을 쓰기 위한 코루틴
    //극 초반에는 길이 탐색이 되지않아 Length가 0이 나온다
    //도착지와 목적지가 설정될 때 그 길을 탐색하기 위함
    IEnumerator SetDest()
    {
        while (true)
        {
            //타워가 없을 때를 대비
            if (targetTower == null)
                break;

            navi.CalculatePath(targetTower.transform.position,path);
            navi.SetPath(path);

            if (navi.path.corners.Length > 1)
            {
                Debug.Log("Now");
                break;
            }
            yield return null;
        }
        yield return null;
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
        navi.speed = 0f;

        searchTowerRadius = 5f;

        if (nextAttackTime <= Time.time && targetTower != null)
        {
            anim.SetTrigger("onAttack");
            nextAttackTime = Time.time + attackRate;
            destTower = targetTower.OnDamaged(attackPower);
            if (!destTower)
                navi.speed = speed;
        }

    }
    private void AttackWall()
    {
        isMove = false;
        anim.SetBool("isMove", isMove);
        navi.speed = 0f;

        searchTowerRadius = 5f;

        if (nextAttackTime <= Time.time && targetWall != null)
        {
            anim.SetTrigger("onAttack");
            nextAttackTime = Time.time + attackRate;
            destWall = targetWall.OnDamaged(attackPower);
            if (!destWall)
                navi.speed = speed;
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
        Destroy(gameObject);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        UnityEditor.Handles.color = Color.green;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, searchTowerRadius);

        UnityEditor.Handles.color = Color.green;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, attackRadius);
    }
#endif
}
