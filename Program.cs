// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codeinsight.FileManager.Services;
using Codeinsight.FileManager.Contracts;


namespace Codeinsight.FileManager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IFileServices fileServices = (IFileServices)new FileServices();
            fileServices.FileImplimentatiionTask();
        }
    }
    
}