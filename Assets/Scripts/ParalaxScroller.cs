using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class ParalaxScroller : MonoBehaviour
{
	[SerializeField] Transform follower;
	[SerializeField] Vector3 multiplier;

	Vector3 startOffset;

	void Start()
	{
		startOffset = follower.position - transform.position;
	}

	Vector3 multiply(Vector3 a, Vector3 b)
	{
		return new Vector3(a.x * b.x, a.y * b.y,a.z * b.z);
	}

	void Update()
	{
		Vector3 p = follower.position;

		transform.position = follower.position - startOffset - (multiply(follower.position, multiplier));
	}
}
