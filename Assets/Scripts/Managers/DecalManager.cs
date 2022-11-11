using System.Collections.Generic;
using UnityEngine;

public class DecalManager : Singleton<DecalManager>
{
	[SerializeField] private DecalSetting _decalSetting;
	private Queue<Decal> _decals = new Queue<Decal>();
	private int _decalsCount = 0;

	public void AddDecalToQueue(Decal decal)
	{
		_decals.Enqueue(decal);
		_decalsCount++;
		CheckMaxDecalsCount();
	}

	private void CheckMaxDecalsCount()
	{
		if (_decalsCount >= _decalSetting.MaxDecalsCountInTheScene)
		{
			Destroy(_decals.Dequeue().gameObject);
			_decalsCount--;
		}
	}
}
