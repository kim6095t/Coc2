using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MapManager : Singletone<MapManager>
{
	public string TAG_PLAYABLE = "Playable";

	[SerializeField]
	private GameObject _mapPrefab;
	private Vector3 _generatePos = new Vector3(50, 0, 50);
	bool isBake;
	NavMeshSurface[] surfaces;


	private void Awake()
	{
		base.Awake();
		//����ŷ �� ���� �����ϰ� �����.
		GameObject obj = Instantiate(_mapPrefab, _generatePos, Quaternion.identity, transform);
		_generatePos += new Vector3(50, 0, 50);
		surfaces = gameObject.GetComponentsInChildren<NavMeshSurface>();

		StartCoroutine(GenerateNavmesh());
	}

	IEnumerator GenerateNavmesh()
	{
		foreach (var s in surfaces)
		{
			s.RemoveData();
			s.BuildNavMesh();
			yield return null;
		}
		isBake = false;
		yield return null;
	}
	public void ReBake()
	{
		//����ũ ���� ���� ����ũ ���Ұ�
		if (!isBake)
		{
			isBake = true;
			StartCoroutine(GenerateNavmesh());
		}
	}
}