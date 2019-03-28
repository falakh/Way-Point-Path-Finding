using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringData  {

    public Vector3 _linear;  //vector3 tipe data yg menampung nilai x,y,z
    public float _angular;
    public static Vector3 getNewOrientation(Vector3 _currentOrientation, Vector3 _velocity)
    {
        //Make sure we have a velocity
        if (_velocity.magnitude > 0) //length didapat dri fungsi magnitude (mendapatkan resultan)
        {
            //Calculate orientation using an arc tangent of the velocity components.
            float radian = Mathf.Atan2(_velocity.x, _velocity.z); //dalam radian, tidak perlu minus karena rotasi unity searah jarum jam
            float angle = radian * (180 / Mathf.PI);  // untuk mengganti ke angle
            Vector3 newOrientation = new Vector3(0, angle, 0);
            return newOrientation;
        }

        //Otherwise use the current orientation
        else
            return _currentOrientation;

    }

}
