import unittest

from day01 import _calculate_fuel_required, _calculate_total_fuel_required, part_one_answer, part_two_answer

class TestCalculateFuelRequirement(unittest.TestCase):
    def test_part_one(self):
        self.assertEqual(2, _calculate_fuel_required(12))
        self.assertEqual(2, _calculate_fuel_required(14))
        self.assertEqual(654, _calculate_fuel_required(1969))
        self.assertEqual(33583, _calculate_fuel_required(100756))
        self.assertEqual(33583, _calculate_fuel_required(100756))
        self.assertEqual(3269199, part_one_answer())

    def test_part_two(self):
        self.assertEqual(2, _calculate_total_fuel_required(12))
        self.assertEqual(2, _calculate_total_fuel_required(14))
        self.assertEqual(966, _calculate_total_fuel_required(1969))
        self.assertEqual(50346, _calculate_total_fuel_required(100756))
        self.assertEqual(4900909, part_two_answer())