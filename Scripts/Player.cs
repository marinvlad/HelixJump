using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody _rigidBody;
    [SerializeField] private Vector3 _jump;
    [SerializeField] private float _jumpForce;
    [SerializeField] private CollisionEffect _dropEffect;
    [SerializeField] private CollisionEffect _splashEffect;
    [SerializeField] private IntEvent _onStepDestroy;
    [SerializeField] private SimpleEvent _onLevelEnd;
    [SerializeField] private SimpleEvent _onGameOver;
    [SerializeField] private SimpleEvent _onRestart;
    [SerializeField] private IntVariable _currentStep;
    [SerializeField] private IntVariable _stepsCount;
    [SerializeField] private IntVariable _currentScore;

    private void OnEnable()
    {
        _onRestart.Subscribe(Restart);
    }

    private void OnDisable()
    {
        _onRestart.Unsubscribe(Restart);
    }

    private void Start()
    {
        _rigidBody = gameObject.GetComponent<Rigidbody>();
        _currentStep.Value = 0;
        _stepsCount.Value = 0;
        _currentScore.Value = 0;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Final"))
        {
            _onStepDestroy.Invoke(other.gameObject.GetInstanceID());
            _onLevelEnd.Invoke();
        }
        else if (other.gameObject.CompareTag("GameOver"))
        {
            _onGameOver.Invoke();
        }
        else
        {
            _rigidBody.AddForce(_jump * _jumpForce, ForceMode.Impulse);
            GameObject drop = Instantiate(_dropEffect.effect, other.contacts[0].point, Quaternion.Euler(-90, 0, 0));
            GameObject splash = Instantiate(_splashEffect.effect, other.contacts[0].point, Quaternion.identity);
            splash.transform.SetParent(other.transform);
            Destroy(drop, 1.5f);
            Destroy(splash, 1.5f);
            _stepsCount.Value = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _onStepDestroy.Invoke(other.transform.parent.gameObject.GetInstanceID());
        if (other.gameObject.CompareTag("StartStep"))
            return;
        _currentStep.Value++;
        _stepsCount.Value++;

        _currentScore.Value += 10 * _stepsCount.Value;
    }

    private void Restart()
    {
        _currentStep.Value = 0;
        _stepsCount.Value = 0;
        _currentScore.Value = 0;
        transform.position = new Vector3(0, 0, -1);
    }
}