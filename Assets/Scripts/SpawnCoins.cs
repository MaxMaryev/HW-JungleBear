using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCoins : MonoBehaviour
{
    [SerializeField] private Coin _coin;
    [SerializeField] private Transform _spawnPointsParent;

    private Transform[] _spawnCoinPoints;

    private const string ÑoinTag = "Coin";

    private void Awake()
    {
        _spawnCoinPoints = new Transform[_spawnPointsParent.childCount];

        for (int i = 0; i < _spawnPointsParent.childCount; i++)
        {
            _spawnCoinPoints[i] = _spawnPointsParent.GetChild(i);
        }
    }

    private void Start()
    {
        StartCoroutine(TrySpawn());
    }

    private IEnumerator TrySpawn()
    {
        bool isSpawning = true;

        while (isSpawning)
        {
            GameObject[] coins = GameObject.FindGameObjectsWithTag(ÑoinTag);

            if (coins.Length == 0)
                Instantiate(_coin, _spawnCoinPoints[Random.Range(0, _spawnCoinPoints.Length)].localPosition, Quaternion.identity);

            yield return new WaitForSeconds(1);
        }
    }
}
