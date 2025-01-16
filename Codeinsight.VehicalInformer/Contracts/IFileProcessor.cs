using Codeinsight.VehicalInformer.DTOs;

namespace Codeinsight.VehicalInformer.Contracts
{
    public interface IFileProcessor
    {
        string ReadFiles(string filepath);

        void GenerateFile(string filepath, string content);
    }
}

