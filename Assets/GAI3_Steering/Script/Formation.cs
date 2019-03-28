using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Formation : MonoBehaviour {

    public GameObject prefab;

    public int jumKarakter;
    public float radius;
    List<GameObject> slots = new List<GameObject>();

    float derajatRad;
    // Use this for initialization
    void Start () {

        GameObject GJ = Instantiate(prefab,this.transform.position+ (this.transform.right*radius), Quaternion.identity);
        GJ.transform.parent = this.transform;
        slots.Add(GJ);
        derajatRad = (360/jumKarakter)*Mathf.Deg2Rad;
        Vector3 pusatFormasi = this.transform.position;
        Debug.Log(Mathf.Cos(derajatRad));


        for (int i = 0; i < jumKarakter - 1; i++)
        {
            float xNew = (slots[i].transform.position.x - pusatFormasi.x) * Mathf.Cos(derajatRad) - (slots[i].transform.position.z - pusatFormasi.z) * Mathf.Sin(derajatRad) + pusatFormasi.x;
            float zNew = (slots[i].transform.position.x - pusatFormasi.x) * Mathf.Sin(derajatRad) + (slots[i].transform.position.z - pusatFormasi.z) * Mathf.Cos(derajatRad) + pusatFormasi.z;
            Vector3 posBaru = new Vector3(xNew, 0, zNew);
            GameObject nGJ = Instantiate(prefab, posBaru, Quaternion.identity);
            nGJ.transform.parent = this.transform;
            slots.Add(nGJ);
        }


    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
