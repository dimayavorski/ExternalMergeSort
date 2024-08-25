namespace FileGenerator
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var filePath = "input.txt";
            var fileGenerator = new FileGenerator(maxNumber: 10000000, maxUniqueWords: 5000);
            await fileGenerator.GenerateFile(filePath, 1);
        }
    }
}