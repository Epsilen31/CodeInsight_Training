using Codeinsight.VehicleInformer.Contracts;

namespace Codeinsight.VehicleInformer.Services
{
    public class FileProcessor : IFileProcessor
    {
        public string ReadFiles(string filepath)
        {
            if (!File.Exists(filepath))
            {
                return "File does not exist";
            }
            return string.Join("\n", File.ReadAllLines(filepath));
        }

        public void GenerateFile(string directoryPath , string  content)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            File.WriteAllText(directoryPath, content);
        }      
    }
}

