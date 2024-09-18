using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StearingBehaivourSeek : MonoBehaviour
{
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 currentPosition = transform.position;
        Vector2 targetPosition = _targetPosition.position;
        Vector2 desiredMovement = targetPosition - currentPosition;
        Vector2 desiredDirection = desiredMovement.normalized;
        Vector2 desiredVelocity = desiredDirection * _maxvelocity;

  
        //La diferencia entre nuestra velocidad actual y la deseada es nuestra uerza steering
        Vector2 steering = desiredVelocity - _velocity;

        //Nos aseguramos de que la fuerza steering aplicada para modificar la velocidad
        //no exceda una fuerza maxima (No permite cambiar demasiado rápido la trayectoria).

        steering = Vector2.ClampMagnitude(steering, _maxForce);
        //La massa determina una mayor/menor agilidad
        steering /= _mass;
        //Nos aseguramos que la velocidad más la fuerza steering no exceda la máxima velocidad.
        _velocity = Vector2.ClampMagnitude(_velocity + steering, _maxvelocity);
        
        transform.position = currentPosition + _velocity * Time.deltaTime;
    }
}
