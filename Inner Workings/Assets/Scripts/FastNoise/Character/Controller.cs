using UnityEngine;
using System.Collections;

[System.Serializable]
public class Controller : MonoBehaviour
{
    // All of this is a hacky way of using C# properties while maintaining SerializeField benefits
    [SerializeField]
    private string _faction;
    public string Faction { get { return _faction; } set { _faction = value; } }

    [SerializeField]
    private float _health;
    public float Health { get { return _health; } set { _health = value; } }
    [SerializeField]
    private float _healthmax;
    public float HealthMax { get { return _healthmax; } set { _healthmax = value; } }
    private float healthRegenCurrent;
    [SerializeField]
    private float _healthregentime;
    public float HealthRegenTime { get { return _healthregentime; } set { _healthregentime = value; } }
    [SerializeField]
    private float _healthregenmax;
    public float HealthRegenMax { get { return _healthregenmax; } set { _healthregenmax = value; } }

    [SerializeField]
    private Vector3 _movement;
    public Vector3 Movement { get { return _movement; } set { _movement = value; } }
    [SerializeField]
    private float _movementbackstepmult;
    public float MovementBackstepMult { get { return _movementbackstepmult; } set { _movementbackstepmult = value; } }
    [SerializeField]
    private float _movementsprintmult;
    public float MovementSprintMult { get { return _movementsprintmult; } set { _movementsprintmult = value; } }

    // Time control stuff
    private Vector3 lastPosition;
    public float Timestep { get; set; }

    // Use this for initialization
    protected virtual void Start()
    {
        Timestep = 1;
        lastPosition = transform.position;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        // This is my hacky way of controlling 'time' for individual objects, by lerping them from their previous position towards their current position based on Timestep
        transform.position = Vector3.Lerp(lastPosition, transform.position, Timestep);

        Health = Mathf.SmoothDamp(Health, HealthMax, ref healthRegenCurrent, HealthRegenTime, HealthRegenMax);

        Timestep = Mathf.Lerp(Timestep, 1, 0.01f);

        lastPosition = transform.position;
    }
}
