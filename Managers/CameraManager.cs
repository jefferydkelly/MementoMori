using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CameraManager
{
    Room currentRoom;
    Camera myCamera;
    float panTime = 0.5f;
    float panTick = 1.0f / 30.0f;
    Timer panTimer;
    public bool IsPanning
    {
        get;
        private set;
    }

    static CameraManager instance;


    public static CameraManager Instance
    {
        get {
            if (instance == null || instance.myCamera == null)
                instance = new CameraManager();
            return instance;
        }
    }

    private CameraManager()
    {
        myCamera = Camera.main;
        panTimer = new Timer(panTick, (int)(panTime / panTick));
        panTimer.OnComplete.AddListener(() =>
        {
            panTimer.OnTick.RemoveAllListeners();
        });
    }
   
    public Room CurrentRoom
    {
        get => currentRoom;
        set
        {
            if (value && value != currentRoom)
            {
               
                
                if (currentRoom)
                {
                    currentRoom.OnExit.Invoke();
                    Vector3 dif = value.transform.position - currentRoom.transform.position;
                    float distPerTick = dif.magnitude * panTick / panTime;
                    panTimer.OnTick.AddListener(()=> {
                        myCamera.transform.position += dif.normalized * distPerTick;
                    });

                    panTimer.OnComplete.AddListener(() =>
                    {
                        value.OnEnter.Invoke();
                        myCamera.transform.position = value.transform.position.SetZ(myCamera.transform.position.z);
                    });

                    panTimer.Start();
                    
                }
                else
                {
                    myCamera.transform.position = value.transform.position.SetZ(myCamera.transform.position.z);
                    value.OnEnter.Invoke();
                }
                currentRoom = value;
            }
        }
    }
}
