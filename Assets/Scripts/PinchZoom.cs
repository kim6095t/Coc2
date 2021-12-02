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
        m_fFieldOfView = 60f;   // 카메라의 FieldOfView의 기본값을 60으로 정합니다.
        m_fOldToucDis = 0f;     // 터치 이전 거리를 저장합니다.
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

        // 터치가 두개이고, 두 터치중 하나라도 이동한다면 카메라의 fieldOfView를 조정합니다.
        if (Input.touchCount == 2 && (Input.touches[0].phase == TouchPhase.Moved || Input.touches[1].phase == TouchPhase.Moved))
        {
            isMapZoom = true;
            m_fToucDis = (Input.touches[0].position - Input.touches[1].position).sqrMagnitude;

            fDis = (m_fToucDis - m_fOldToucDis) * 0.01f;

            // 이전 두 터치의 거리와 지금 두 터치의 거리의 차이를 FleldOfView를 차감합니다.
            m_fFieldOfView -= fDis;

            // 최대는 100, 최소는 20으로 더이상 증가 혹은 감소가 되지 않도록 합니다.
            m_fFieldOfView = Mathf.Clamp(m_fFieldOfView, 30.0f, 60.0f);

            // 확대 / 축소가 갑자기 되지않도록 보간합니다.
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
