using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//기존바닥 깔기
//벽타일은 다른걸로
//벽은 빌드에서 무시

public class Unit : MonoBehaviour
{
    public static string KEY_NAME = "Name";
    public static string KEY_TYPE = "Type";
    public static string KEY_POWER = "Power";
    public static string KEY_ATKSPD = "AttackRate";
    public static string KEY_ATKRANGE = "AttackRadius";
    public static string KEY_SERCHTOWERRANGE = "SearchTowerRadius";
    public static string KEY_SERCHWALLDISTAN = "SearchWallDistance";
    public static string KEY_MOVESPEED = "MoveSpeed";
    public static string KEY_PRICE = "Price";
    public static string KEY_HP = "Hp";

    public enum Unit_TYPE
    {
        None = -1,

        Goblin,
        Archer,
        Warrier,
        Giant,

        Count,
    }

    [SerializeField] Unit_TYPE type;
    [SerializeField] Sprite sprite;
    [SerializeField] LayerMask searchTowerMask;
    [SerializeField] LayerMask searchWallMask;


    public Unit_TYPE Type => type;

    //엑셀에서 가져올 값들
    protected float attackPower;
    protected float attackRate;
    protected float attackRadius;
    protected float searchTowerRadius;
    protected float searchWallDistance;
    protected int moveSpeed;
    protected int price;
    protected float hp;


    //전역 변수로 사용될 리스트
    Tower targetTower = null;
    Wall targetWall = null;
    float nextAttackTime;
    float maxSearchRadius;
    float distanceBetween;
    float speed;            

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
        line = GetComponent<LineRenderer>();
        path = new NavMeshPath();

        navi.enabled = false;
        navi.enabled = true;

        RePathManager.Instance.RegestedPath(DlRePath);

        maxSearchRadius = 100f;
        distanceBetween = maxSearchRadius;
        speed = navi.speed;
    }

    private void OnDestroy()
    {
        RePathManager.Instance.RemovePath(DlRePath);
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


        //타워탐색
        if (targetTower == null)
        {
            destWall = false;
            destTower = false;
            SearchTower();
        }
        //길이 검색되었을 때 벽 탐색할수 있게 설정, 타워가 null이 아닐때 가는길에 벽이 있는지 확인
        //Length 1이상이란 것은 최소한 현재위치와 도착지가 설정된 것
        else if (navi.path.corners.Length > 1 && targetTower!=null)
        {
            SearchWall();
        }


        //공격범위내에 공격대상이 있을 때
        if (distanceBetween <= attackRadius)
        {
            if (destTower)
                AttackTower();
            else if (destWall)
                AttackWall();
            //아무런 도착지가 정해지지않았을때 임의로 도착지 지정
            else
            {
                if (destWall == false)
                    destWall = true;
                else if (destTower == false)
                    destTower = true;
            }
        }
        //공격범위가 아닌 경우는 탐색된 타워로 움직이게 한다
        //벽이 부서졌을때는 RePathManager를 이용하여 재탐색
        //길이 1이하라는 것은 길이 검색 되지 않았다는 것, 길 검색이 되면 그 길을 따라 가게 만들어둠 
        else if(targetTower!=null && navi.path.corners.Length <=1)
        {
            MoveTo();
        }
    }

    public void Setup(UnitData data)
    {
        attackPower = float.Parse(data.GetData(KEY_POWER));
        attackRate = float.Parse(data.GetData(KEY_ATKSPD));
        attackRadius = float.Parse(data.GetData(KEY_ATKRANGE));

        searchTowerRadius=float.Parse(data.GetData(KEY_SERCHTOWERRANGE));
        searchWallDistance=float.Parse(data.GetData(KEY_SERCHWALLDISTAN));

        moveSpeed = int.Parse(data.GetData(KEY_MOVESPEED));
        price = int.Parse(data.GetData(KEY_PRICE));
        //hp = int.Parse(data.GetData(KEY_HP));
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
                destTower = true;
                destWall = false;
                RePathManager.Instance.RePath();
            }
        }
        else
        {
            searchTowerRadius = Mathf.Clamp(searchTowerRadius *= 1.2f, attackRate, maxSearchRadius);
            if(searchTowerRadius>=maxSearchRadius && targetTower==null)
            {
                navi.speed = 0;
                isMove = false;
                anim.SetBool("isMove", isMove);
            }
        }
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

        navi.CalculatePath(targetTower.transform.position, path);
        navi.SetPath(path);

        DrawLine();
    }

    //벽이나 타워가 부서졌을때 길 재 탐색
    private void DlRePath()
    {
        isMove = true;
        anim.SetBool("isMove", isMove);
        navi.speed = speed;

        destTower = true;
        destWall = false;

        if (targetTower != null)
        {
            navi.CalculatePath(targetTower.transform.position, path);
            navi.SetPath(path);
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
        hp -= damaged;

        if (hp <= 0)
            OnDead();
    }

    private void OnDead()
    {
        Destroy(gameObject);
        UnitManager.Instance.maxUnitCount--;
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
