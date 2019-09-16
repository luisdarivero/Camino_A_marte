using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class contadorVerdes : MonoBehaviour
{

	int contadorIndividual;
	int contadorColaborativo;
    // Start is called before the first frame update
    void Start()
    {
        contadorIndividual = 0;
        contadorColaborativo = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void anadirContadorIndividual(){
    	contadorIndividual ++;
    	
    }

    public void anadirContadorColaborativo(){
    	contadorColaborativo ++;
    }

    void OnGUI()
	{
		//show the GUI for the speed and gear we are on.
		GUI.Box(new Rect(10,10,250,30),"contador Verdes (Individual): " + contadorIndividual);

		GUI.Box(new Rect(10,70,250,30),"contador Naranjas (Colaborativo): " + contadorColaborativo);
		
	}
}
