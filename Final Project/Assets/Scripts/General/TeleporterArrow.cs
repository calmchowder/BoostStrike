using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterArrow : MonoBehaviour {

    public GameObject finishPortal;  // Initialized here because it is deactivated at the beginning

    // Update is called once per frame
	void Update () {
        // The offset 15,25,13 is there because the portal's transform is located at the bottom corner. 
        // This points it to the center of the portal
        transform.LookAt(finishPortal.transform.position + new Vector3(15, 25, 13));
	}

}
