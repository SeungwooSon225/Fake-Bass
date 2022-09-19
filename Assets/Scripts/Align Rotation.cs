using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignRotation : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject mController;

    [SerializeField]
    GameObject mTarget;

    [SerializeField]
    Vector3 mOffsetVector;

    void LateUpdate() 
    {
        if(mController != null || mTarget != null)
        {
            mTarget.transform.rotation = mController.transform.rotation
                * Quaternion.Euler(mOffsetVector);
        }    
    }
}
