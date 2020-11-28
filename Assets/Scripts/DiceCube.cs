﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceCube : MonoBehaviour {

    public int value = 6;
    public int iterations = 10;
    public float iterationDelay = 0.2f;
    public Transform[] faces;
    public bool isRolling;
    private GameManager gm;

    private void Start()
    {
        gm = GameManager.instance;
    }

    public IEnumerator Roll()
    {
        int randomValue = value;
        isRolling = true;
        gm.waitingForRoll = false;
        for (int i = 0; i < iterations; i++)
        {
            while (randomValue == value)
                randomValue = Random.Range(1, 7);           //max number is exclusive (only 1-6)
            value = Random.Range(1, 7);
            for (int j = 0; j < 6; j++)
            {
                faces[j].gameObject.SetActive( (j+1 == value) );
            }
            yield return new WaitForSeconds(iterationDelay);
        }
        isRolling = false;
        gm.BeginTurn();
    }

}
