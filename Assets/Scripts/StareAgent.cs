using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StareAgent : MonoBehaviour
{
    public StarePlayer StarePlayer;
    public ControlTempo ControlTempo;

    private float minStareDuration = 1.0f;
    private float maxStareDuration = 3.0f;
    private bool isStaring = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Guitarist")
        {
            float stareDuration = Random.Range(minStareDuration, maxStareDuration);

            isStaring = true;

            StartCoroutine(WaitForStareDuration(stareDuration));
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Guitarist")
        {
            isStaring = false;
            StopAllCoroutines();
            StarePlayer.Return();
            ControlTempo.IsTempoAdjustable = false;
        }
    }


    IEnumerator WaitForStareDuration(float stareDuration)
    { 
        yield return new WaitForSeconds(stareDuration);

        if (isStaring)
        {
            StarePlayer.Stare();
            ControlTempo.IsTempoAdjustable = true;
        }
    }
}
