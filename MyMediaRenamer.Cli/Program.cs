using System;
using Microsoft.Extensions.CommandLineUtils;

namespace MyMediaRenamer.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new CommandLineApplication();
            app.Name = "MyMediaRenamer";
            app.Description = "hello world!";

            app.HelpOption("-h|--h");

            app.Execute(args);
        }
    }
}
