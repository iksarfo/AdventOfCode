using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AOC._2019.Day1
{
    public static class Solution
    {
        public static int GetFuelRequirements(int moduleMass, bool includeFuelMass = false)
        {
            var fuelRequired = CalculateFuelRequired(moduleMass);

            return includeFuelMass && fuelRequired >= 3
                ? fuelRequired + GetFuelRequirements(fuelRequired, true)
                : fuelRequired;
        }

        private static int CalculateFuelRequired(int moduleMass)
        {
            var calculation = moduleMass / 3 - 2;

            return calculation > 0
                ? calculation
                : 0;
        }
    }

    public class Day1Tests
    {
        [Test]
        public void Mass_12() => Assert.AreEqual(2, Solution.GetFuelRequirements(12));

        [Test]
        public void Mass_14_Part_One() => Assert.AreEqual(2, Solution.GetFuelRequirements(14));

        [Test]
        public void Mass_14_Part_Two() => Assert.AreEqual(2, Solution.GetFuelRequirements(14, true));

        [Test]
        public void Mass_1969_Part_One() => Assert.AreEqual(654, Solution.GetFuelRequirements(1969));

        [Test]
        public void Mass_1969_Part_Two() => Assert.AreEqual(966, Solution.GetFuelRequirements(1969, true));

        [Test]
        public void Mass_100756() => Assert.AreEqual(33583, Solution.GetFuelRequirements(100756));

        [Test]
        public void Mass_100756_Part_One() => Assert.AreEqual(33583, Solution.GetFuelRequirements(100756));

        [Test]
        public void Mass_100756_Part_Two() => Assert.AreEqual(50346, Solution.GetFuelRequirements(100756, true));

        [Test]
        public void PartOne()
        {
            var sumOfModuleMasses =
                File.ReadAllLines(@"C:\Temp\input.txt")
                    .Select(int.Parse)
                    .Sum(moduleMass => Solution.GetFuelRequirements(moduleMass));

            Assert.AreEqual(3269199, sumOfModuleMasses);
        }

        [Test]
        public void PartTwo()
        {
            var sumOfModuleMasses =
                File.ReadAllLines(@"C:\Temp\input.txt")
                    .Select(int.Parse)
                    .Sum(_ => Solution.GetFuelRequirements(_, true));

            Assert.AreEqual(4900909, sumOfModuleMasses);
        }
    }
}