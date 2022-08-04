using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class VRControllerInputManager : MonoBehaviour
{
    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean RightPress;
    public SteamVR_Action_Boolean LeftPress;


    public bool RightPressed()
    {
        return RightPress.GetStateDown(handType);
    }


    public bool LeftPressed()
    {
        return LeftPress.GetStateDown(handType);
    }
}
