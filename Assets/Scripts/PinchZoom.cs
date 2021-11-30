using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PinchZoom : Singletone<PinchZoom>
{
    float m_OldTouchDistance;
    float m_NowTouchDistance;

    private void Update()
    {
        if(Input.touches.Length > 1)
            MultiTouch();
    }

    void MultiTouch()
    {
        //현재 터치 개수를 받아온다 
        int touchCount = Input.touchCount;
        //터치 Begin에서 터치 개수가 2개면 두 점의 거리를 계산한다. 
        if (touchCount == 2)
            m_OldTouchDistance = Vector2.Distance(Input.touches[0].position, Input.touches[1].position);

        //계속 거리를 계산해서 거리가 짧아지면 줌인, 거리가 멀어지면 줌아웃 한다. 이전 거리 갱신한다. 
        m_NowTouchDistance = Vector2.Distance(Input.touches[0].position, Input.touches[1].position);
        if (m_NowTouchDistance > m_OldTouchDistance)
            ZoomOut();
        else if (m_NowTouchDistance < m_OldTouchDistance)
            ZoomIn();
        m_OldTouchDistance = m_NowTouchDistance;
    }

    // 이곳에서 줌인 줌아웃을 구현합니다.
    void ZoomOut()
    {
        Debug.Log("ZoomOut");
    }
    void ZoomIn()
    {
        Debug.Log("ZoomIn");
    }
}
