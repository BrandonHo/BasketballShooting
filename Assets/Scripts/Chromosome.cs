using UnityEngine;

public class Chromosome
{
    public float UpwardForce, ForwardForce, Fitness;

    private float UpwardForceThreshold, ForwardForceThreshold;

    public Chromosome(float upwardForceThreshold, float forwardForceThreshold)
    {
        UpwardForceThreshold = upwardForceThreshold;
        ForwardForceThreshold = forwardForceThreshold;

        UpwardForce = Random.Range(0f, UpwardForceThreshold);
        ForwardForce = Random.Range(0f, ForwardForceThreshold);
    }

    public void Mutate(float mutationRate)
    {
        UpwardForce = Random.Range(0f, 1f) <= mutationRate ? Random.Range(0f, UpwardForceThreshold) : UpwardForce;
        ForwardForce = Random.Range(0f, 1f) <= mutationRate ? Random.Range(0f, ForwardForceThreshold) : ForwardForce;
    }
}
