using UnityEngine;

/// <summary>
/// Representation of a solution in the basketball shooting genetic algorithm.
/// </summary>
public class Chromosome
{
    public float UpwardForce, ForwardForce;                     // Forces to be applied onto a ball game object as per this solution.
    public float Fitness;                                       // Strength of this chromosome as a solution.
    private float UpwardForceThreshold, ForwardForceThreshold;  // Thresholds used for randomising the force properties.

    /// <summary>
    /// Constructor which involves initialisation of the chromosome and its properties.
    /// </summary>
    /// <param name="upwardForceThreshold">Threshold for randomising the upward force property.</param>
    /// <param name="forwardForceThreshold">Threshold for randomising the forward force property.</param>
    public Chromosome(float upwardForceThreshold, float forwardForceThreshold)
    {
        UpwardForceThreshold = upwardForceThreshold;
        ForwardForceThreshold = forwardForceThreshold;
        UpwardForce = Random.Range(0f, UpwardForceThreshold);
        ForwardForce = Random.Range(0f, ForwardForceThreshold);
    }

    /// <summary>
    /// Utility method for mutating the properties of this chromosome.
    /// </summary>
    /// <param name="mutationRate">Probability of mutating a property of this chromosome.</param>
    public void Mutate(float mutationRate)
    {
        UpwardForce = Random.Range(0f, 1f) <= mutationRate ? Random.Range(0f, UpwardForceThreshold) : UpwardForce;
        ForwardForce = Random.Range(0f, 1f) <= mutationRate ? Random.Range(0f, ForwardForceThreshold) : ForwardForce;
    }
}
