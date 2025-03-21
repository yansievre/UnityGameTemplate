﻿using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageAnimator2 : MonoBehaviour {
	public byte currSequence = 0;

	[SerializeField] bool startWithRandom = true;
	[SerializeField] bool stopAfterAnim = false;
	[SerializeField] float secondsForOneSprite = 0.35f;
	[SerializeField] Sprite[] sprites0 = null;
	[SerializeField] Sprite[] sprites1 = null;
	[SerializeField] Sprite[] sprites2 = null;
	[ReadOnly] [SerializeField] Image img = null;

	byte currSprite = 0;
	float time = 0;

#if UNITY_EDITOR
	private void OnValidate() {
		if (img == null)
			img = GetComponent<Image>();
	}
#endif

	private void Awake() {
		Reinit();
	}

	void Update() {
		time += Time.deltaTime;
		if(time >= secondsForOneSprite) {
			time -= secondsForOneSprite;
			++currSprite;
			if (currSprite == GetCurrSequence().Length) {
				if (stopAfterAnim) {
					enabled = false;
					return;
				}
				currSprite = 0;
			}
			img.sprite = GetCurrSequence()[currSprite];
		}
	}

	public float GetDuration() {
		return secondsForOneSprite * GetCurrSequence().Length + secondsForOneSprite / 2;
	}

	public void SetSprites(Sprite[] _sprites, int id) {
		switch (id) {
			case 0:
				sprites0 = _sprites;
				break;
			case 1:
				sprites1 = _sprites;
				break;
			case 2:
				sprites2 = _sprites;
				break;
		}

		Reinit();
	}

	public void SetSpritesDublicateInner(Sprite[] _sprites, int id) {
		List<Sprite> list = new List<Sprite>(_sprites.Length * 2 - 2);
		list.AddRange(_sprites);
		for (int i = _sprites.Length - 2; i >= 1; --i)
			list.Add(_sprites[i]);

		switch (id) {
			case 0:
				sprites0 = list.ToArray();
				break;
			case 1:
				sprites1 = list.ToArray();
				break;
			case 2:
				sprites2 = list.ToArray();
				break;
		}

		Reinit();
	}

	void Reinit() {
		if (startWithRandom) {
			currSprite = (byte)Random.Range(0, GetCurrSequence().Length);
			time = Random.Range(0, secondsForOneSprite - Time.deltaTime);
		}
		
		img.sprite = GetCurrSequence()[currSprite];
	}

	Sprite[] GetCurrSequence() {
		switch (currSequence) {
			case 0:
				return sprites0;
			case 1:
				return sprites1;
			case 2:
				return sprites2;
		}
		return sprites0;
	}
}
