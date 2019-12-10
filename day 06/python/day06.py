def __read_input():
    input_file = open('input.txt', 'r')
    content = input_file.read()
    input_file.close()
    return content.split("\n")

def _get_orbits():
    orbits = {}
    orbit_definitions = __read_input()

    for definition in orbit_definitions:
        orbit = definition.split(")")

        if len(orbit) > 1:
            bigger = orbit[0]
            smaller = orbit[1]
            orbits[smaller] = bigger

    return orbits

def _count_objects_orbits(o, maps):
    count = 0

    while o in maps:
        count += 1
        o = maps[o]

    return count

def _count_all_orbits(maps):
    count = 0

    for o in maps.keys():
        count += _count_objects_orbits(o,maps)
    
    return count

_count_all_orbits(_get_orbits())
