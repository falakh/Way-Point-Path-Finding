using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour {
  
    public float _radius=7;
    public Vector3 _center;
    // Use this for initialization
    void Start () {
        _center = this.transform.position;
    }
	
	
	void Update () {
        _center = this.transform.position;
        	
	}
}
