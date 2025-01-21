namespace Codeinsight.VehicleInformer.Contracts
{
    public interface IFileProcessor
    {
        string ReadFiles(string filepath);
        
        void GenerateFile(string filepath, string content);
    }
}
