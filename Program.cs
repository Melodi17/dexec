using System.Diagnostics;
using System.Net;
using CommandLine;

namespace dexec
{
    public class Options
    {
        [Value(0, MetaName = "file", HelpText = "File/url to download", Required = true)]
        public string File { get; set; }

        //[Option('p', "persist", HelpText = "Whether the file should persist after execution", Default = false)]
        //public bool Persist { get; set; }

        //[Option('e', "elevated", HelpText = "Whether the file should be elevated during execution", Default = false)]
        //public bool Elevated { get; set; }
    }

    internal class Program
    {
        static Options opt = null;
        static string localFileName;
        static void Main(string[] args)
        {
            string[] safeArgs = Box.IOUtils.ConnectStrings(string.Join(' ', args), false);
            Parser.Default.ParseArguments<Options>(safeArgs).WithParsed(o => opt = o);
            
            if (opt == null)
                return;
            
            WebClient client = new();
            localFileName = Path.GetFileName(opt.File);
            client.DownloadFile(opt.File, localFileName);
            Process p = new()
            {
                StartInfo = new()
                {
                    WorkingDirectory = Environment.CurrentDirectory,
                    FileName = "explorer",
                    Arguments = $" \"{localFileName}\"",
                    UseShellExecute = true,
                }
            };
            p.Start();
        }
    }
}