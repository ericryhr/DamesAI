using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    public Animator anim;
	public List<GameObject> objs;

	public void FadeIn()
	{
		anim.Play("FadeIn");
		foreach (GameObject obj in objs)
		{
			obj.SetActive(false);
		}
	}
}
