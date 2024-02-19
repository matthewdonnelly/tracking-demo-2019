/* Example level loader */
using UnityEngine;
using System.Collections;

public class DemoGUI : MonoBehaviour {

    public Main main;
            
    void OnGUI ()
    {
        // Make a background box
        GUI.Box(new Rect(10,10,150,130), "Menu");
    
        // Make the first button. If it is pressed, ___
        if(GUI.Button(new Rect(20,40,130,20), "Start Demo"))
        {
            main.StartDemo();
        }

        if(GUI.Button(new Rect(20,70,130,20), "Show Line Map")) 
        {
            main.ShowLineMap();
        }

        if(GUI.Button(new Rect(20,100,130,20), "Compare Rotations")) 
        {
            main.CompareRotations();
        }
    

    }
}