using UnityEngine;
using System.Collections;

public class Fireworks : MonoBehaviour {

	// A list of the sounds to play when launching a rocket.
	public AudioClip[] mortarSounds;

	// A list of the sounds to play when a rocket explodes.
	public AudioClip[] explodeSounds;

	// A list of all our mortar tubes.
	public ParticleSystem[] mortars;
	
	private ParticleSystem.Particle[] particles = new ParticleSystem.Particle[1000];
	private Vector3 listenerPosition;
	private float speedOfSound = 343.2f;
	private AudioClip clip;
	private float distance;
	
	void Start () {
		listenerPosition = Camera.main.transform.position;
	}
	
	void Update() {
		foreach (ParticleSystem mortar in mortars) {
			int length = mortar.GetParticles(particles);
			int i = 0;
			while (i < length) {
				if (particles[i].remainingLifetime < Time.deltaTime) {
					clip = explodeSounds[Random.Range(0, explodeSounds.Length)];
					distance = Vector3.Distance(particles[i].position, listenerPosition);
					StartCoroutine(PlayDelayedSound(clip, distance, "explosion"));
				}
				i++;
			}
		}
	}

	IEnumerator PlayDelayedSound(AudioClip clip, float distance, string type) {
		float waitTime = distance / speedOfSound;
        
        yield return new WaitForSeconds(waitTime);

		if (type == "mortar") {
			GetComponent<AudioSource>().volume = 0.6f;
        	GetComponent<AudioSource>().pitch = Random.Range(0.6f, 0.8f);
		}
		else {
			GetComponent<AudioSource>().pitch = Random.Range(0.5f, 1.5f);
		}
        
        GetComponent<AudioSource>().PlayOneShot(clip);
    }

	// Launches a rocket from the given mortar tube.
	// We call this method from the attached animation clip.
	// This lets us compose a fireworks sequence using the animation view.
	void LaunchRocket (int mortarTube) {
		mortars[mortarTube].Emit(1);
		distance = Vector3.Distance(mortars[mortarTube].transform.position, listenerPosition);
		clip = mortarSounds[Random.Range(0, mortarSounds.Length)];
		StartCoroutine(PlayDelayedSound(clip, distance, "mortar"));
    }
}
