using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codeinsight.FileManager.Contracts
{
    internal interface IFileProcessor
    {

        string ReadFile(string path);

        void WriteFile(string path, string content);

        void CopyFile(string sourcePath, string destinationPath);

        void DeleteFile(string path);

        void MoveFile(string sourcePath, string destinationPath);
    }
}
