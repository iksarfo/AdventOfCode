import numpy as np

def _calculate_fuel_required(mass):
    return np.floor(mass / 3) - 2

def _calculate_total_fuel_required(mass):
    total_required = 0

    fuel_required = _calculate_fuel_required(mass)

    while fuel_required > 0:
        total_required += fuel_required
        fuel_required = _calculate_fuel_required(fuel_required)
    
    return total_required

def __read_input():
    input_file = open('input.txt', 'r')
    content = input_file.readlines()
    input_file.close()
    return content

def sum_fuel_required(calculation_function):
    modules = np.array(__read_input())
    calculate = lambda x: calculation_function(int(x))
    calculate_many = np.vectorize(calculate)
    return np.sum(calculate_many(modules))

def part_one_answer():
    return sum_fuel_required(_calculate_fuel_required)

def part_two_answer():
    return sum_fuel_required(_calculate_total_fuel_required)
