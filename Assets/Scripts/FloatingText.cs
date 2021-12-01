using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour
{
	[SerializeField] Text FloatTextPrint;
	float moveSpeed;
	float destroyTime;
	Vector3 vector;

	public void print(string Text)
	{
		FloatTextPrint.text = string.Format(" {0}", Text);
	}

	private void Start()
	{
		moveSpeed = 2f; //���� �����̴� �ӵ���
		destroyTime = 0.5f; //���� �� ���� �ɰ���
	}

	void Update()
	{
		vector.Set(FloatTextPrint.transform.position.x, FloatTextPrint.transform.position.y
			+ (moveSpeed + Time.deltaTime), FloatTextPrint.transform.position.z);

		FloatTextPrint.transform.position = vector;
		destroyTime -= Time.deltaTime;

		if (destroyTime <= 0)
		{
			Destroy(this.gameObject);
		}
	}
}
