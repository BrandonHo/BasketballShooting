# Basketball Shooting

This Unity project demonstrates the usage of a genetic algorithm to calculate the required forces to successfully shoot a basketball into the basketball hoop from any position in the basketball court.

# Details

Genetic algorithms involve generating a population of random solutions, and evolving these solutions according to a specific goal. These solutions are represented as chromosomes. In this scenario, chromosomes represent solutions of basketball shots. Chromosomes include two parameters, namely an upward force and a forward force.

The fitness of chromosomes are evaluated by processing a basketball shot with its two force parameters after a specified duration. Fitness is a term used to describe the strength of the solution. The fitness of chromosomes are evaluated based on two factors, namely:

1. The closest distance between the basketball and the basket during the evaluation; and
2. If the basketball had successfully been scored in the basketball hoop.

The evolution of a chromosome population involves the following three phases:
1. Selection - selecting chromosomes with mostly high fitness measurements (roulette method)
2. Crossover - breeding new chromosomes that were selected during the selection phase
3. Mutation - adding a small probability of changing parameters of chromosome offsprings that were bred during the crossover phase

By continuously evolving a population of chromosomes, the solutions slowly converge towards the specified goal. Therefore, the basketball shots slowly converge towards successfully being shot into the basketball hoop.

# Demonstration

First Generation

![First Generation Demo](demo/demo1.gif)

100% Score After 17th Generation

![100% Score Generation Demo](demo/demo2.gif)

# References

[Computational Intelligence: An Introduction by Andries P. Engelbrecht](https://www.amazon.com/Computational-Intelligence-Introduction-Andries-Engelbrecht/dp/0470035617)

[The Nature of Code by Daniel Shiffman](https://natureofcode.com/book/)
