
using UnityEngine;
using Unity.Netcode;
using System.Collections;
using System.Collections.Generic;

using Random = UnityEngine.Random;

public class EnemySpawner : SceneManager {

	[SerializeField] private NetworkObject _enemyPrefab;

	[SerializeField] private float _circleSpawnRadius = 30f;
	[SerializeField] private float _spawnDelay = 5f;

	[SerializeField] private int _spawnEnemiesPerCycleCount = 3;
	[SerializeField] private int _maxEnemiesCount = 15;
	
	private CoroutineWrapper _enemySpawnRoutine = null;
	private List<NetworkObject> _activeEnemies = new List<NetworkObject>();

	public bool IsRunning => _enemySpawnRoutine != null;

	public void StartSpawning() {

		if (!NetworkManager.Singleton.IsServer || IsRunning) {
			return;
		}

		_enemySpawnRoutine?.Stop();
		_enemySpawnRoutine = CoroutineBehavior.StartCoroutine(SpawningCoroutine());

	}

	public void StopSpawning(bool destroyAllEnemies = false) {

		if (!NetworkManager.Singleton.IsServer || !IsRunning) {
			return;
		}

		_enemySpawnRoutine?.Stop();
		_enemySpawnRoutine = null;

		if (destroyAllEnemies) {
			


		}

	}

	private IEnumerator SpawningCoroutine() {

		while (true) {
			SpawnEnemies();
			yield return new WaitForSeconds(_spawnDelay);
		}

		void SpawnEnemies() {

			for (int i = 0; i < _spawnEnemiesPerCycleCount; i++) {

				if (_maxEnemiesCount <= _activeEnemies.Count) {
					return;
				}

				var point = (Quaternion.Euler(0, Random.Range(0f, 360f), 0) * Vector3.forward) * _circleSpawnRadius;
				var emeny = NetworkManager.Singleton.SpawnManager.InstantiateAndSpawn(
					_enemyPrefab,
					0, false, false, false,
					point,
					Quaternion.identity);
				_activeEnemies.Add(emeny);

			}

		}

	}

}
