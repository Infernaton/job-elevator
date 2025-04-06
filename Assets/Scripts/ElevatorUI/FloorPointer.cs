using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorPointer : MonoBehaviour
{
    [SerializeField] Transform m_PassengerPosition;
    [SerializeField] float m_OffsetPosition;
    [SerializeField] AnimationCurve m_MovementCurve;

    private List<Passenger> _passengersAtFloor = new();
    public bool ArePassengersWaiting() => _passengersAtFloor.Count > 0;
    public Passenger GetFirstPassengerWaiting() => _passengersAtFloor[0];

    public IEnumerator MovePassenger(Passenger passenger, Vector3 to)
    {
        yield return Animation.Slide(passenger.gameObject, to, .2f, m_MovementCurve);
    }

    public void SetNewPassenger(Passenger newPassenger)
    {
        Vector3 targetPos = m_PassengerPosition.position + (_passengersAtFloor.Count * m_OffsetPosition * Vector3.right);
        // Move Set Far away the passenger for the animation
        newPassenger.transform.position = targetPos + Vector3.right * 5f;
        StartCoroutine(MovePassenger(newPassenger, targetPos));

        _passengersAtFloor.Add(newPassenger);
    }

    public void RemovePassenger(Passenger passenger)
    {
        _passengersAtFloor.Remove(passenger);

        ResetPassengersPosition();
    }

    private void ResetPassengersPosition()
    {
        for (int i = 0; i < _passengersAtFloor.Count; i++)
        {
            var p = _passengersAtFloor[i];
            StartCoroutine(MovePassenger(p, m_PassengerPosition.position + (i * m_OffsetPosition * Vector3.right)));
        }
    }
}
