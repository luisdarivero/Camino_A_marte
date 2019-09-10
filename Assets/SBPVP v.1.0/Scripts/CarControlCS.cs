//2016 Spyblood Productions
//Use for non-commerical games only. do not sell comercially
//without permission first

using UnityEngine;
using System.Collections;



public enum DriveType
{
	RWD,
	FWD,
	AWD
};


public class CarControlCS : MonoBehaviour {

	
	
	public GUITexture gasPedal;
	public GUITexture brakePedal;
	public GUITexture leftPedal;
	public GUITexture rightPedal;

	//mis variables
	Rigidbody rb;
	int rotation;
	float velocidad;
	bool sensorChoque;
	Collider[] collidersObstaculos;

	// Use this for initialization
	void Start () {
		//Alter the center of mass for stability on your car
		
		gasPedal.gameObject.SetActive(false);
		leftPedal.gameObject.SetActive(false);
		rightPedal.gameObject.SetActive(false);
		brakePedal.gameObject.SetActive(false);

		rb = GetComponent<Rigidbody>();
		rotation =0 ;
		velocidad = 5.0f;
		sensorChoque = false;
		collidersObstaculos = new Collider[10];
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (Input.GetKeyDown(KeyCode.W))
        {
        	pararAuto();
        	rotarAuto(10);
        }

        if(Input.GetKeyDown("s")){
        	
			Debug.Log("s");
			
			avanzarConRotacion(rotation);
        }

        if(sensorChoque){
        	pararAuto();
        	rotarAuto(90);
        }

        sensorChoque = false;

	}

	//solo se activa si choca directamente 
	void OnCollisionEnter (Collision col)
    {
        //no Debería llegar aquí por que ya chocaste
    }

    //Destroy everything that enters the trigger
	void OnTriggerEnter (Collider col) {
		if(col.gameObject.tag == "Obstaculo")
        {
            anadirColliderObstaculoALista(col);
        }	
    }

    void anadirColliderObstaculoALista(Collider col){
    	for(int i = 0; i< collidersObstaculos.Length ; i++){
    		if(collidersObstaculos[i] == null){
    			collidersObstaculos[i] = col;
    			i = collidersObstaculos.Length + 1;
    		}
    	}
    }

	void avanzarConRotacion(int deg){
		float co = Mathf.Sin(degToRad(deg));
		co = radToDeg(co) * Time.deltaTime;

		float ca = Mathf.Cos(degToRad(deg));
		ca = radToDeg(ca) * Time.deltaTime;

		cambiarVelocidad(velocidad * co,0,velocidad*ca);
	}

	void rotarAuto(int deg){
		rotation = rotation + deg;
		rb.MoveRotation(Quaternion.Euler(0, rotation, 0));
	}

	void pararAuto(){
		rb.velocity = new Vector3(0,0,0);
	}

	void cambiarVelocidad(float x, float y, float z){
		rb.velocity = new Vector3(x,y,z);
	}

	float radToDeg(float rad){
		return ((rad * 180.0f) / Mathf.PI);
	}

	float degToRad(float deg){
		return ((deg * Mathf.PI) / 180.0f);
	}


	void OnGUI()
	{
		//show the GUI for the speed and gear we are on.
		GUI.Box(new Rect(10,10,70,30),"MPH: " );
		if (false)
			GUI.Box(new Rect(10,70,70,30),"Gear: " );
		if (true)//if the car is going backwards display the gear as R
			GUI.Box(new Rect(10,70,70,30),"Gear: R");
	}
}
