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

	
	//no se ocupan
	public GUITexture gasPedal;
	public GUITexture brakePedal;
	public GUITexture leftPedal;
	public GUITexture rightPedal;
	//no se ocupan

	//mis variables
	Rigidbody rb;
	int rotation;
	float velocidad;
	Collider[] collidersObstaculos;
	bool sensorChoqueObjeto;
	Rigidbody piedraRecogida;
	Vector3 puntoInicial;
	bool sensorBaseEspacial;
	Rigidbody baseEspacial;
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
		sensorChoqueObjeto = false;
		piedraRecogida = null;
		setPuntoInicial();
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

        if(sensorChoqueObjeto){
        	pararAuto();
        	rotarAuto(90);
        }

        if(piedraRecogida != null){
        	moverPiedraRecogida();
        }

        if(sensorBaseEspacial){
        	Debug.Log("En base prros");
        }

        desactivarSensores(); //siempre se debe poner al final de la funcion
        
	}

	void desactivarSensores(){
		sensorChoqueObjeto = false;
        sensorBaseEspacial = false;
	}

	void setPuntoInicial(){
		puntoInicial = rb.position;
	}

	float calcularDistanciaRecorrida(){
		float dist = Vector3.Distance(puntoInicial, rb.position);
		return dist;
	}

	bool isModoReversa(){
		if(velocidad >0){
			return false;
		}
		return true;
	}

	bool isModoAdelante(){
		if(velocidad >0){
			return true;
		}
		return false;
	}

	void setModoReversa(){
		if(velocidad >0){
			velocidad = velocidad * (-1);
		}
	}

	void setModoAdelante(){
		if(velocidad <0){
			velocidad = -velocidad * (-1);
		}
	}

	void soltarPiedraRecogida(){
		piedraRecogida = null;
	}

	void moverPiedraRecogida(){
		piedraRecogida.MovePosition(rb.position + new Vector3(0,10,0));
	}

	void OnCollisionStay(Collision col)
    {
        if(col.gameObject.tag == "Obstaculo")
        {
            Debug.Log("Esta chocandaaaaaa");
            sensorChoqueObjeto = true;
        }	
    }

    
    //Destroy everything that enters the trigger
	void OnTriggerEnter (Collider col) {
		if(col.gameObject.tag == "Roca")
        {
            piedraRecogida = col.GetComponent<Collider>().attachedRigidbody;

        }	
    }

    void OnTriggerStay(Collider col){
    	if(col.gameObject.tag == "BaseEspacial"){
    		sensorBaseEspacial = true;
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
