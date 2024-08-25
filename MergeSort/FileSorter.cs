using NickStrupat;
using System.Diagnostics;
using System.Text;

namespace MergeSort
{
    internal class FileSorter
    {
        private const string _outputFilePath = "output.txt";
        private const int bytesOfRamToProcessOneByteOfFile = 3;

        public void SortFile(string filePath)
        {
            var maximumChunSize = GetChunkSize();
            var temporaryFiles = SplitFile(filePath, maximumChunSize);
            GenerateOutput(temporaryFiles);
            DeleteTemporaryFiles(temporaryFiles);
        }

        private ulong GetChunkSize()
        {
            ComputerInfo ci = new ComputerInfo();
            var availableBytes = ci.AvailablePhysicalMemory;
            var canHandleBytesPerIteration = availableBytes / bytesOfRamToProcessOneByteOfFile;
            return canHandleBytesPerIteration;
        }

        private string[] SplitFile(string filePath, ulong maximumChunkSize)
        {
            var temporaryFiles = new List<string>();
            int fileCounter = 1;
            SortedDictionary<string, List<int>> storage = new SortedDictionary<string, List<int>>();

            using (StreamReader reader = new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {
                    ulong chunkSize = 0;
                    string fileName = fileCounter + ".txt";
                    while (chunkSize < maximumChunkSize && !reader.EndOfStream)
                    {
                        var row = new Row(reader.ReadLine()!);
                        var key = row.GetWord().ToString();
                        if (storage.TryGetValue(key, out var numbers))
                        {
                            numbers.Add(row.Number);
                        }
                        else
                        {
                            storage.Add(key, new List<int> { row.Number });
                        }

                        chunkSize += (ulong)Encoding.UTF8.GetBytes(row.ToString()).Length;
                    }

                    CreateTemporaryFile(fileName, storage);
                    temporaryFiles.Add(fileName);

                    storage.Clear();
                    fileCounter++;
                }
            }

            return temporaryFiles.ToArray();
        }

        private void GenerateOutput(string[] files)
        {
            var rows = files.Select(x =>
            {
                var reader = new StreamReader(x);
                return new Row(reader.ReadLine()!, reader);
            }).ToList();

            try
            {
                var sortedList = new List<Row>();
                using (var writer = new StreamWriter(_outputFilePath))
                {
                    while (rows.Any())
                    {
                        rows.Sort();
                        var currentRow = rows[0];
                        writer.WriteLine(currentRow.ToString());

                        if (currentRow.Reader.EndOfStream)
                        {
                            currentRow.Reader.Dispose();
                            rows.Remove(currentRow);
                        }
                        else
                        {
                            rows[0] = new Row(currentRow.Reader.ReadLine()!, currentRow.Reader);
                        }
                    }
                }
            }
            catch (Exception)
            {
                rows.ForEach(row =>
                {
                    row.Reader.Dispose();
                });
            }
        }

        private void CreateTemporaryFile(string fileName, SortedDictionary<string, List<int>> dict)
        {
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                foreach (var row in dict)
                {
                    row.Value.Sort();
                    row.Value.ForEach(val => writer.WriteLine(val + ". " + row.Key));
                }
            }
        }

        private void DeleteTemporaryFiles(string[] files)
        {
            foreach (var file in files)
            {
                File.Delete(file);
            }
        }
    }
}
