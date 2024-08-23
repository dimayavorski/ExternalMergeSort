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
        public async Task GenerateFile(int numberOfRows)
        {
            RemoveFileIfExists(_fileName);

            var randomWords = GenerateWords(_maxUniqueWords);
            using (var writer = new StreamWriter(_fileName))
            {
                for (int i = 0; i < numberOfRows; i++)
                {
                    await writer.WriteLineAsync(GetRandomNumber(_maxNumber) + ". " + randomWords[_faker.Random.Number(0, randomWords.Length - 1)]);
                }
            }
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
