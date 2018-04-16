import sys
import matplotlib as mpl 

## agg backend is used to create plot as a .png file
mpl.use('agg')

import matplotlib.pyplot as plt 

dataFiles = []
axes = []

def parseCommandLineArguments():
	i = 0
	while i < len(sys.argv) - 1:
		i = i + 1
		if sys.argv[i] == "-i":
			axisCount = 0
			for j in range(i + 1, len(sys.argv)):
				i = j
				if sys.argv[j] == "-axes":
					break
				dataFiles.append(sys.argv[j])
				axes.append(axisCount + 1)
				axisCount = axisCount + 1
		if sys.argv[i] == "-axes":
			axisCount = 0
			for j in range(i + 1, len(sys.argv)):
				axes[axisCount] = sys.argv[j]
				axisCount = axisCount + 1

def readInDataFile(file):
	with open(file, 'r') as f:
		for line in f.readlines():
			strs = line.strip().split(',')
			return list(map(float, strs))

def readInData():
	data = []
	for file in dataFiles:
		data.append(readInDataFile(file))
	return data

parseCommandLineArguments()

data_to_plot = readInData()

# Create a figure instance
fig = plt.figure(1, figsize=(9, 6))

# Create an axes instance
ax = fig.add_subplot(111)

# Create the boxplot
bp = ax.boxplot(data_to_plot)

ax.set_xticklabels(axes)

# Save the figure
fig.savefig('fig1.png', bbox_inches='tight')