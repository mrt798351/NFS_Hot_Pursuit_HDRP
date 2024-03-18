using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CarController : Car
{
    [SerializeField] private CarModel carModel = null;
    [SerializeField] private CarView carView = null;
    [SerializeField] private GameObject car = null;
    [SerializeField] private Rigidbody rb = null;
    [SerializeField] private bool isAIControllerCar = false;
    [SerializeField] private AI agent = null;
    [SerializeField] private SelfDrivingCar pathFollower = null;
    [SerializeField] private GameObject racingCameraObject = null;
    [SerializeField] private GameObject carHeadlightFlare1 = null;
    [SerializeField] private GameObject carHeadlightFlare2 = null;
    [SerializeField] private GameObject nitrous1 = null;
    [SerializeField] private GameObject nitrous2 = null;
    [SerializeField] private AudioSource gasAudio = null;
    [SerializeField] private AudioSource breakAudio = null;
    [SerializeField] private AudioSource nitrousAudio = null;
    [SerializeField] private AudioSource crashAudio = null;    
    [SerializeField] private GameObject nitrousUI = null;
    [SerializeField] private GameObject interiorView = null;
    [SerializeField] private List<MeshRenderer> bodyMeshRenderers = null;
    [SerializeField] private List<GameObject> bodyGameObjects = null;
    [SerializeField] private MeshCollider bodyCollider = null;
    private bool nitrousReady = false;
    [SerializeField] private bool raceStarted;
    private float nitrousTimer;
    [SerializeField] private GameObject rearViewMirrorCamera;
    [SerializeField] private GameObject breakLight1;
    [SerializeField] private GameObject breakLight2;


    [System.Serializable]
    public class AxleInfo
    {
        public WheelCollider leftWheel;
        public WheelCollider rightWheel;
        public bool motor;
        public bool steering;
    }

    public override void Start()
    {
        base.Start();
        if (isAIControllerCar)
        {
            Events.CarSpawnedToTrack?.Invoke(this);
        }
        Events.RaceStarted += HandleRaceStarted;
        
        // if (!isAIControllerCar)
        // {
            // carModel.SetCarLabel(GameManager.GetUsername());
        // }
        if (nitrous1 != null)
        {
            nitrous1.SetActive(false);
        }
        if (nitrous2 != null)
        {
            nitrous2.SetActive(false);
        }
    }

    public override void Update()
    {
        base.Update();
        if (isAIControllerCar || !raceStarted)
        {
            return;
        }

        Quaternion tempRotation = transform.rotation;
        tempRotation.x = 0;
        tempRotation.z = 0;
        transform.rotation = tempRotation;
        if (Input.GetKeyDown(KeyCode.C))
        {
            interiorView.SetActive(!interiorView.activeInHierarchy);
        }
        if (nitrousReady && Input.GetKeyDown(KeyCode.N))
        {
            UseNitrous();
        }
        nitrousUI.SetActive(nitrousReady);

        foreach (MeshRenderer meshRenderer in bodyMeshRenderers)
        {
            meshRenderer.enabled = !interiorView.activeInHierarchy;
        }

        foreach (GameObject bodyGameObject in bodyGameObjects)
        {
            bodyGameObject.SetActive(!interiorView.activeInHierarchy);
        }
        bodyCollider.enabled = !interiorView.activeInHierarchy;
        rearViewMirrorCamera.SetActive(interiorView.activeInHierarchy);
        if (Input.GetKey(KeyCode.UpArrow))
        {
            HandleGasPedal();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            HandleBreakPedal();
        }

        breakLight1.SetActive(Input.GetKey(KeyCode.DownArrow));
        breakLight2.SetActive(Input.GetKey(KeyCode.DownArrow));
        if (!nitrousReady && nitrousTimer >= 30.0f)
        {
            nitrousReady = true;
        }
        else if (!nitrousReady)
        {
            nitrousTimer += Time.deltaTime;
        }
    }

    private void OnDestroy()
    {
        Events.RaceStarted -= HandleRaceStarted;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (crashAudio != null && crashAudio.isPlaying)
        {
            crashAudio.Play();
        }
    }

    private void UseNitrous()
    {
        nitrousReady = false;
        nitrousUI.SetActive(false);
        ToggleNitrous(true);
        IncreaseSpeed();
        Invoke("HideNitrousEffect()", 5.0f);
        nitrousTimer = 0;
    }

    private void HideNitrousEffect()
    {
        DecreaseSpeed();
        ToggleNitrous(false);
    }

    public void DisplayCar(bool shouldShow, bool applyCustomizations = false, bool showHeadlightFlares = false)
    {
        car.SetActive(shouldShow);
        if (applyCustomizations)
        {
            SetCarColorAndRims();
        }

        ToggleHeadlightFlares(showHeadlightFlares);
    }

    public void SetCarColorAndRims()
    {
        carView.SetCarColorAndRims();
    }

    public GameObject GetCar()
    {
        return car;
    }

    public string GetCarName()
    {
        return carModel.GetCarName();
    }

    public float GetCarPrice()
    {
        return carModel.GetCarPrice();
    }

    public void SetCarColor(Color color)
    {
        carView.SetCarColor(color);
    }

    public void SetRimMaterial(Material material)
    {
        carView.SetRimMaterial(material);
    }

    public Material[] GetCarBodyMaterials()
    {
        return carView.GetCarBodyMaterials();
    }

    public Material GetRimMaterial()
    {
        return carView.GetRimMaterial();
    }

    public float GetSpeed()
    {
        return rb.velocity.magnitude;
    }

    public void HandleCheckpointWasHit()
    {
        carModel.CheckpointWasHit();
    }

    public void SetCarLabel(string label)
    {
        carModel.SetCarLabel(label);
    }

    public string GetCarLabel()
    {
        return carModel.GetCarLabel();
    }

    public void SetDistanceToNextCheckpoint(float distance)
    {
        carModel.SetDistanceToNextCheckpoint(distance);
    }
    public void GetDistanceToNextCheckpoint()
    {
        carModel.GetDistanceToNextCheckpoint();
    }

    public List<AxleInfo> AxleInfos;
    public float maxMotorTorque;
    public float maxSteeringAngle;
    public float motor;

    private void FixedUpdate()
    {
        if (isAIControllerCar || !raceStarted)
        {
            return;
        }

        motor = maxMotorTorque * Input.GetAxis("Vertical");
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
        foreach (AxleInfo axleInfo in AxleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }

            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;                
            }
        }
    }
    public void EnableAICar()
    {
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ |
                         RigidbodyConstraints.FreezeRotationY;
        agent.enabled = true;
        pathFollower.enabled = true;
    }

    public GameObject GetRacingCameraObject()
    {
        return racingCameraObject;
    }

    public void ToggleNitrous(bool shouldShow)
    {
        nitrous1.SetActive(shouldShow);
        nitrous2.SetActive(shouldShow);
        if (shouldShow)
        {
            nitrousAudio.Play();
        }
        else
        {
            nitrousAudio.Stop();
        }
    }

    public void IncreaseSpeed()
    {
        maxMotorTorque *= 4;
    }

    public void DecreaseSpeed()
    {
        maxMotorTorque /= 4;
    }
    void HandleRaceStarted()
    {
        raceStarted = true;
    }

    public void ToggleHeadlightFlares(bool shouldShow)
    {
        carHeadlightFlare1.SetActive(shouldShow);
        carHeadlightFlare2.SetActive(shouldShow);
    }

    private void HandleGasPedal()
    {
        if (gasAudio.isPlaying)
        {
            return;
        }
        gasAudio.Play();
    }
    
    private void HandleBreakPedal()
    {
        if (breakAudio.isPlaying)
        {
            return;
        }
        breakAudio.Play();
    }
}