using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class battering_ram : MonoBehaviour {

    public Animator anim;
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	
	void Update () {
		if(Input.GetMouseButtonDown(0)){
            anim.Play("choice", -1);
        }
            
	}
}
