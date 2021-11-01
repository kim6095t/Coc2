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
        //���̳� Ÿ������ �Ÿ� ����
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
        //���ݹ������� ���ݴ���� ���� ��
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
        
        //���� ������ �ִ� Ÿ���� Ÿ������ ����
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

    //���� �ʿ�
    //����-> Ž���� ���� �� ���� ������ �ִ� ���� Ž���Ѵ�
    //�ٲٰ����ϴ� ����-> ���� �����ϴ� ���� Ž���Ͽ� ȿ�������� ����
    private void SearchWall()
    {
        //���� ������ �ִ� ���� Ÿ������ ����
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
        navi.CalculatePath(targetTower.transform.position, path);    //Ÿ������ ���� ��ȿ���� �˻�

        //ó�� �������� Ÿ���� ����
        if (destTower == false && destWall == false)
        {
            navi.SetPath(path);
            destTower = true;
            destWall = false;
        }
        //Ÿ���� �������� �� Ÿ���� ����� ���� ����ش�
        if (destTower && targetTower!=null)     
        {
            navi.CalculatePath(targetTower.transform.position, path);
            navi.SetPath(path);

            //Ÿ��Ž�� �������� �� �а� ���� ���� �� ��.
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

        //���� �������� �� ������ ����� ���� ����ش�
        if (destWall &&targetWall!=null)        
        {
            navi.SetDestination(targetWall.transform.position);
        }
        //Ÿ������ ���� �� ã�� ���Ͽ��� �� ��ó���� �ͼ� �μ� ��Ž��
        if (path.status != NavMeshPathStatus.PathComplete && !destWall)     
        {
            //Length�� 2�� �� [0]�� ������Ʈ ��ġ [1]�� ������
            if (navi.path.corners.Length == 2) {      
                if (Vector3.Distance(navi.path.corners[1], transform.position) < attackRadius 
                    && Vector3.Distance(navi.path.corners[1], transform.position)>1f)
                {
                    destTower = false;
                    destWall = true;
                    SearchWall();       //�μ� �� Ž��
                    navi.CalculatePath(targetWall.transform.position, path);
                    navi.SetPath(path);
                }
            }
        }
        DrawLine();
    }

    // ���� ������ �׸���.  
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
