using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorPointer : MonoBehaviour
{
    [SerializeField] Transform m_PassengerPosition;
    [SerializeField] float m_OffsetPosition;

    private List<Passenger> _passengersAtFloor = new();
    public bool ArePassengersWaiting() => _passengersAtFloor.Count > 0;
    public Passenger GetFirstPassengerWaiting() => _passengersAtFloor[0];

    public void SetNewPassenger(Passenger newPassenger)
    {
        newPassenger.transform.position = m_PassengerPosition.position + (_passengersAtFloor.Count * m_OffsetPosition * Vector3.right);

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

            p.transform.position = m_PassengerPosition.position + (i * m_OffsetPosition * Vector3.right);
        }
    }
}
