Param(
  [string]$duration
)

.\ParallelSteering.exe -t $duration -o out1.txt -numThreads 1
.\ParallelSteering.exe -t $duration -o out2.txt -numThreads 2
.\ParallelSteering.exe -t $duration -o out4.txt -numThreads 4
.\ParallelSteering.exe -t $duration -o out8.txt -numThreads 8
.\ParallelSteering.exe -t $duration -o out16.txt -numThreads 16
.\ParallelSteering.exe -t $duration -o out32.txt -numThreads 32

python .\Plotter.py -i out1.txt out2.txt out4.txt out8.txt out16.txt out32.txt -axes 1 2 4 8 16 32