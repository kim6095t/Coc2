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
		GenerateNavmesh();
	}

	private void GenerateNavmesh()
	{
		GameObject obj = Instantiate(_mapPrefab, _generatePos, Quaternion.identity, transform);
		_generatePos += new Vector3(50, 0, 50);

		NavMeshSurface[] surfaces = gameObject.GetComponentsInChildren<NavMeshSurface>();

		foreach (var s in surfaces)
		{
			s.RemoveData();
			s.BuildNavMesh();
		}
		isBake = false;
	}
	public void ReBake()
	{
		if (!isBake)
		{
			isBake = true;
			GenerateNavmesh();
		}
		Debug.Log("hihihih");
	}
}