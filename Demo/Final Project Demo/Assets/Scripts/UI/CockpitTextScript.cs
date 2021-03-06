using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CockpitTextScript : MonoBehaviour
{
	private Text shipPartsText;

    public static bool found = false;

    // Start is called before the first frame update
    void Start()
    {
        shipPartsText = GetComponent<Text>() as Text;
    }

    // Update is called once per frame
    void Update()
    {
        if (found) {
            shipPartsText.color = Color.green;
        } else {
            shipPartsText.color = Color.red;
        }
        shipPartsText.text = "Cockpit";
    }

    public static void setToFound() {
				Debug.Log("Set To Found");
        found = true;
    }

    public static bool getFoundStatus() {
        return found;
    }
}
