using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour {
    public bool isPaused;
	public UIManager p1UI;
	public UIManager p2UI;
	public UIManager p3UI;
	public UIManager p4UI;
    private enum Pauser
    {
        P1,
        P2,
        P3,
        P4
    }
    private Pauser _pauser;
	
	void Update () {
        if (Input.GetButtonDown("P1_Start"))
        {
            if (!isPaused)
            {
                isPaused = true;
                _pauser = Pauser.P1;
                Time.timeScale = 0;
				p2UI.UIPause(true);
				p3UI.UIPause(true);
				p4UI.UIPause(true);
            }
            else
            {
                if (_pauser == Pauser.P1)
                {
                    isPaused = false;
                    Time.timeScale = 1;
					p2UI.UIPause(false);
					p3UI.UIPause(false);
					p4UI.UIPause(false);
                }
            }
        }

        if (Input.GetButtonDown("P2_Start"))
        {
            if (!isPaused)
            {
                isPaused = true;
                _pauser = Pauser.P2;
                Time.timeScale = 0;
				p1UI.UIPause(true);
				p3UI.UIPause(true);
				p4UI.UIPause(true);
            }
            else
            {
                if (_pauser == Pauser.P2)
                {
                    isPaused = false;
                    Time.timeScale = 1;
					p1UI.UIPause(false);
					p3UI.UIPause(false);
					p4UI.UIPause(false);
                }
            }
        }

        if (Input.GetButtonDown("P3_Start"))
        {
            if (!isPaused)
            {
                isPaused = true;
                _pauser = Pauser.P3;
                Time.timeScale = 0;
				p1UI.UIPause(true);
				p2UI.UIPause(true);
				p4UI.UIPause(true);
            }
            else
            {
                if (_pauser == Pauser.P3)
                {
                    isPaused = false;
                    Time.timeScale = 1;
					p1UI.UIPause(false);
					p2UI.UIPause(false);
					p4UI.UIPause(false);
                }
            }
        }

        if (Input.GetButtonDown("P4_Start"))
        {
            if (!isPaused)
            {
                isPaused = true;
                _pauser = Pauser.P4;
                Time.timeScale = 0;
				p1UI.UIPause(true);
				p2UI.UIPause(true);
				p3UI.UIPause(true);
            }
            else
            {
                if (_pauser == Pauser.P4)
                {
                    isPaused = false;
                    Time.timeScale = 1;
					p1UI.UIPause(false);
					p2UI.UIPause(false);
					p3UI.UIPause(false);
                }
            }
        }
    }
}
