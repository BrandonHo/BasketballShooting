using UnityEngine;
using System.Collections.Generic;

public class GeneticAlgorithm
{
    private int PopulationSize;
    private float MutationRate;
    private List<Chromosome> Chromosomes;

    private float UpwardForceThreshold, ForwardForceThreshold;

    public GeneticAlgorithm(int populationSize, float mutationRate, float upwardForceThreshold, float forwardForceThreshold)
    {
        PopulationSize = populationSize;
        MutationRate = mutationRate;

        UpwardForceThreshold = upwardForceThreshold;
        ForwardForceThreshold = forwardForceThreshold;
    }

    public void InitialisePopulation()
    {
        Chromosomes = new List<Chromosome>();
        for (int i = 0; i < PopulationSize; i++)
            Chromosomes.Add(new Chromosome(UpwardForceThreshold, ForwardForceThreshold));
    }

    public void UpdateChromosomeFitness(float fitness, int position)
    {
        if (position < Chromosomes.Count)
            if (Chromosomes[position] != null)
                Chromosomes[position].Fitness = fitness;
    }

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

    public void Selection(float totalFitness, out Chromosome chromosomeA, out Chromosome chromosomeB)
    {
        RouletteWheelSelectionApproach(totalFitness, out chromosomeA, out chromosomeB);
    }

    private void RouletteWheelSelectionApproach(float totalFitness, out Chromosome chromosomeA, out Chromosome chromosomeB)
    {
        // Important to normalise the fitness for this selection approach
        chromosomeA = RouletteWheelSelectionSpin(Random.Range(0, totalFitness));
        chromosomeB = RouletteWheelSelectionSpin(Random.Range(0, totalFitness));
    }

    private float GetTotalFitnessFromChromosomes()
    {
        float totalFitness = 0;
        foreach (Chromosome aChromosome in Chromosomes)
            totalFitness += aChromosome.Fitness;
        return totalFitness;
    }

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

    public Chromosome Crossover(Chromosome chromosomeA, Chromosome chromosomeB)
    {
        return AllRandomCrossOver(chromosomeA, chromosomeB);
    }

    private Chromosome AllRandomCrossOver(Chromosome chromosomeA, Chromosome chromosomeB)
    {
        // If <= 0.5, choose chromosomeA's data, otherwise choose chromosomeB's data
        return new Chromosome(UpwardForceThreshold, ForwardForceThreshold)
        {
            UpwardForce = Random.Range(0f, 1f) <= 0.5f ? chromosomeA.UpwardForce : chromosomeB.UpwardForce,
            ForwardForce = Random.Range(0f, 1f) <= 0.5f ? chromosomeA.ForwardForce : chromosomeB.ForwardForce
        };
    }

    public Chromosome GetChromosomeAtPosition(int position)
    {
        if (Chromosomes != null)
            if (position < Chromosomes.Count)
                return Chromosomes[position];
        return null;
    }
}
