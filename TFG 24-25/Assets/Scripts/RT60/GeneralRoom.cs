using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio; 

public enum ROOM { SMALL, MEDIUM, LARGE }


public class GeneralRoom : MonoBehaviour
{
    ROOM room = ROOM.SMALL;

    // Diccionari amb les dades acústiques: volum, absorció, RT60
    Dictionary<ROOM, (int volume, float absorption, float t60)> roomData = new Dictionary<ROOM, (int, float, float)>()
    {
        { ROOM.SMALL, (36, 2.39f, 2.43f) },
        { ROOM.MEDIUM, (60, 3.48f, 2.77f) },
        { ROOM.LARGE, (90, 4.76f, 3.04f) }
    };

       public void ChangeRoom(int newRoom)
    {
        room = (ROOM)newRoom;
    }

    public void Done()
    {

        float previousValue;

        RuntimeManager.StudioSystem.getParameterByName("RT60", out previousValue);
        Debug.Log("Valor previo de RT60: " + previousValue);

        var (volume, absorption, t60) = roomData[room];

        Debug.Log($"Has seleccionat: {room}. Volum: {volume} m³, Absorció: {absorption} Sabines, RT60: {t60}s");

        // Enviar el valor de RT60 a FMOD
       RuntimeManager.StudioSystem.setParameterByName("RT60", t60);



        float currentValue;

  

    
        RuntimeManager.StudioSystem.getParameterByName("RT60", out currentValue);
        Debug.Log("Valor actual de RT60: " + currentValue);

    }
}
