using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PinchZoom : Singletone<PinchZoom>
{
    float m_fOldToucDis;       
    float m_fFieldOfView;     

    private float moveSpeed;
    private Vector2 nowPos, prePos;
    private Vector3 movePos;

    public bool isObjectMove;
    private bool isMapZoom;

    private void Start()
    {
        m_fFieldOfView = 60f;   // ī�޶��� FieldOfView�� �⺻���� 60���� ���մϴ�.
        m_fOldToucDis = 0f;     // ��ġ ���� �Ÿ��� �����մϴ�.
        moveSpeed = 3f;
    }

    void Update()
    {
        if (Input.touchCount < 2)
            isMapZoom = false;

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            MapZoom();
            MapMove();
        }
    }

    void MapZoom()
    {
        int nTouch = Input.touchCount;
        float m_fToucDis = 0f;
        float fDis = 0f;

        // ��ġ�� �ΰ��̰�, �� ��ġ�� �ϳ��� �̵��Ѵٸ� ī�޶��� fieldOfView�� �����մϴ�.
        if (Input.touchCount == 2 && (Input.touches[0].phase == TouchPhase.Moved || Input.touches[1].phase == TouchPhase.Moved))
        {
            isMapZoom = true;
            m_fToucDis = (Input.touches[0].position - Input.touches[1].position).sqrMagnitude;

            fDis = (m_fToucDis - m_fOldToucDis) * 0.01f;

            // ���� �� ��ġ�� �Ÿ��� ���� �� ��ġ�� �Ÿ��� ���̸� FleldOfView�� �����մϴ�.
            m_fFieldOfView -= fDis;

            // �ִ�� 100, �ּҴ� 20���� ���̻� ���� Ȥ�� ���Ұ� ���� �ʵ��� �մϴ�.
            m_fFieldOfView = Mathf.Clamp(m_fFieldOfView, 30.0f, 60.0f);

            // Ȯ�� / ��Ұ� ���ڱ� �����ʵ��� �����մϴ�.
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, m_fFieldOfView, Time.deltaTime * 5);

            m_fOldToucDis = m_fToucDis;
        }
    }

    void MapMove()
    {
        if (isObjectMove || isMapZoom || Input.touchCount==0)
            return;

        Touch touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
        {
            prePos = touch.position - touch.deltaPosition;
        }
        else if (touch.phase == TouchPhase.Moved)
        {
            nowPos = touch.position - touch.deltaPosition;
            movePos = (Vector3)(prePos - nowPos) * Time.deltaTime * moveSpeed;

            Camera.main.transform.Translate(movePos);

            prePos = touch.position - touch.deltaPosition;
        }

    }
}
