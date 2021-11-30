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
        //���� ��ġ ������ �޾ƿ´� 
        int touchCount = Input.touchCount;
        //��ġ Begin���� ��ġ ������ 2���� �� ���� �Ÿ��� ����Ѵ�. 
        if (touchCount == 2)
            m_OldTouchDistance = Vector2.Distance(Input.touches[0].position, Input.touches[1].position);

        //��� �Ÿ��� ����ؼ� �Ÿ��� ª������ ����, �Ÿ��� �־����� �ܾƿ� �Ѵ�. ���� �Ÿ� �����Ѵ�. 
        m_NowTouchDistance = Vector2.Distance(Input.touches[0].position, Input.touches[1].position);
        if (m_NowTouchDistance > m_OldTouchDistance)
            ZoomOut();
        else if (m_NowTouchDistance < m_OldTouchDistance)
            ZoomIn();
        m_OldTouchDistance = m_NowTouchDistance;
    }

    // �̰����� ���� �ܾƿ��� �����մϴ�.
    void ZoomOut()
    {
        Debug.Log("ZoomOut");
    }
    void ZoomIn()
    {
        Debug.Log("ZoomIn");
    }
}
