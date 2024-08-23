namespace FileGenerator
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //100_000_000 1.5GB
            var fileGenerator = new FileGenerator(maxNumber: 100000, maxUniqueWords: 300);
            await fileGenerator.GenerateFile(300_000);
        }
    }
}