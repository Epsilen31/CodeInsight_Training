using Codeinsight.VehicalInformer.Contracts;

namespace Codeinsight.VehicalInformer.Services
{
    public class FileProcessor : IFileProcessor
    {
        public string ReadCarDetails(string filepath)
        {
            if (!File.Exists(filepath))
            {
                return "File does not exist";
            }
            return string.Join("\n", File.ReadAllLines(filepath));
        }
    }
}

