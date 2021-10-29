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
            if (destWall == true)
            {
                if (targetWall != null)
                    distanceBetween = Vector3.Distance(targetWall.transform.position, transform.position);
            }
            else
                distanceBetween = Vector3.Distance(targetTower.transform.position, transform.position);
        }

        if (targetTower == null)
            SearchTower();
        else if (distanceBetween <= attackRadius)
        {
            Debug.Log("1111111");

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
        navi.CalculatePath(targetTower.transform.position, pathToTower);    //Ÿ������ ���� ��ȿ���� �˻�
        //ó�� �������� Ÿ���� ����
        if (destTower == false && destWall == false)
        {
            navi.SetPath(pathToTower);
            Debug.Log("2");
            destTower = true;
            destWall = false;
        }
        if (destTower && targetTower!=null)
        {
            navi.CalculatePath(targetTower.transform.position, pathToTower);    //Ÿ������ ���� ��ȿ���� �˻�
            navi.SetPath(pathToTower);
        }
        if (destWall &&targetWall!=null)
        {
            navi.CalculatePath(targetWall.transform.position, pathToTower);    //Ÿ������ ���� ��ȿ���� �˻�
            navi.SetPath(pathToTower);
        }

        if (pathToTower.status != NavMeshPathStatus.PathComplete && !destWall)     //Ÿ������ ���� �� ��ã���� �� ��ó���� ������ ��Ž��
        {
            if (navi.path.corners.Length == 2) {        //[0]�� ������Ʈ ��ġ [1]�� ������

                if (Vector3.Distance(navi.path.corners[1], transform.position) < attackRadius 
                    && Vector3.Distance(navi.path.corners[1], transform.position)>1f)
                {

                    destTower = false;
                    destWall = true;
                    SearchWall();

                    navi.CalculatePath(targetWall.transform.position, pathToTower);    //Ÿ������ ���� ��ȿ���� �˻�
                    navi.SetPath(pathToTower);
                }
            }
        }
  
        //if (navi.path.corners.Length > 1 && destTower == true)                      //Ÿ���˻������� ��� Ÿ������ �ٰ��� �� ������ �μ��� �Ѵ�.
        //{
        //    for (int i = 0; i < navi.path.corners.Length; i++)
        //    {
        //        Debug.Log($"�Ÿ�����: {Vector3.Distance(transform.position, navi.path.corners[i])}");
        //        if (Vector3.Distance(transform.position, navi.path.corners[i]) > searchTowerRadius + 1f)   //�̼��� ���� ������ ���ֱ� ���� 1�� ���Ѵ�
        //        {
        //            Debug.Log($"�ڳʱ���: {navi.path.corners.Length}");
        //            Debug.Log($"i: {i}");

        //            Debug.Log($"Ÿ������: {searchTowerRadius}");

        //            SearchWall();
        //            navi.CalculatePath(targetWall.transform.position, pathToTower);    //Ÿ������ ���� ��ȿ���� �˻�
        //            navi.SetPath(pathToTower);
        //            destTower = false;
        //            destWall = true;
        //        }
        //    }
        //}
        DrawLine();
    }

    private void DrawLine()
    {
        // ���� ������ �׸���.        
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
