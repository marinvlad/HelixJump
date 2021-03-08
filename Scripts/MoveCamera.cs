using System;
using System.Collections;
using System.Collections.Generic;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private GameObject _player;

    [BoxGroup("Offset values"), SerializeField]
    private Vector3 _rotationOffsset;

    [BoxGroup("Offset values"), SerializeField]
    private Vector3 _positionOffset;

    private bool _followPlayer = true;

    public GameObject Player
    {
        get => _player;
        set
        {
            _player = value;
            StartFollow();
        }
    }
    
    private void StartFollow()
    {
        Timing.RunCoroutine(Move().CancelWith(gameObject), Segment.LateUpdate);
    }

    private IEnumerator<float> Move()
    {
        while (_followPlayer)
        {
            var playerPosition = _player.transform.TransformPoint(_positionOffset);
            transform.position = playerPosition;
            transform.rotation = Matrix4x4.Rotate(_player.transform.rotation).rotation * Quaternion.Euler(_rotationOffsset);
            yield return 0;
        }
    }
}

