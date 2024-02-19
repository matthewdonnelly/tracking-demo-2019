using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;


public class Main : MonoBehaviour
{

    public Transform headset;
    public Waypoints[] waypoints;
    public LineRenderer lineMap;
    public Text welcomeMessage;
    public Text endMessage;

    int m_waypointIndex;

    bool m_demoStarted = false;
    bool m_waypointReached = false;
    bool m_allWaypointsReached;
    bool m_recordingComplete;

    float m_time;
    float m_timeOfLastRecording;
    float m_timeSinceLastWaypoint;


    Quaternion m_rotation;
    Vector3 m_position;

    float m_startingRotationY;

    ArrayList m_timeToWaypoints = new ArrayList();

    List<Vector3> m_positionsList = new List<Vector3>();
    


    void RecordData()
    {

        //Path of the file
        string path = Application.dataPath + "/Data.txt";

        //Create file if it doesn't exist
        if (!File.Exists(path))
        {
            File.WriteAllText(path, "Data Log: Total time elapsed, (position), (rotation), Time elapsed since last waypoint, time to current waypoint  \n\n");
        }
        
        //Content to be added to the file
        string timeToWaypoint = "";

        if(m_waypointReached)
        {
            timeToWaypoint = m_timeToWaypoints[m_waypointIndex - 1].ToString();
            m_waypointReached = false;
        }
        
        else
        {
            timeToWaypoint = "n/a";
        }
        
        string content = m_time + "," + m_position + "," + m_rotation + "," + m_timeSinceLastWaypoint + "," + timeToWaypoint + "\n"; 
        
        if (m_allWaypointsReached)
        {
            content += "\n[SUMMARY] Total Time: " + m_time + "s.  Time to each wapoint: " +  "[" + string.Join(",", m_timeToWaypoints.ToArray()) + "]\n\n====================================================================\n\n";
            m_recordingComplete = true;
        }

        //Add content to file
        File.AppendAllText(path, content);

        //updates timeOfLastRecording to current time
        m_timeOfLastRecording = m_time;

    }

    void Start()
    {
        //hides waypoints until demo is started
        foreach(Waypoints waypoint in waypoints)
        {
            waypoint.gameObject.SetActive(false);
        }

        //hides lineMap until ShowLineMap is called
        lineMap.gameObject.SetActive(false);


    }

    //to be called once player understands and is ready
    public void StartDemo()
    {
        m_demoStarted = true;

        //remembers player's starting y rotation
        m_startingRotationY = headset.rotation.eulerAngles.y;

        //PlayerGUI deactivated
        welcomeMessage.gameObject.SetActive(false);

        //all timers set to 0 on start
        m_time = 0f;
        m_timeOfLastRecording = 0f;
        m_timeSinceLastWaypoint = 0f;

        //start waypointIndex at 0
        m_waypointIndex = 0;

        //activate first waypoint
        waypoints[0].gameObject.SetActive(true);

        m_allWaypointsReached = false;
        m_recordingComplete = false;
    }


    void Update()
    {
        if (m_demoStarted)
        {
            //gets position and rotation of headset
            m_position = headset.position; 
            m_rotation = headset.rotation; 


            //updates time and timeSinceLastWaypoint
            m_time += Time.deltaTime;
            m_timeSinceLastWaypoint += Time.deltaTime;


            //records data every 0.25 seconds
            if (m_time - m_timeOfLastRecording > 0.25) 
            {
                if (!m_recordingComplete)
                {
                    RecordData();

                    //update positionsList for LineMap
                    m_positionsList.Add(m_position);
                    

                }
            }
        }

    }


    //called when player passes through a waypoint
    public void WaypointReached()
    {

        m_waypointReached = true;
        //records time it took player to reach this waypoint
        m_timeToWaypoints.Add(m_timeSinceLastWaypoint);

        //resets timeSinceLastWaypoint
        m_timeSinceLastWaypoint = 0;

        //reached waypoint disappears
        waypoints[m_waypointIndex].gameObject.SetActive(false); 

        //increments waypointIndex and makes the next waypoint appear
        m_waypointIndex++;
        if(m_waypointIndex < waypoints.Length)
        {
            waypoints[m_waypointIndex].gameObject.SetActive(true);
        }
        else
        {
            m_allWaypointsReached = true;   
        }

    }

    public void ShowLineMap()
    {
        //reactivates LineMap
        lineMap.gameObject.SetActive(true);

        //set positions in LineMap to the completed positionsList
        lineMap.positionCount = m_positionsList.Count;
        lineMap.SetPositions(m_positionsList.ToArray());
    }

    //to be called after player has been told to look in the direction that they were facing at the start of the demo
    public void CompareRotations()
    {
        float difference = Mathf.Abs(m_startingRotationY - m_rotation.eulerAngles.y);
        bool accurate = difference < 15;
        
        //display result message
        endMessage.gameObject.SetActive(true);
        if(accurate)
        {
            endMessage.text = "Congratulations! You looked in the right direction. You were only " + difference.ToString("F") + " degrees off.";

        }
        else
        {
            endMessage.text = "You were " + difference.ToString("F") + " degrees off.";

        }

        
    }

}
