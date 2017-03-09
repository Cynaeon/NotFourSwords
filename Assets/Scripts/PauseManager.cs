using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour {
    public bool isPaused;
    private enum Pauser
    {
        P1,
        P2,
        P3,
        P4
    }
    private Pauser _pauser;

    void Awake () {
		
	}
	
	void Update () {
        if (Input.GetButtonDown("P1_Pause"))
        {
            if (!isPaused)
            {
                isPaused = true;
                _pauser = Pauser.P1;
                Time.timeScale = 0;
            }
            else
            {
                if (_pauser == Pauser.P1)
                {
                    isPaused = false;
                    Time.timeScale = 1;
                }
            }
        }

        if (Input.GetButtonDown("P2_Pause"))
        {
            if (!isPaused)
            {
                isPaused = true;
                _pauser = Pauser.P2;
                Time.timeScale = 0;
            }
            else
            {
                if (_pauser == Pauser.P2)
                {
                    isPaused = false;
                    Time.timeScale = 1;
                }
            }
        }

        if (Input.GetButtonDown("P3_Pause"))
        {
            if (!isPaused)
            {
                isPaused = true;
                _pauser = Pauser.P3;
                Time.timeScale = 0;
            }
            else
            {
                if (_pauser == Pauser.P3)
                {
                    isPaused = false;
                    Time.timeScale = 1;
                }
            }
        }

        if (Input.GetButtonDown("P4_Pause"))
        {
            if (!isPaused)
            {
                isPaused = true;
                _pauser = Pauser.P4;
                Time.timeScale = 0;
            }
            else
            {
                if (_pauser == Pauser.P4)
                {
                    isPaused = false;
                    Time.timeScale = 1;
                }
            }
        }
    }
}
