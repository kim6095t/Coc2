using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//�����ٴ� ���
//��Ÿ���� �ٸ��ɷ�
//���� ���忡�� ����

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
        path = new NavMeshPath();
        line = GetComponent<LineRenderer>();

        navi.enabled = false;
        navi.enabled = true;

        speed = navi.speed;
    }

    void Update()
    {
        SearchWall();

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
        //���ư��� ����� �ٶ󺸴� ������ ���� �� ���� �ִ��� Ž��
        if (navi.path.corners.Length > 1) {
            SearchWall();
            //Vector3 rot = navi.path.corners[1] - navi.path.corners[0];
            //float rodeRot = Mathf.Atan2(rot.x, rot.z) * Mathf.Rad2Deg;
            //if (rodeRot < 0)
            //    rodeRot = 360 + rodeRot;

            //Debug.Log($"�氢�� :{rodeRot}");
            //Debug.Log($"ĳ���Ͱ��� :{transform.rotation.y}");


            //if (Mathf.Abs(transform.rotation.y - rodeRot) > 0.5)
            //    SearchWall();
        }
        //Ÿ��Ž��
        if (targetTower == null)
            SearchTower();
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

    private void SearchWall()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, searchWallDistance, searchWallMask))
        {
            Wall wall = hit.collider.gameObject.GetComponent<Wall>();
            if (wall != null)
            {
                targetWall = wall;
                destWall = true;
                destTower = false;
            }
        }

        Debug.DrawRay(transform.position, transform.forward * searchWallDistance, Color.red);
    }

    private void MoveTo()
    {
        isMove = true;

        anim.SetBool("isMove", isMove);
        navi.CalculatePath(targetTower.transform.position, path);    //Ÿ������ ���� ��ȿ���� �˻�
        //�������� Ÿ���� ����
        if (destTower == false && destWall == false)
        {
            navi.SetPath(path);
            destTower = true;
            destWall = false;
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
        navi.speed = 0f;

        searchTowerRadius = 5f;

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
                navi.speed = speed;
            }
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
            {
                destTower = false;
                targetTower = null;
                targetWall = null;
                navi.speed = speed;
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

        UnityEditor.Handles.color = Color.green;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, attackRadius);
    }
#endif
}
