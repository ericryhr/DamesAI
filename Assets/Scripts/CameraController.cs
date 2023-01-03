using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
	public void CanviarJugador()
	{
		Vector3 rot = Camera.main.transform.eulerAngles;
		rot.z += 180;
		Camera.main.transform.eulerAngles = rot;
	}
}
