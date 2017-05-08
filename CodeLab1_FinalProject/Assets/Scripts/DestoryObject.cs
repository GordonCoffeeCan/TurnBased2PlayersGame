using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Invoke("DestroySelf", 6);
	}

    private void DestroySelf() {
        Destroy(this.gameObject);
    }
}
