using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHideUi : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(false);
    }
    public void OnButtonPressed()
    {
        gameObject.SetActive(false);
    }
}
