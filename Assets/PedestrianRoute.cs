using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

public class PedestrianRoute : MonoBehaviour
{
    public List<Transform> wps;
    public List<Transform> route;
    public int routeNumber = 0;
    int targetWP = 0;

    private bool _enabled = false;
    private float _enabledT = 0f;

    private const int WAYPOINT_COUNT = 8;

    private Rigidbody _rb;

    private void Awake()
    {
        _enabledT = Random.Range(2f, 12f);
        _rb = GetComponent<Rigidbody>();
        for (int i = 1; i <= WAYPOINT_COUNT; i++)
            wps.Add(GameObject.Find($"WP{i}").transform);
    }

    // Start is called before the first frame update
    void Start()
    {
        SetRoute();
        transform.position = new Vector3(0f, -5f, 0f);
    }

    private void Update()
    {
        if (!_enabled)
        {
            _enabledT -=Time.deltaTime;
            if (_enabledT < 0)
                _enabled = true;
        }
    }

    void FixedUpdate()
    {
        if (_enabled)
        {
            Vector3 displacement = route[targetWP].position - transform.position;
            displacement.y = 0;
            float dist = displacement.magnitude;

            if (dist < 0.1f)
            {
                targetWP++;
                if (targetWP >= route.Count)
                {
                    SetRoute();
                    return;
                }
            }

            //calculate velocity for this frame
            Vector3 velocity = displacement;
            velocity.Normalize();
            velocity *= 2.5f;
            //apply velocity
            Vector3 newPosition = transform.position;
            newPosition += velocity * Time.deltaTime;
            _rb.MovePosition(newPosition);
            //align to velocity
            Vector3 desiredForward = Vector3.RotateTowards(transform.forward, velocity,
            10.0f * Time.deltaTime, 0f);
            Quaternion rotation = Quaternion.LookRotation(desiredForward);
            _rb.MoveRotation(rotation);
        }
    }

    void SetRoute()
    {
        //randomise the next route
        routeNumber = Random.Range(0, 12);
        //set the route waypoints
        if (routeNumber == 0) route = new List<Transform> { wps[0], wps[4],
wps[5], wps[6] };
        else if (routeNumber == 1) route = new List<Transform> { wps[0], wps[4],
wps[5], wps[7] };
        else if (routeNumber == 2) route = new List<Transform> { wps[2], wps[1],
wps[4], wps[5], wps[6] };
        else if (routeNumber == 3) route = new List<Transform> { wps[2], wps[1],
wps[4], wps[5], wps[7] };
        else if (routeNumber == 4) route = new List<Transform> { wps[3], wps[4],
wps[5], wps[6] };
        else if (routeNumber == 5) route = new List<Transform> { wps[3], wps[4],
wps[5], wps[7] };
        else if (routeNumber == 6) route = new List<Transform> { wps[6], wps[5],
wps[4], wps[0] };
        else if (routeNumber == 7) route = new List<Transform> { wps[6], wps[5],
wps[4], wps[3] };
        else if (routeNumber == 8) route = new List<Transform> { wps[6], wps[5],
wps[4], wps[1], wps[2] };
        else if (routeNumber == 9) route = new List<Transform> { wps[7], wps[5],
wps[4], wps[0] };
        else if (routeNumber == 10) route = new List<Transform> { wps[7],
wps[5], wps[4], wps[3] };
        else if (routeNumber == 11) route = new List<Transform> { wps[7],
wps[5], wps[4], wps[1], wps[2] };
        //initialise position and waypoint counter
        transform.position = new Vector3(route[0].position.x, 0.0f,
        route[0].position.z);
        targetWP = 1;
    }
}