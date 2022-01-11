using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsSpawner : MonoBehaviour
{
    [SerializeField] private Coin _coin;
    [SerializeField] private Transform _spawnPointsParent;

    private Transform[] _spawnCoinPoints;
    private List<Coin> _coins = new List<Coin>();

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
            if (_coins.Count == 0)
                _coins.Add(Instantiate(_coin, _spawnCoinPoints[Random.Range(0, _spawnCoinPoints.Length)].localPosition, Quaternion.identity));
            else if (_coins[0] == null)
                _coins.RemoveAt(0);

            yield return new WaitForSeconds(1);
        }
    }
}
