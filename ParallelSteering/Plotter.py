import sys
import matplotlib as mpl 

## agg backend is used to create plot as a .png file
mpl.use('agg')

import matplotlib.pyplot as plt 

def readInDataFile(file):
	with open(file, 'r') as f:
		for line in f.readlines():
			strs = line.strip().split(',')
			return list(map(float, strs))

def readInData():
	data = []
	for i in range(1, len(sys.argv)):
		data.append(readInDataFile(sys.argv[i]))
	return data


data_to_plot = readInData()

# Create a figure instance
fig = plt.figure(1, figsize=(9, 6))

# Create an axes instance
ax = fig.add_subplot(111)

# Create the boxplot
bp = ax.boxplot(data_to_plot)

# Save the figure
fig.savefig('fig1.png', bbox_inches='tight')