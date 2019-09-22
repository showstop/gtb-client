using UnityEngine;
using System.Collections;

public class GachaShop : PopupComponent
{
    [SerializeField]
    private PopupComponent OnceGachaGO;

    [SerializeField]
    private PopupComponent TensGachaGO;

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void OnceGachaClick()
    {
        OnceGachaGO.gameObject.SetActive(true);
        OnceGachaGO.Open();
    }
    public void OnceGachaClose()
    {
        OnceGachaGO.Close();
        OnceGachaGO.gameObject.SetActive(false);
    }

    public void TensGachaClick()
    {
        TensGachaGO.gameObject.SetActive(true);
        TensGachaGO.Open();
    }
    public void TensGachaClose()
    {
        TensGachaGO.Close();
        TensGachaGO.gameObject.SetActive(false);
    }


}
