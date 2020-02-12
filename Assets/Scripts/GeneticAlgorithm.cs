using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Object which facilitates the genetic algorithm and its processes.
/// </summary>
public class GeneticAlgorithm
{
    private int PopulationSize;                                 // Size of the population which is maintained in the genetic algorithm.
    private float MutationRate;                                 // Probability of mutations occuring when child chromosomes are bred.
    private List<Chromosome> Chromosomes;                       // List of chromosomes in the genetic algorithm.
    private float UpwardForceThreshold, ForwardForceThreshold;  // Threshold values for randomising the force properties in chromosomes.

    /// <summary>
    /// Constructor. Initialises the properties of the genetic algorithm object.
    /// </summary>
    /// <param name="populationSize">Size of chromosome population in genetic algorithm.</param>
    /// <param name="mutationRate">Probability of mutations occuring during cross-overs.</param>
    /// <param name="upwardForceThreshold">Threshold for randomising upward force property in chromosomes.</param>
    /// <param name="forwardForceThreshold">Threshold for randomising forward force property in chromosomes.</param>
    public GeneticAlgorithm(int populationSize, float mutationRate, float upwardForceThreshold, float forwardForceThreshold)
    {
        PopulationSize = populationSize;
        MutationRate = mutationRate;
        UpwardForceThreshold = upwardForceThreshold;
        ForwardForceThreshold = forwardForceThreshold;
    }

    /// <summary>
    /// Helper method which initialises the chromosome population in the genetic algorithm.
    /// </summary>
    public void InitialisePopulation()
    {
        Chromosomes = new List<Chromosome>();
        for (int i = 0; i < PopulationSize; i++)
            Chromosomes.Add(new Chromosome(UpwardForceThreshold, ForwardForceThreshold));
    }

    /// <summary>
    /// Utility method used to update the fitness of a chromosome at a specific position.
    /// </summary>
    /// <param name="fitness">New fitness value for a specific chromosome.</param>
    /// <param name="index">index of the chromosome to be updated.</param>
    public void UpdateChromosomeFitness(float fitness, int index)
    {
        if (index < Chromosomes.Count)
            if (Chromosomes[index] != null)
                Chromosomes[index].Fitness = fitness;
    }

    /// <summary>
    /// Primary utility method used for evolving the chromosome population.
    /// </summary>
    /// <param name="avgFitness">Average fitness argument which is referenced in this method.</param>
    public void EvolvePopulation(out float avgFitness)
    {
        List<Chromosome> evolvedPopulation = new List<Chromosome>();
        float totalFitness = GetTotalFitnessFromChromosomes();
        avgFitness = totalFitness / PopulationSize;

        // Repeat process until evolved population same size as initial population
        while (evolvedPopulation.Count != Chromosomes.Count)
        {
            // Selection
            Chromosome chromosomeA, chromosomeB;
            Selection(totalFitness, out chromosomeA, out chromosomeB);

            // Cross over
            Chromosome newChildChromosome = Crossover(chromosomeA, chromosomeB);

            // Mutation
            newChildChromosome.Mutate(MutationRate);

            // Add evovled child chromosome to evolved population
            evolvedPopulation.Add(newChildChromosome);
        }

        // Replace old population with the evolved population
        Chromosomes = evolvedPopulation;
    }

    /// <summary>
    /// Helper method for summating the fitness from all chromosomes in the population.
    /// </summary>
    /// <returns>The summation of fitness values from all chromosomes in the population.</returns>
    private float GetTotalFitnessFromChromosomes()
    {
        float totalFitness = 0;
        foreach (Chromosome aChromosome in Chromosomes)
            totalFitness += aChromosome.Fitness;
        return totalFitness;
    }


    /// <summary>
    /// Utility method used for performing the selection operation on the chromosome population.
    /// This method selects two chromosomes which tend to have high fitness values.
    /// </summary>
    /// <param name="totalFitness">Total fitness parameter used for facilitating the roulette wheel selection approach.</param>
    /// <param name="chromosomeA">The first chromosome returned/referenced as a result of selection.</param>
    /// <param name="chromosomeB">The second chromosome returned/referenced as a result of selection.</param>
    public void Selection(float totalFitness, out Chromosome chromosomeA, out Chromosome chromosomeB)
    {
        RouletteWheelSelectionApproach(totalFitness, out chromosomeA, out chromosomeB);
    }

    /// <summary>
    /// Helper method for selecting chromosomes from the population. This approach is inspired by roulette wheels 
    /// in gambling. The probability of chromosomes being selected is based on their fitness values.
    /// </summary>
    /// <param name="totalFitness">Total fitness parameter used for facilitating the roulette wheel selection approach.</param>
    /// <param name="chromosomeA">The first chromosome returned/referenced as a result of selection.</param>
    /// <param name="chromosomeB">The second chromosome returned/referenced as a result of selection.</param>
    private void RouletteWheelSelectionApproach(float totalFitness, out Chromosome chromosomeA, out Chromosome chromosomeB)
    {
        // Important to normalise the fitness for this selection approach (fitness can't be negative)
        chromosomeA = RouletteWheelSelectionSpin(Random.Range(0, totalFitness));
        chromosomeB = RouletteWheelSelectionSpin(Random.Range(0, totalFitness));
    }

    /// <summary>
    /// Helper method used by the roulette wheel selection approach. This method is used to
    /// select a chromosome from the population.
    /// </summary>
    /// <param name="fitnessThreshold">Threshold which is used for selecting a chromosome from the population.</param>
    /// <returns></returns>
    private Chromosome RouletteWheelSelectionSpin(float fitnessThreshold)
    {
        float currentTotalFitness = 0;
        foreach (Chromosome aChromosome in Chromosomes)
        {
            currentTotalFitness += aChromosome.Fitness;
            if (currentTotalFitness >= fitnessThreshold)
                return aChromosome;
        }

        return null;
    }

    /// <summary>
    /// Utility method which breeds chromosomes in the population and returns the newly-bred
    /// child chromosome, which inherits properties from its parent chromosomes.
    /// </summary>
    /// <param name="chromosomeA">The first parent chromosome used for breeding.</param>
    /// <param name="chromosomeB">The second parent chromosome used for breeding.</param>
    /// <returns>A newly-bred child chromosome which inherits properties from its parent chromosomes.</returns>
    public Chromosome Crossover(Chromosome chromosomeA, Chromosome chromosomeB)
    {
        return AllRandomCrossOver(chromosomeA, chromosomeB);
    }

    /// <summary>
    /// Helper method which facilitates a complete-random cross-over approach between
    /// parent chromosomes. Each property of the child chromosome may be inherited from
    /// either parent chromosomes.
    /// </summary>
    /// <param name="chromosomeA">The first parent chromosome used for breeding.</param>
    /// <param name="chromosomeB">The second parent chromosome used for breeding.</param>
    /// <returns>A newly-bred child chromosome which inherits properties from its parent chromosomes.</returns>
    private Chromosome AllRandomCrossOver(Chromosome chromosomeA, Chromosome chromosomeB)
    {
        // If <= 0.5, choose chromosomeA's data, otherwise choose chromosomeB's data
        return new Chromosome(UpwardForceThreshold, ForwardForceThreshold)
        {
            UpwardForce = Random.Range(0f, 1f) <= 0.5f ? chromosomeA.UpwardForce : chromosomeB.UpwardForce,
            ForwardForce = Random.Range(0f, 1f) <= 0.5f ? chromosomeA.ForwardForce : chromosomeB.ForwardForce
        };
    }

    /// <summary>
    /// Utility/helper method used to acquire a chromosome from the population at a specific position.
    /// </summary>
    /// <param name="index">Position used to acquire a specific chromosome from the population.</param>
    /// <returns>Chromosome at a specified position in the population.</returns>
    public Chromosome GetChromosomeAtIndex(int index)
    {
        if (Chromosomes != null)
            if (index < Chromosomes.Count)
                return Chromosomes[index];
        return null;
    }
}
