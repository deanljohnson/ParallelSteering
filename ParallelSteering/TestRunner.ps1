.\ParallelSteering.exe -t 1 -o out1.txt -numThreads 1
.\ParallelSteering.exe -t 1 -o out2.txt -numThreads 2
.\ParallelSteering.exe -t 1 -o out4.txt -numThreads 4
.\ParallelSteering.exe -t 1 -o out8.txt -numThreads 8
.\ParallelSteering.exe -t 1 -o out16.txt -numThreads 16

python .\Plotter.py out1.txt out2.txt out4.txt out8.txt out16.txt