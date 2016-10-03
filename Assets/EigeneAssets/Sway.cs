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
            float x = HMD.position.x;
            float z = HMD.position.z;

            float xDiff = x - transform.position.x;
            float zDiff = z - transform.position.z;

            Quaternion target = new Quaternion(0, 0, 0, 1);

            if (Math.Abs(xDiff) <= 1 && Math.Abs(zDiff) <= 1)
            {
                if (Math.Abs(xDiff) > Max)
                {
                    xDiff = Math.Sign(xDiff) * Max;
                }

                if (Math.Abs(zDiff) > Max)
                {
                    zDiff = Math.Sign(zDiff) * Max;
                }

                target = new Quaternion(zDiff, 0, -xDiff, 1);
            }

            Quaternion current = transform.rotation;

            transform.rotation = Quaternion.Slerp(current, target, Speed * Time.deltaTime);
        }
    }
}