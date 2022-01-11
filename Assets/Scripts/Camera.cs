using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] private Transform _player;

    private float _yCoordinate = 5;
    private float _zCoordinate = -10;
    private Vector3 _leftBorderPosition, _rightBorderPosition;

    private void Awake()
    {
        float leftBorderX = 6.44f;
        float rightBorderX = 100;

        _leftBorderPosition = new Vector3(leftBorderX, _yCoordinate, _zCoordinate);
        _rightBorderPosition = new Vector3(rightBorderX, _yCoordinate, _zCoordinate);
    }

    private void LateUpdate()
    {
        if (_player.position.x < _leftBorderPosition.x)
            transform.position = _leftBorderPosition;
        else if (_player.position.x > _rightBorderPosition.x)
            transform.position = _rightBorderPosition;
        else
            transform.position = new Vector3(_player.position.x, _yCoordinate, _zCoordinate);
    }
}
