using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;

namespace grab
{
    class Options
    {
        [Option('c',"collection",Required = true)]
        public string collection { get; set; }

        [Option('a',"apikey", Required = true)]
        public string apiKey { get; set; }

        [Option('p',"path", Required = true)]
        public string outputPath { get; set; }
    }
}
