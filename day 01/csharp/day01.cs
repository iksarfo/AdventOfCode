using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AdventOfCode2019
{
    public static class Day01
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
        public void Mass_12() => Assert.AreEqual(2, Day01.GetFuelRequirements(12));

        [Test]
        public void Mass_14_Part_One() => Assert.AreEqual(2, Day01.GetFuelRequirements(14));

        [Test]
        public void Mass_14_Part_Two() => Assert.AreEqual(2, Day01.GetFuelRequirements(14, true));

        [Test]
        public void Mass_1969_Part_One() => Assert.AreEqual(654, Day01.GetFuelRequirements(1969));

        [Test]
        public void Mass_1969_Part_Two() => Assert.AreEqual(966, Day01.GetFuelRequirements(1969, true));

        [Test]
        public void Mass_100756() => Assert.AreEqual(33583, Day01.GetFuelRequirements(100756));

        [Test]
        public void Mass_100756_Part_One() => Assert.AreEqual(33583, Day01.GetFuelRequirements(100756));

        [Test]
        public void Mass_100756_Part_Two() => Assert.AreEqual(50346, Day01.GetFuelRequirements(100756, true));

        [Test]
        public void PartOne()
        {
            var sumOfModuleMasses =
                File.ReadAllLines(@"C:\Temp\input.txt")
                    .Select(int.Parse)
                    .Sum(moduleMass => Day01.GetFuelRequirements(moduleMass));

            Assert.AreEqual(3269199, sumOfModuleMasses);
        }

        [Test]
        public void PartTwo()
        {
            var sumOfModuleMasses =
                File.ReadAllLines(@"C:\Temp\input.txt")
                    .Select(int.Parse)
                    .Sum(_ => Day01.GetFuelRequirements(_, true));

            Assert.AreEqual(4900909, sumOfModuleMasses);
        }
    }
}