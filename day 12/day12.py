pos = []
pos.append({"x":-16, "y":15, "z":-9})
pos.append({"x":-14, "y":5, "z":4})
pos.append({"x":2, "y":0, "z":6})
pos.append({"x":-3, "y":18, "z":9})

vel = []
vel.append({"x":0, "y":0, "z":0})
vel.append({"x":0, "y":0, "z":0})
vel.append({"x":0, "y":0, "z":0})
vel.append({"x":0, "y":0, "z":0})

axes = [ "x", "y", "z" ]

def new_vel(v, data):
    return len([x for x in data if x > v]) - len([x for x in data if x < v])

def get_vels(axis, pos):
    axis_val = [v[axis] for v in pos]
    new_vels = [new_vel(v, axis_val) for v in axis_val]
    return new_vels

def update_vels(old_vels, new_vels, axis):
    for i in range(len(old_vels)):
        old_vels[i][axis] += new_vels[i]
    return old_vels

for times in range(0, 1000):
    for axis in axes:
        vels = get_vels(axis, pos)
        update_vels(vel, vels, axis)

    for axis in axes:
        for i in range(len(pos)):
            pos[i][axis] += vel[i][axis]

energy = 0

for i in range(len(pos)):
    energy += sum([abs(x) for x in pos[i].values()]) * sum([abs(x) for x in vel[i].values()])
