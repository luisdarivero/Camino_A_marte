//2016 Spyblood Productions
//Use for non-commerical games only. do not sell comercially
//without permission first

using UnityEngine;
using System.Collections;


public class CarControlCSCollaborative : MonoBehaviour {

	
	//no se ocupan

	public GUITexture gasPedal;
	public GUITexture brakePedal;
	public GUITexture leftPedal;
	public GUITexture rightPedal;
	//no se ocupan

	public Rigidbody baseEspacial;
	public Rigidbody contadorColaborativo;

	//Referencias a las moronas
	public Rigidbody morona1;
	public Rigidbody morona2;
	public Rigidbody morona3;
	public Rigidbody morona4;
	public Rigidbody morona5;
	public Rigidbody morona6;

	//moronas que se usan
	Rigidbody moronaCompartimiento1;
	Rigidbody moronaCompartimiento2;
	bool dirigirseAmoronas;

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
		dirigirseAmoronas = true;

		rotarAutoAleatoriamente();

		moronaCompartimiento1 = morona1;
		moronaCompartimiento2 = morona2;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if(sensorChoqueObjeto){//Evitar obstaculos
			pararAuto();
			rotarAuto(5);
			rodeandoObjeto = true;
			setPuntoInicial();
			if(!sensorPiedraRecogida){
				dirigirseAmoronas = true;
			}
			capaAvanzar();
		}
		else if(sensorPiedraRecogida && sensorBaseEspacial){//muestras y en nave: Soltarlas
			soltarPiedraRecogida();
			rotarAuto(180);
			dirigirseAmoronas = true;
		}
		else if(false){//muestras y no en nave: soltar 2 moronas e ir a nave
			//esta accion se realiza cada frame en ontrigger enter
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
		if(sensorPiedraRecogida || dirigirseAmoronas){
			if(rodeandoObjeto){
				if(calcularDistanciaRecorrida() > 2.0f){
					rodeandoObjeto = false;
				}
			}
			else{
				if(sensorPiedraRecogida){

					LookRotationBaseEspacial();		
				}
				else if(dirigirseAmoronas){
					LookRotationMorona();
				}
			}
		}
		
		moverPiedraRecogida();
		avanzarConRotacion(rotation);	
		moverMoronas();
	}

	

	void LookRotationMorona(){
		Rigidbody moronaTemporal = buscarMoronasSueltas();
		if(moronaTemporal != null){
			Vector3 relativePos = moronaTemporal.transform.position - transform.position;
			Quaternion newRotation = Quaternion.LookRotation(relativePos, Vector3.up);
			transform.rotation = newRotation;
			rotation = transform.localEulerAngles.y;
		}
		
	}

	Rigidbody buscarMoronasSueltas(){
		if(morona1.tag == "MoronaSuelta"){
			return morona1;
		}
		else if(morona6.tag == "MoronaSuelta"){
			return morona6;
		}
		else if(morona3.tag == "MoronaSuelta"){
			return morona3;
		}
		else if(morona4.tag == "MoronaSuelta"){
			return morona4;
		}
		else if(morona5.tag == "MoronaSuelta"){
			return morona5;
		}
		else if(morona2.tag == "MoronaSuelta"){
			return morona2;
		}
		else{
			return null;
		}
	}

	void moverMoronas(){
		if(moronaCompartimiento1 != null){
			moronaCompartimiento1.MovePosition(rb.position + new Vector3(0,5,0));
		}
		if(moronaCompartimiento2 != null){
			moronaCompartimiento2.MovePosition(rb.position + new Vector3(0,5,0));
		}
	}

	void soltarMoronas(){
		if(moronaCompartimiento1 != null){
			moronaCompartimiento1.MovePosition(rb.position);
			moronaCompartimiento1.tag = "MoronaSuelta";
			moronaCompartimiento1 = null;

		}
		if(moronaCompartimiento2 != null){
			moronaCompartimiento2.MovePosition(rb.position);
			moronaCompartimiento2.tag = "MoronaSuelta";
			moronaCompartimiento2 = null;
		}
	}

	void ajustarRotaciónAuto(){
		rotation = transform.localEulerAngles.y;
	}

	void rotarAutoAleatoriamente(){
		float numeroAleatorio = Random.Range(-45.0f, 45.0f);
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
		contadorColaborativo.GetComponent<contadorVerdes>().anadirContadorColaborativo();
	}

	void moverPiedraRecogida(){
		if(sensorPiedraRecogida){
			piedraRecogida.MovePosition(rb.position + new Vector3(0,10,0));
		}
	}

	

    
    //Destroy everything that enters the trigger
	void OnTriggerEnter (Collider col) {
		if(col.gameObject.tag == "Roca")
        {
        	if(sensorPiedraRecogida == false){
        		piedraRecogida = col.GetComponent<Collider>().attachedRigidbody;
            	sensorPiedraRecogida = true;
        	}
            
            soltarMoronas();
        }	
        else if(col.gameObject.tag == "MoronaSuelta" && sensorPiedraRecogida==false){
        	
        	if(moronaCompartimiento1 == null && dirigirseAmoronas == true){
	        	moronaCompartimiento1 = col.GetComponent<Rigidbody>();
	        	moronaCompartimiento1.tag = "Morona";
        	}
        	else if(moronaCompartimiento2 == null && dirigirseAmoronas == true){
	        	moronaCompartimiento2 = col.GetComponent<Rigidbody>();
	        	moronaCompartimiento2.tag = "Morona";
        	}
        	dirigirseAmoronas = false;
        	
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
