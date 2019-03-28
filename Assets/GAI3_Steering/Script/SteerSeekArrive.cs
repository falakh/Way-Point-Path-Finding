using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteerSeekArrive : MonoBehaviour
{
    public List<Transform> waypoints;

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
    public Vector3 _targetVelocity;

    public float _targetSpeed;
    // public Vector3 _accel;
    public int currentPoint = 0;
    

    public float _targetRadius; //Holds the radius for arriving at the target
    public float _slowRadius; //Holds the radius for beginning to slow down

    public float distance;

    void Start()
    {
        waypoints = new List<Transform>();
        GameObject wp = GameObject.Find("Waypoints");
        for (int i = 0; i < wp.transform.childCount; i++)
        {
            waypoints.Add(wp.transform.GetChild(i));
        }

        _target = waypoints[currentPoint];

//        StartCoroutine(FindPath());
    }

    // Update is called once per frame
    void Update()
    {
        _velocity = _velocity + getSteering()._linear * Time.deltaTime; // Vt = Vo +a.t

        if (_velocity.magnitude > _maxSpeed)
        {
            _velocity = _velocity.normalized * _maxSpeed;
        }

        transform.position = transform.position + _velocity * Time.deltaTime;
        transform.eulerAngles = SteeringData.getNewOrientation(transform.eulerAngles, _velocity);
        if (Result() > 0 && currentPoint<waypoints.Count-1)
        {
            currentPoint++;
            _target = waypoints[currentPoint];

        }

    }

    double Result()
    {

        var sklarA = skalar(Projection());
        var sklarB = skalar((JarakPoint()));
        return sklarA / sklarB;


    }

    Vector3 Projection()
    {
        var currentPos = transform.position - _target.position;
        var jarakWayPoint = JarakPoint();
        var curretDotJarak = dotVector(currentPos, jarakWayPoint);
        var bKuadrat = dotVector(jarakWayPoint, jarakWayPoint);
        float pembagian = (curretDotJarak / bKuadrat);
        return jarakWayPoint * pembagian ;
    }

    float dotVector(Vector3 a,Vector3 b)
    {
        return  a.x*b.x+a.y*b.y+a.z*b.z;
    }

    float skalar(Vector3 a)
    {
        return a.x + a.y + a.z;
    }

    Vector3 JarakPoint()
    {
        return waypoints[currentPoint + 1].position - waypoints[currentPoint].position;;
    }

    public SteeringData getSteering()
    {
        SteeringData _SteeringOut = new SteeringData();
        _SteeringOut._linear = _target.position - transform.position; //#direction

        distance = _SteeringOut._linear.magnitude;

        if (distance > _slowRadius)
        {
            _targetSpeed = _maxSpeed;
        }

        else if (distance <= _targetRadius)
        {
            _targetSpeed = 0;
        }

        else
        {
            _targetSpeed = _maxSpeed * distance / _slowRadius;
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

    IEnumerator FindPath()
    {
        for (int i = 0; i < waypoints.Count; i++)
        {
            _target = waypoints[i];
            yield return new WaitForSeconds(2);
        }
    }
}