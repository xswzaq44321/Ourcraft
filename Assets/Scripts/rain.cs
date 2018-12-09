using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rain : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.position.y < transform.parent.GetComponent<Weather>().player.transform.position.y - 50)
            Destroy(this.gameObject);
	}

}
