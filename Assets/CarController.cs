using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class CarController : MonoBehaviour
{
    public Vector2 SensingCastSize = new Vector2(8, 10);
    public Vector2 CarSensingCastSize = new Vector2(2, 10);
    public float SensingZOffset = 3f;
    public Vector3 TargetPos;
    public bool IsDriving = true;
    public bool IsObstructed = false;

    private float _speed = 0f;
    public float MaxSpeed = 10f;
    public float MaxAccel = 7.5f;

    private void Awake()
    {
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    private float _scaleT = 0f;
    public AnimationCurve ScaleCurve;

    // Update is called once per frame
    void Update()
    {
        if (IsDriving)
        {
            // align with the target position
            Vector3 diff = TargetPos - transform.position;
            Vector3 targetRot = Vector3.RotateTowards(transform.forward, diff.normalized, 10f * Time.deltaTime, 0f);
            transform.rotation = Quaternion.LookRotation(targetRot, Vector3.up);
            if (diff.magnitude < 0.25f)
                IsDriving = false;

            Collider[] cols = Physics.OverlapBox(transform.position + transform.forward * (SensingZOffset + SensingCastSize.y / 2), new Vector3(SensingCastSize.x / 2, 1.5f, SensingCastSize.y / 2), transform.rotation);
            Collider[] carCols = Physics.OverlapBox(transform.position + transform.forward * (SensingZOffset + CarSensingCastSize.y / 2), new Vector3(CarSensingCastSize.x / 2, 1.5f, CarSensingCastSize.y / 2), transform.rotation);
            bool _found = false;
            if (cols != null)
            {
                foreach (Collider col in cols)
                {
                    if (col.tag == "Pedestrian")
                    {
                        _found = true;
                        break;
                    }
                }
            }
            if (carCols != null)
            {
                foreach (Collider col in carCols)
                {
                    if (col.tag == "Car")
                    {
                        _found = true;
                        break;
                    }
                }
            }
            IsObstructed = _found;
        }
        else
        {
            _scaleT += Time.deltaTime;
            transform.localScale = Vector3.one * ScaleCurve.Evaluate(_scaleT);
            if (_scaleT > ScaleCurve.keys[ScaleCurve.keys.Length - 1].time)
            {
                Destroy(gameObject);
            }
        }

        if (!IsObstructed)
        {
            if (_speed < MaxSpeed)
                _speed += MaxAccel * Time.deltaTime;
        }
        else
        {
            if (_speed > 0)
                _speed -= MaxAccel * Time.deltaTime * 2.5f;
        }
        transform.position += transform.forward * _speed * Time.deltaTime;
        _speed = Mathf.Clamp(_speed, 0f, MaxSpeed);
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 center = new Vector3(0, 0, SensingZOffset + SensingCastSize.y / 2);
        Gizmos.DrawWireCube(transform.TransformPoint(center), new Vector3(SensingCastSize.x, 2, SensingCastSize.y));
    }
}
