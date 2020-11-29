using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using URD = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class SFXPlayer : InstanceBehaviour<SFXPlayer> {
	
	public List<SFXCollection> sfxCollection;
	
	public AudioSource audioSource;
	
	private Dictionary<string, SFXCollection> dict;
	
	public static void PlayClip(AudioClip clip, Vector3 position, float volume =1, float pitch =1) {
		var source = Instance.audioSource;
		Instance.transform.position = position;
		
		source.pitch = pitch;
		source.PlayOneShot(clip, volume);
	}
	
	public static void PlaySFX(string sfxName, Vector3 position, float volume = 1, float pitch = 1) {
		var audio = Rand.PickFrom(Instance.dict[sfxName].audio);
		PlayClip(audio, position, volume, pitch);
	}
	
	protected override void Awake() {
		base.Awake();
		audioSource = GetComponent<AudioSource>();
	}
	
	// Use this for initialization
	void Start () {
		dict = sfxCollection.ToDictionary(x=>x.name);
	}
	
	
}
