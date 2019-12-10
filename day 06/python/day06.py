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

def _get_objects_orbits(o, maps):
    orbits = {}

    while o in maps:
        orbits[maps[o]] = o
        o = maps[o]

    return orbits

def _count_all_orbits(maps):
    count = 0

    for o in maps.keys():
        count += len(_get_objects_orbits(o,maps))
    
    return count

# Part one
_count_all_orbits(_get_orbits())

def _get_distance(to,orbits):
    return list(orbits.keys()).index(to)

def _orbital_transfers(fro,to,orbits):
    mine =  _get_objects_orbits(fro, orbits)
    santas = _get_objects_orbits(to, orbits)

    distances = {}
    for o in santas.keys():
        if o in mine:
            distances[o] = _get_distance(o, mine) + _get_distance(o, santas)

    return distances.values()

# Part two
min(_orbital_transfers('YOU', 'SAN', _get_orbits()))
