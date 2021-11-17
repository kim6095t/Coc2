using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class MapManager : Singletone<MapManager>
{
	public string TAG_PLAYABLE = "Playable";
	string sceneName;

	[SerializeField]
	private GameObject _mapPrefab;
	private Vector3 _generatePos = new Vector3(50, 0, 50);
	NavMeshSurface[] surfaces;


	private void Awake()
	{
		base.Awake();
		//베이킹 할 맵을 지정하고 만든다.
		GameObject obj = Instantiate(_mapPrefab, _generatePos, Quaternion.identity, transform);
		_generatePos += new Vector3(50, 0, 50);
		surfaces = gameObject.GetComponentsInChildren<NavMeshSurface>();

		StartCoroutine(GenerateNavmesh());

		sceneName = SceneManager.GetActiveScene().name;
	}

	IEnumerator GenerateNavmesh()
	{
		foreach (var s in surfaces)
		{
			s.RemoveData();
			s.BuildNavMesh();
			yield return null;
		}
		yield return null;
	}
	public void ReBake()
	{
		if(SceneManager.GetActiveScene().name == sceneName)
			StartCoroutine(GenerateNavmesh());
	}
}