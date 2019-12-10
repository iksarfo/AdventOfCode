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

part_two = []

for y in range(0, height):
    for x in range(0, width):
        z = 0
        i = y * width + x

        while True:
            pixel = layers[z][i]
            z += 1
            if pixel != '2':
                break

        part_two.append(pixel)

for y in range(0, height):
    read_start = y * width
    read_end = read_start + width
    line = ''.join(part_two[read_start:read_end])
    print(line.replace('0', " ").replace('1', '*'))


