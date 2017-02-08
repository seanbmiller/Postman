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
        [Option('c',"collection")]
        public string collection { get; set; }

        [Option('a',"apikey", Required = true)]
        public string apiKey { get; set; }

        [Option('p',"path", Required = true)]
        public string outputPath { get; set; }

        [Option('e',"environment")]
        public string envName { get; set; }
    }
}
