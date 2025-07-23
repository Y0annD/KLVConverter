using Microsoft.Extensions.Logging;

internal class Program
{
    /// <summary>
    /// Logger.
    /// </summary>
    private ILogger Logger;
    /// <summary>
    /// List of not processed files.
    /// </summary>
    private List<string> NotProcessedFiles = [];
    /// <summary>
    /// List of processed files.
    /// </summary>
    private List<string> ProcessedFiles = [];

    public Program()
    {
        using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
        Logger = factory.CreateLogger("KLVConverter");
    }

    /// <summary>
    /// Convert list of KLV Files.
    /// </summary>
    /// <param name="args">array of klv files to convert</param>
    public void Process(string[] args)
    {
        Logger.LogInformation("Ask to convert KLV data with args: {Description}.", args);
        if (args.Length > 0)
        {
            Logger.LogInformation("{nbFiles} KLV data files to convert", args.Length);
            foreach (string datafile in args)
            {
                // check if file exist and open it
                if (File.Exists(datafile))
                {
                    Logger.LogInformation("Process {file} datafile", datafile);
                    using (var fs = new FileStream(@datafile, FileMode.Open))
                    {
                        Logger.LogInformation("{datafile}: {length} bytes", datafile, fs.Length);
                    }
                    ProcessedFiles.Add(datafile);

                }
                else
                {
                    Logger.LogWarning("File {datafile} does not exist", datafile);
                    NotProcessedFiles.Add(datafile);
                }
            }
            PrintReport();
        }
        else
        {
            Logger.LogError("No source file to convert");
        }
    }

    /// <summary>
    /// Print report to user.
    /// </summary>
    private void PrintReport()
    {
        if (NotProcessedFiles.Count > 0)
        {
            Logger.LogInformation("{count} files were not processed", NotProcessedFiles.Count);
            foreach (string file in NotProcessedFiles)
            {
                Logger.LogInformation(" - {file}", file);
            }
            Logger.LogInformation(" ");
        }
        if (ProcessedFiles.Count > 0)
        {
            Logger.LogInformation("{count} files were processed", ProcessedFiles.Count);
            foreach (string file in ProcessedFiles)
            {
                Logger.LogInformation(" - {file}", file);
            }
        }

    }

    private static void Main(string[] args)
    {
        Program prog = new Program();
        prog.Process(args);

    }
}