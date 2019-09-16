using UnityEngine;
using System.Collections;

public class VehicleCameraControl : MonoBehaviour
{

	float velocidad;
		
	void Start(){
		velocidad = 300;
	}
	
	void Update(){}
	
	void FixedUpdate (){
		
		if (Input.GetKey("right")){
            transform.position = transform.position + new Vector3(velocidad * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey("left")){
            transform.position = transform.position + new Vector3(-velocidad * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey("up")){
            transform.position = transform.position + new Vector3(0, 0, velocidad * Time.deltaTime);
        }
        if (Input.GetKey("down")){
            transform.position = transform.position + new Vector3(0, 0, -velocidad * Time.deltaTime);
        }
		if (Input.GetKey("s")){
            transform.position = transform.position + new Vector3(0, velocidad * Time.deltaTime, 0);
        }
        if (Input.GetKey("w")){
            transform.position = transform.position + new Vector3(0, -velocidad * Time.deltaTime, 0);
        }
		
	}

}