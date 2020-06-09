﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    public float HealthPoints
    {
        get
        {
            return healthPoints;
        }
        set
        {
            healthPoints = value;

            // What if we reach 0?
            if(healthPoints <= 0)
            {
                Destroy(GameObject.FindGameObjectWithTag("Player"));
            }
        }
    }

    [SerializeField]
    private float healthPoints = 100f;
}
