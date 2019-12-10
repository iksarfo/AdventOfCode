input_file = open('input.txt', 'r')
content = input_file.read()

width = 25
height = 6
layer_length = width * height

layers = {}

layer_count = int(len(content) / layer_length)

def findDigits(layer,digit):
    return [i for i, found in enumerate(layer) if found == digit]

zeros = {}
fewest = {}

for i in range(0, layer_count):
    read_start = i * layer_length
    read_end = (i + 1) * layer_length
    layer = content[read_start:read_end]
    layers[i] = layer
    found = findDigits(layer, '0')
    zeros[i] = found
    fewest[len(found)] = i

fewest_zeros = min([len(zero) for zero in zeros.values()])

layer_fewest = layers[fewest[fewest_zeros]]

part_one = len(findDigits(layer_fewest, '1')) * len(findDigits(layer_fewest, '2'))



