using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ROOM {SMALL, MEDIUM, LARGE}
public class GeneralRoom : MonoBehaviour
{
    ROOM room = 0;
  
    public void ChangeRoom(int newRoom)
    {
        room = (ROOM)newRoom;
    }
    public void Done()
    {
        Debug.Log("La medida de tu habitación es: " + room);
    }
    
}
