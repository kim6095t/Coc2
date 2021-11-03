using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MapManager : Singletone<MapManager>
{
	[SerializeField]
	private GameObject _mapPrefab;
	private Vector3 _generatePos = new Vector3(50, 0, 50);
	bool isBake;

	public void Init()
	{
	}

	private void Awake()
	{
		base.Awake();
		StartCoroutine(GenerateNavmesh());
	}
	

	IEnumerator GenerateNavmesh()
	{
		GameObject obj = Instantiate(_mapPrefab, _generatePos, Quaternion.identity, transform);
		_generatePos += new Vector3(50, 0, 50);

		NavMeshSurface[] surfaces = gameObject.GetComponentsInChildren<NavMeshSurface>();

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
		//베이크 중일 때는 베이크 사용불가
		//게임 종료시에 Instance가 null이 되면서 에러창이 뜬다. 그걸 방지하기 위함.
		if (!isBake && MapManager.Instance!=null)
		{
			isBake = true;
			StartCoroutine(GenerateNavmesh());
		}
	}
}