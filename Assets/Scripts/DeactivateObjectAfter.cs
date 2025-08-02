using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateObjectAfter : MonoBehaviour
{
    public float statDeactivateTime = 2f; // Time in seconds before the object is deactivated
    private float deactivateTimer = 2;

    void OnEnable()
    {
        deactivateTimer = statDeactivateTime;
    }

    void Update()
    {
        deactivateTimer -= Time.deltaTime;
        if (deactivateTimer <= 0f)
        {
            gameObject.SetActive(false);
        }
    }
}
