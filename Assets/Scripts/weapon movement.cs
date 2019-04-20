using System.Collections;
using UnityEngine;

public class weaponmovement : MonoBehaviour {

    public int rotationOffset = 0;

	// Update is called once per frame
	void Update () {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();     //Normalize so vector equals 1

        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + rotationOffset);
	}
}
