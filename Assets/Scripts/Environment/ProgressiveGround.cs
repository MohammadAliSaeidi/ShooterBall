using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// This script creates tiled ground around the target (i.e. player).
/// Every time target moves, new tiles will be created at the way of it. 
/// And older tiles that the target moves away from, will be destroyed.
/// </summary>
public class ProgressiveGround : MonoBehaviour
{
	[Tooltip("If Enabled the script works")]
	[SerializeField]
	private bool Enable = true;

	[Tooltip("Script will create tiled ground for it")]
	[SerializeField]
	private Transform _target;

	[Tooltip("If enabled, the script checks everything at lower fps for better performance")]
	[SerializeField]
	private bool _useLowerFps = false;

	[Tooltip("if \"Use Lower Fps\" is enabled this field will be effected")]
	[SerializeField]
	[Range(10, 60)]
	private float _lowerFpsAmount = 20;

	[Tooltip("Create tiles around the target with this radius")]
	[Range(1, 10)]
	[SerializeField]
	private float _groundRadius = 5;

	[Tooltip("Offset of the first tile position bellow the target")]
	[SerializeField]
	private Vector3 _groundBaseOffset;

	[Tooltip("tiles will be created under the target to create ground")]
	[SerializeField]
	private GameObject _tilePrefab;

	[Tooltip("Tiles will be instanitated next to each other based on this field")]
	[SerializeField]
	private Vector3 _tileDimentions;

	private Vector3 _basePosition;
	private List<GameObject> _allTiles = new List<GameObject>();

	private void Start()
	{
		_basePosition = _target.position;

		StartCoroutine(Co_Update());
	}

	private IEnumerator Co_Update()
	{
		var frameTime = 1.0f / _lowerFpsAmount;
		while (true)
		{
			var tilesAroundThePlayer = new List<GameObject>();

			if (_target != null)
			{
				var newBasePosition = GetTilePositionUnderTarget();
				var tilePositions = CreateTileMatrix(newBasePosition);

				// for each tile pos near the target:
				foreach (var tilePos in tilePositions)
				{
					var isTilesExistsInPosition = TilesExistsInPosition(tilePos);
					if (!isTilesExistsInPosition.Item2)
					{
						var tileInstance = Instantiate(_tilePrefab, tilePos, Quaternion.identity);
						_allTiles.Add(tileInstance);
						tilesAroundThePlayer.Add(tileInstance);
					}
					else if (isTilesExistsInPosition.Item1)
					{
						tilesAroundThePlayer.Add(isTilesExistsInPosition.Item1);
					}
				}

				var tilesToDestroy = _allTiles.Except(tilesAroundThePlayer).ToList();
				foreach (var tile in tilesToDestroy)
				{
					_allTiles.Remove(tile);
					Destroy(tile);
				}
			}

			if (Time.deltaTime <= frameTime)
			{
				yield return new WaitForSeconds(frameTime);
			}

			yield return null;
		}
	}

	private Vector3 GetTilePositionUnderTarget()
	{
		var xDist = _basePosition.x - _target.position.x;
		var zDist = _basePosition.z - _target.position.z;

		var xIndex = Mathf.Round(xDist / _tileDimentions.x);
		var zIndex = Mathf.Round(zDist / _tileDimentions.z);

		var xPos = xIndex * _tileDimentions.x;
		var zPos = zIndex * _tileDimentions.z;

		return new Vector3(xPos, _groundBaseOffset.y, zPos);
	}

	private List<Vector3> CreateTileMatrix(Vector3 baseTilePosition)
	{
		var tilePositions = new List<Vector3>();
		int xTilesCount = Mathf.CeilToInt(_groundRadius * 2 / _tileDimentions.x);
		int zTilesCount = Mathf.CeilToInt(_groundRadius * 2 / _tileDimentions.z);

		for (int x = 0; x < xTilesCount; x++)
		{
			for (int z = 0; z < zTilesCount; z++)
			{
				var xTileIndex = x - Mathf.Ceil(xTilesCount / 2);
				var zTileIndex = z - Mathf.Ceil(zTilesCount / 2);

				var xPos = (xTileIndex * _tileDimentions.x) - baseTilePosition.x;
				var zPos = (zTileIndex * _tileDimentions.z) - baseTilePosition.z;

				if (Vector3.Distance(new Vector3(_target.position.x, 0, _target.position.z), new Vector3(xPos, 0, zPos)) <= _groundRadius)
				{
					tilePositions.Add(new Vector3(xPos, _groundBaseOffset.y, zPos));
				}
			}
		}

		return tilePositions;
	}

	private Tuple<GameObject, bool> TilesExistsInPosition(Vector3 tilePosition)
	{
		if (_allTiles.Any())
		{
			foreach (var tile in _allTiles)
			{
				if (Vector3.Distance(tile.transform.position, tilePosition) <= 0.01f)
				{
					return new Tuple<GameObject, bool>(tile, true);
				}
			}
		}

		return new Tuple<GameObject, bool>(null, false);
	}
}
