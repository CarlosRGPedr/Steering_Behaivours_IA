using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringWanderBehaivour : MonoBehaviour
{
    enum Stearing_Behaivor {SEEK, FLEE, ARRIVE}
    private Stearing_Behaivor stearingBehaivor;
    [SerializeField]
    private Transform _targetPosition;
    [SerializeField]
    private Vector2 _velocity;
    [SerializeField]
    private float _maxvelocity = 2.00f;
    [SerializeField]
    private float _maxForce = 2.00f;
    [SerializeField]
    private float _mass = 1.0f;
    [SerializeField]
    private float nextDecision = 1.0f;
    [SerializeField]
    private float now = 0;
    [SerializeField] bool wandering = true;
    [SerializeField]
    private float _slowRadius = 1.0f;
    [SerializeField]
    private float _wanderCircleDistance = 10.0f;
    [SerializeField]
    private float _wanderCircleRadius = 5.0f;
    [SerializeField]
    private float _wanderAngle = 0;
    [SerializeField]
    private float _wanderChange = 45;
    [SerializeField]
    private float distanceToSeeAhead = 5.0f;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    private void Wander()
    {
        now += Time.deltaTime;
        if(now >= nextDecision)
        {
            UpdateTargetPosForWandering();
            now = 0;
        }
    }

    void UpdateTargetPosForWandering()
    {
        Vector2 position = transform.position;
        Vector2 targetPos = _targetPosition.position;
        Vector2 desiredMovement = targetPos - position;
        Vector2 desiredDirection = desiredMovement.normalized;
        Vector2 circleCenter = desiredDirection * _wanderCircleDistance;
        _wanderAngle = Random.Range(-_wanderChange, _wanderChange);
        Vector2 displacement = Quaternion.AngleAxis(_wanderAngle, Vector3.forward) * (Vector2.up * _wanderCircleRadius);

        Vector2 newTargetPos = circleCenter + displacement;
        _targetPosition.position = newTargetPos;
    }

    private void AvoidObstacle()
    {
        Vector2 currentPosition = transform.position;
        Vector2 detectObstacle = currentPosition + CalculateDesiredVelocity(transform.position, _targetPosition.position).normalized * distanceToSeeAhead;
        Vector2 detectObstacle2 = detectObstacle * 0.5f;
    }

    Vector2 CalculateDesiredVelocity(Vector2 currentPosition, Vector2 targetPosition)
    {
        Vector2 desiredMovement = targetPosition - currentPosition;
        Vector2 desiredDirection = desiredMovement.normalized;
        Vector2 desiredVelocity;
        float distance = desiredMovement.magnitude;
        if (distance < _slowRadius)
        {
            desiredVelocity = desiredDirection * _maxvelocity * (distance / _slowRadius);
        }
        else
        {
            desiredVelocity = desiredDirection * _maxvelocity;
        }
        return desiredVelocity;
    }

    private void SeekTarget()
    {
        Vector2 currentPosition = transform.position;
        Vector2 targetPosition = _targetPosition.position;
       
        Vector2 desiredVelocity = CalculateDesiredVelocity(currentPosition, targetPosition);
        //La diferencia entre nuestra velocidad actual y la deseada es nuestra fuerza steering
        Vector2 steering = desiredVelocity - _velocity;
        steering = Vector2.ClampMagnitude(steering, _maxForce);
        //La massa determina una mayor/menor agilidad
        steering /= _mass;
        //Nos aseguramos que la velocidad más la fuerza steering no exceda la máxima velocidad.
        _velocity = Vector2.ClampMagnitude(_velocity + steering, _maxvelocity);

        transform.position = currentPosition + _velocity * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {

        Wander();
        SeekTarget();    
    }
}
