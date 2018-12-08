using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class button_help : MonoBehaviour, IPointerClickHandler
{

    public GameObject introduction;

    public void OnPointerClick(PointerEventData eventData)
    {
        introduction.gameObject.SetActive(true);
    }
    // Use this for initialization
    void Start () {
        introduction.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {

	}
}
