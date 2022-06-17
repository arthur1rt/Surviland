using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject healthBar;
    public GameObject healthText;

    private float maxLife = 100;
    private float minLife = 0;
    private float currentLife = 100;

    // Start is called before the first frame update
    void Start()
    {
        ChangeLife(-50);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeLife(float qty)
    {
        currentLife = Mathf.Clamp(currentLife + qty, minLife, maxLife);

        float newScale = Mathf.Clamp(currentLife / maxLife, 0, 1);
        healthBar.transform.localScale = new Vector3(newScale, 1, 1);

        healthText.GetComponent<Text>().text = currentLife + "/100";
    }
}
