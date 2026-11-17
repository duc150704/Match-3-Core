using UnityEngine;
using UnityEngine.InputSystem;

public class MouseFollower : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float _followSpeed = 10f;

    private bool _follow = true;

    private float _distanceToTarget;
    private float _knockBackForce;

    private Vector3 _knockBackDirection;
    private Vector3 _directionToTarget;

    public float DistanceToTarget => _distanceToTarget;

    private void Update()
    {
        if (_follow == false || _camera == null)
            return;

        Vector3 mousePos = _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePos.z = 0;
        
        _directionToTarget = mousePos - transform.position;
        _distanceToTarget = _directionToTarget.magnitude;

        transform.position += 
             (_directionToTarget * _followSpeed + _knockBackDirection * _knockBackForce) 
            * Time.deltaTime;
        

    }

    public void KnockBack()
    {
        
    }
}
