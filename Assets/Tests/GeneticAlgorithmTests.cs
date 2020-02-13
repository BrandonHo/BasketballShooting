using NUnit.Framework;

public class GeneticAlgorithmTests
{
    [Test]
    public void InitialisePopulationTest()
    {
        GeneticAlgorithm ga = new GeneticAlgorithm(50, 0.01f, 60f, 60f);
        ga.InitialisePopulation();
        Assert.AreEqual(ga.GetChromosomes().Count, 50);
    }

    [Test]
    public void UpdateFitnessTest()
    {
        GeneticAlgorithm ga = new GeneticAlgorithm(50, 0.01f, 60f, 60f);
        ga.InitialisePopulation();
        ga.UpdateChromosomeFitness(5f, 0);
        Assert.AreEqual(ga.GetChromosomeAtIndex(0).Fitness, 5f);
    }

    [Test]
    public void SelectionTest()
    {
        GeneticAlgorithm ga = new GeneticAlgorithm(50, 0.01f, 60f, 60f);
        ga.InitialisePopulation();
        ga.UpdateChromosomeFitness(5f, 15);

        Chromosome chromosomeA, chromosomeB;
        ga.Selection(out chromosomeA, out chromosomeB);
        Assert.IsTrue(((chromosomeA.Fitness == 5f) && (chromosomeB.Fitness == 5f)));
    }

    [Test]
    public void CrossoverTest()
    {
        GeneticAlgorithm ga = new GeneticAlgorithm(50, 0.01f, 60f, 60f);
        ga.InitialisePopulation();
        ga.UpdateChromosomeFitness(5f, 15);

        Chromosome chromosomeA, chromosomeB, childChromosome;
        ga.Selection(out chromosomeA, out chromosomeB);
        childChromosome = ga.Crossover(chromosomeA, chromosomeB);

        // Chromosome A should be equal to Chromosome B, so interchangable here.
        Assert.IsTrue(((childChromosome.UpwardForce == chromosomeA.UpwardForce) && (childChromosome.ForwardForce == chromosomeB.ForwardForce)));
    }

    [Test]
    public void MutationTest()
    {
        GeneticAlgorithm ga = new GeneticAlgorithm(50, 1f, 60f, 60f);
        ga.InitialisePopulation();
        ga.UpdateChromosomeFitness(5f, 15);

        Chromosome chromosomeA, chromosomeB, childChromosome;
        ga.Selection(out chromosomeA, out chromosomeB);
        childChromosome = ga.Crossover(chromosomeA, chromosomeB);

        float prevUpwardForce = childChromosome.UpwardForce;
        float prevForwardForce = childChromosome.ForwardForce;
        childChromosome.Mutate(1f);

        // Possible that both forces being randomised to same value after mutation, but highly unlikely
        Assert.IsTrue(((childChromosome.UpwardForce != prevUpwardForce) || (childChromosome.ForwardForce != prevForwardForce)));
    }
}
