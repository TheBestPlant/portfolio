using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	public AudioSource BGM;

	void Start () {
		DontDestroyOnLoad (gameObject);
	}
}
