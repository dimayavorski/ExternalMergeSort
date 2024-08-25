using Bogus;

namespace FileGenerator
{
    internal class FileGenerator
    {
        private const string _fileName = "input.txt";

        private Faker _faker = new Faker();
        private readonly int _maxNumber;
        private readonly int _maxUniqueWords;

        public FileGenerator(int maxNumber, int maxUniqueWords)
        {
            _maxNumber = maxNumber;
            _maxUniqueWords = maxUniqueWords;
        }

        /// <summary>
        /// File size in GB
        /// </summary>
        /// <param name="fileSize"></param>
        /// <returns></returns>
        public async Task GenerateFile(int fileSize)
        {
            var fileSizeInBytes = GetFileSizeInBytes(fileSize);
            RemoveFileIfExists(_fileName);
            var randomWords = GenerateWords(_maxUniqueWords);

            using (var writer = new StreamWriter(_fileName))
            {   
                long currentFileSize = 0;
                while (currentFileSize < fileSizeInBytes)
                {
                    var row = GetRandomNumber(_maxNumber) + ". " + randomWords[_faker.Random.Number(0, randomWords.Length - 1)];
                    await writer.WriteLineAsync(row);
                    currentFileSize += System.Text.Encoding.UTF8.GetBytes(row).Length + Environment.NewLine.Length;
                }
            }
        }

        private long GetFileSizeInBytes(int fileSize)
        {
            return (long)fileSize * 1024 * 1024 * 1024;
        }

        private string[] GenerateWords(int numberOfWords)
        {
            return _faker.Random.WordsArray(numberOfWords);
        }

        private int GetRandomNumber(int maxNumber)
        {
            return _faker.Random.Int(1, maxNumber);
        }

        private void RemoveFileIfExists(string _fileName)
        {
            if(File.Exists(_fileName)) 
            {
                File.Delete(_fileName);
            }
        }
    }
}
