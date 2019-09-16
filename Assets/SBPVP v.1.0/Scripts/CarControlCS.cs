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

	public Rigidbody baseEspacial;
	public Rigidbody contadorIndividual;

	//mis variables
	Rigidbody rb;
	float rotation;
	float velocidad;
	Collider[] collidersObstaculos;
	bool sensorChoqueObjeto;
	Rigidbody piedraRecogida;
	Vector3 puntoInicial;
	bool sensorBaseEspacial;
	bool sensorPiedraRecogida;
	int alturaPiedra;
	bool rodeandoObjeto;
	// Use this for initialization
	void Start () {
		//Alter the center of mass for stability on your car
		
		gasPedal.gameObject.SetActive(false);
		leftPedal.gameObject.SetActive(false);
		rightPedal.gameObject.SetActive(false);
		brakePedal.gameObject.SetActive(false);

		rb = GetComponent<Rigidbody>();
		rotation = transform.localEulerAngles.y;
		velocidad = 10.0f;
		sensorChoqueObjeto = false;
		piedraRecogida = null;
		sensorPiedraRecogida = false;
		setPuntoInicial();
		alturaPiedra = 0;
		rodeandoObjeto = false;

		rotarAutoAleatoriamente();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if(sensorChoqueObjeto){//Evitar obstaculos
			pararAuto();
			rotarAuto(5);
			rodeandoObjeto = true;
			setPuntoInicial();
			capaAvanzar();
		}
		else if(sensorPiedraRecogida && sensorBaseEspacial){//muestras y en nave: Soltarlas
			soltarPiedraRecogida();
			//rotarAuto(180);
		}
		else if(false){//muestras y no en nave: soltar 2 moronas e ir a nave

		}
		else if(false){//Deteccion muestra: recoger
			//esta accion se realiza cada frame en ontrigger enter
		}
		else if(false){//deteccion moronas: tomar 1 y seguir camino

		}
		else if(true){//nada: moverse
			capaAvanzar();
		}
        desactivarSensores(); //siempre se debe poner al final de la funcion  
        ajustarRotaciónAuto(); 
        

	}

	void capaAvanzar(){
		if(sensorPiedraRecogida){
				if(rodeandoObjeto){
					if(calcularDistanciaRecorrida() > 2.0f){
						rodeandoObjeto = false;
					}
				}
				else{
						LookRotationBaseEspacial();		
				}
			}
		moverPiedraRecogida();
		avanzarConRotacion(rotation);	
	}

	

	void ajustarRotaciónAuto(){
		rotation = transform.localEulerAngles.y;
	}

	void rotarAutoAleatoriamente(){
		float numeroAleatorio = Random.Range(-30.0f, 30.0f);
		rotarAuto(numeroAleatorio);
	}

	void LookRotationBaseEspacial(){
		Vector3 relativePos = baseEspacial.transform.position - transform.position;
		Quaternion newRotation = Quaternion.LookRotation(relativePos, Vector3.up);
		transform.rotation = newRotation;
		rotation = transform.localEulerAngles.y;
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
		piedraRecogida.MovePosition(baseEspacial.position+ new Vector3(0,5 + alturaPiedra,0));
		alturaPiedra += 1;
		piedraRecogida = null;
		sensorPiedraRecogida = false;
		modificarMarcador();
	}

	void modificarMarcador(){
		contadorIndividual.GetComponent<contadorVerdes>().anadirContadorIndividual();
	}

	void moverPiedraRecogida(){
		if(sensorPiedraRecogida){
			piedraRecogida.MovePosition(rb.position + new Vector3(0,10,0));
		}
	}

	void OnCollisionStay(Collision col)
    {
        if(col.gameObject.tag == "Obstaculo")
        {
            Debug.Log("Esta chocandaaaaaa");
            
        }	
    }

    
    //Destroy everything that enters the trigger
	void OnTriggerEnter (Collider col) {
		if(col.gameObject.tag == "Roca" && sensorPiedraRecogida == false)
        {
            piedraRecogida = col.GetComponent<Collider>().attachedRigidbody;
            sensorPiedraRecogida = true;

        }	
    }

    void OnTriggerStay(Collider col){
    	if(col.gameObject.tag == "BaseEspacial"){
    		sensorBaseEspacial = true;
    	}
    	else if(col.gameObject.tag == "Obstaculo"){
    		sensorChoqueObjeto = true;
    	}
    	else if(col.gameObject.tag == "Auto"){
    		sensorChoqueObjeto = true;
    		rotarAutoAleatoriamente();
    	}
    }

	void avanzarConRotacion(float deg){
		float co = Mathf.Sin(degToRad(deg));
		co = radToDeg(co) * Time.deltaTime;

		float ca = Mathf.Cos(degToRad(deg));
		ca = radToDeg(ca) * Time.deltaTime;

		cambiarVelocidad(velocidad * co,0,velocidad*ca);
	}

	void rotarAuto(float deg){
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


}
