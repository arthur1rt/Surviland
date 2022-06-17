using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    private float maxLife = 100;
    private float minLife = 0;
    private float currentLife = 50;

    public float life => currentLife;

    public void ChangeLife(float qty)
    {
        currentLife = Mathf.Clamp(currentLife + qty, minLife, maxLife);
    }
}
