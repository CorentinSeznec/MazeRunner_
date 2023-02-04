using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{
    private Vector3 center;
    [SerializeField] GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        center = new Vector3(0, 0, 0);
    }

    private void ComputeRotation()
    {
        Vector3 actualPosition = player.transform.position;

        Vector3 diff = center - actualPosition;
        diff.y = 0;

        Vector3 inFront = player.transform.forward;
        inFront.y = 0;

        float rotation = Vector3.SignedAngle(diff, inFront, Vector3.up);
        transform.rotation = Quaternion.Euler(0, 0, rotation);

    }

    // Update is called once per frame
    void Update()
    {
        ComputeRotation();
    }
}
