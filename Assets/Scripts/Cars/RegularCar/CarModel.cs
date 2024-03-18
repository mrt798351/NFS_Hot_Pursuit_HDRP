using UnityEngine;

public class CarModel : MonoBehaviour
{
    [SerializeField] private string carName = null;
    [SerializeField] private float carPrice = 0f;
    private string label;
    private int numberOfCheckpointsHit;
    private float distanceToNextCheckpoint;

    public string GetCarName()
    {
        return carName;
    }

    public string GetCarLabel()
    {
        return label;
    }

    public void SetCarLabel(string label)
    {
        this.label = label;
    }

    public int GetNumberOfCheckpointsHit()
    {
        return numberOfCheckpointsHit;
    }

    public float GetCarPrice()
    {
        return carPrice;
    }

    public void CheckpointWasHit()
    {
        numberOfCheckpointsHit++;
    }

    public float GetDistanceToNextCheckpoint()
    {
        return distanceToNextCheckpoint;
    }

    public void SetDistanceToNextCheckpoint(float distance)
    {
        distanceToNextCheckpoint = distance;
    }
}