using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [SerializeField] private GameObject currentHitObject; // хранит игровой объект которого коснулась наша окружность
    [SerializeField] private float circleRadius; // радиус окружности
    [SerializeField] private float maxDistance; // максимальная дистанция между противником и окружностью
    [SerializeField] private LayerMask layerMask; // слой который будте виден нашему противнику

    private EnemyController _enemyController;
    private Vector2 _origin; // точка откуда будет создаваться наша окружность
    private Vector2 _direction; // задает направление от точки origin до создания окружности

    private float _currentHitdistance; // расстояние от нашего противника до объекта, который попал в радиус нашей окружности

   
    private void Start()
    {
        _enemyController = GetComponent<EnemyController>();
    }
    private void Update()
    {
        _origin = transform.position;

        if (_enemyController.IsFacingRight)
        {
            _direction = Vector2.right;

        }
        else
        {
            _direction = Vector2.left;
        }
        RaycastHit2D hit = Physics2D.CircleCast(_origin, circleRadius, _direction, maxDistance, layerMask); // создаем невидимый объект в виде окружности

        if (hit)
        {
            currentHitObject = hit.transform.gameObject;
            _currentHitdistance = hit.distance;

            if (currentHitObject.CompareTag("Player"))
            {
                _enemyController.StartChasingPlayer();
            }
            else
            {
                currentHitObject = null;
                _currentHitdistance = maxDistance;
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_origin, _origin + _direction * _currentHitdistance);
        Gizmos.DrawWireSphere(_origin + _direction * _currentHitdistance, circleRadius);
    }
}
