using System;
using UnityEngine;

public class Sway : MonoBehaviour
{
	public float Max = 0.1f;
	public float Speed = 1;

	public Transform HMD;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (HMD == null)
		{
			SteamVR_TrackedObject[] trackedObjects = FindObjectsOfType<SteamVR_TrackedObject>();

			foreach (SteamVR_TrackedObject tracked in trackedObjects)
			{
				if (tracked.index == SteamVR_TrackedObject.EIndex.Hmd)
				{
					HMD = tracked.transform;
					break;
				}
			}
		}

		if (HMD != null)
		{
			Debug.Log ("Found HMD");

			float x = HMD.position.x;
			float z = HMD.position.z;

			if (Math.Abs(x) > Max)
			{
				x = Math.Sign(x) * Max;
			}

			if (Math.Abs(z) > Max)
			{
				z = Math.Sign(z) * Max;
			}

			Quaternion current = transform.rotation;

			Quaternion target = new Quaternion(z, 0, -x, 1);

			transform.rotation = Quaternion.Slerp(current, target, Speed * Time.deltaTime);
		}
	}
}