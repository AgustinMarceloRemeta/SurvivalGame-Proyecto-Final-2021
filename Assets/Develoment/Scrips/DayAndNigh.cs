using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayAndNigh : MonoBehaviour
{
    public GameObject luz;
    public float time, MaxTime, TimeTicks;
    public GameObject luz1, luz2, luz3, luz4;

    void Update()
    {
        if (time < MaxTime) time += Time.deltaTime / TimeTicks;
        luz.transform.rotation = Quaternion.Euler(time, 0, 0);
        if (time >= 360) time = 0;
        if (time > 190 && time < 350) {
            luz1.SetActive(true);
            luz2.SetActive(true);
            luz3.SetActive(true);
            luz4.SetActive(true);
                }
        else
        {
            luz1.SetActive(false);
            luz2.SetActive(false);
            luz3.SetActive(false);
            luz4.SetActive(false);
        }
    }
}
