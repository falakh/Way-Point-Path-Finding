using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteerSeek : MonoBehaviour
{
        
 /**
 data posisi dan orientasi pada karakter telah terdapat pada class transform
untuk mengakses gunakan transform
      **/
   
    public Transform _target;
    public int _maxSpeed;
    public int _maxAcceleration;
    public Vector3 _velocity = Vector3.zero;
    

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        _velocity = _velocity + getSteering()._linear * Time.deltaTime; // Vt = Vo +a.t
        
        if (_velocity.magnitude > _maxSpeed)
        {
            _velocity = _velocity.normalized * _maxSpeed;

        }

        this.transform.position = transform.position + _velocity * Time.deltaTime;
        this.transform.eulerAngles = SteeringData.getNewOrientation(transform.eulerAngles,_velocity);




    }

    public SteeringData getSteering()
    {
        SteeringData _SteeringOut = new SteeringData();
        _SteeringOut._linear = _target.position - transform.position; //#direction
        _SteeringOut._linear = _SteeringOut._linear.normalized; // normalize membuat resultan vektor = 1.
        _SteeringOut._linear *= _maxAcceleration; 
        return _SteeringOut;
    }

   

}
