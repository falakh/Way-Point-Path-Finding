using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteerSeekColl : MonoBehaviour
{


    /**

data kinematic : posisi dan orientasi pada karakter telah terdapat pada class transform
untuk mengakses gunakan transform
         **/



    //Transform charakter; // sudah tidak perlu karena sudah bisa diakses dengan syntax this.transform 
    public Transform _target;
    public float _maxSpeed;
    public int _maxAcceleration;
    public int _maxBrakeForce;
    public Vector3 _velocity = Vector3.zero;
    public Vector3 _targetVelocity ;
    public float _targetSpeed;
    public Vector3 _steeringAll = Vector3.zero;
    public float _max_See_Ahead = 8;
    public float _max_Avoid_Force = 8;

    public float _targetRadius;  //Holds the radius for arriving at the target
    public float _slowRadius;  //Holds the radius for beginning to slow down

    public float _distance;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _steeringAll = getSteering()._linear + getCollideAvoid()._linear;
        _velocity = _velocity + _steeringAll * Time.deltaTime; // Vt = Vo +a.t

        if (_velocity.magnitude > _maxSpeed)
        {
            _velocity = _velocity.normalized * _maxSpeed;

        }

        this.transform.position = transform.position + _velocity * Time.deltaTime;
        this.transform.eulerAngles = SteeringData.getNewOrientation(transform.eulerAngles, _velocity);



    }

    public SteeringData getSteering()
    {
        SteeringData _SteeringOut = new SteeringData();
        _SteeringOut._linear = _target.position - transform.position; //#direction

        _distance = _SteeringOut._linear.magnitude;

        if (_distance > _slowRadius)
        {

            _targetSpeed = _maxSpeed;
        }

        else if (_distance <= _targetRadius)
        {
            _targetSpeed = 0;
        }

        else
        {
            _targetSpeed = _maxSpeed * _distance / _slowRadius;
        }

        _targetVelocity = _SteeringOut._linear.normalized * _targetSpeed;
        _SteeringOut._linear = (_targetVelocity - _velocity);

        // if (_targetSpeed < _maxSpeed)  //jika melambat gunakan brakeForce
        // {
        //         _SteeringOut._linear = _SteeringOut._linear.normalized; // normalize membuat resultan vektor = 1.
        //         _SteeringOut._linear *= _maxBrakeForce;   
        // }
        //  else
        // {
        if (_SteeringOut._linear.magnitude > _maxAcceleration)
        {
            _SteeringOut._linear = _SteeringOut._linear.normalized; // normalize membuat resultan vektor = 1.
            _SteeringOut._linear *= _maxAcceleration;
        }
        //  }

        return _SteeringOut;
    }

    public SteeringData getCollideAvoid()
    {
       Vector3 ahead = this.transform.position + _velocity.normalized * _max_See_Ahead ;
       Vector3 ahead2 = this.transform.position + _velocity.normalized * _max_See_Ahead * 0.5f;

        Circle _ClosestObsCirle = findMostThreateningObstacle(ahead,ahead2); 
        SteeringData _SteeringOut = new SteeringData();

        
        if (_ClosestObsCirle != null)
        {
            _SteeringOut._linear = ahead - _ClosestObsCirle._center;

            _SteeringOut._linear = _SteeringOut._linear.normalized *_max_Avoid_Force;

        }
        else { _SteeringOut._linear = Vector3.zero; }
        return _SteeringOut;
    }

    public float getNewOrientation(Vector3 _currentOrientation, Vector3 _velocity)
    {
        //Make sure we have a velocity
        if (_velocity.magnitude > 0) //length didapat dri fungsi magnitude (mendapatkan resultan)
        {
            //Calculate orientation using an arc tangent of the velocity components.
            float radian = Mathf.Atan2(_velocity.x, _velocity.z); //dalam radian, tidak perlu minus karena rotasi unity searah jarum jam
            float angle = radian * (180 / Mathf.PI);  // untuk mengganti ke angle
            //Vector3 newOrientation = new Vector3(0, angle, 0);
            return angle;
        }

        //Otherwise use the current orientation
        else
            return _currentOrientation.y;

    }




    public bool lineIntersectsCircle(Vector3 _ahead, Vector3 _ahead2 , Circle obstacle )
    {
        // the property "center" of the obstacle is a Vector3D.

        float distance1 = (obstacle._center - _ahead).magnitude;
        float distance2 = (obstacle._center - _ahead2).magnitude;

        return (distance1 <= obstacle._radius) || (distance2 <= obstacle._radius);
    }

   public Circle findMostThreateningObstacle(Vector3 _ahead, Vector3 _ahead2)  {
    
    Circle mostThreatening  = null;
    //cek semua objek yg perlu dihindari
    foreach (GameObject _obstacle in GameObject.FindGameObjectsWithTag("Obstacle"))
     { Circle CircleObs  = _obstacle.GetComponent<Circle>();
        bool _isCollide = lineIntersectsCircle(_ahead, _ahead2, CircleObs);

            //cek collision yang paling dekat
            if (_isCollide)
            {
                if (mostThreatening == null) { 
                  mostThreatening = CircleObs;
                    Debug.Log(mostThreatening.gameObject);
                }
                else {
                    Debug.Log(GameObject.FindGameObjectsWithTag("Obstacle").Length );
                  float distanceCurrent = (this.transform.position - mostThreatening._center).magnitude;
                  float distanceCek = (this.transform.position - CircleObs._center).magnitude;
                    mostThreatening = distanceCek < distanceCurrent ? CircleObs : mostThreatening;
                }
            }
        }
     return mostThreatening;
    }





  



}
