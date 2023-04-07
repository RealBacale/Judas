using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject healthBarObj;
    [SerializeField] private GameObject healthTxtObj;

    private Image healthBar;
    private TMP_Text healthTxt;

    // Start is called before the first frame update
    void Start()
    {
        healthBar = healthBarObj.GetComponent<Image>();
        healthTxt = healthTxtObj.GetComponent<TMP_Text>();
    }

    public void UpdatePlayerHealth(float currentHealt, float maxHealth)
    {
        print("UPDATE UI C_" + currentHealt + "_M_" + maxHealth);
        healthTxt.text = currentHealt.ToString();
        float healthPercentage = 0;
        if(maxHealth > 0 && currentHealt >= 0)
            healthPercentage = currentHealt / maxHealth;
        healthBar.fillAmount = healthPercentage;
    }
}
