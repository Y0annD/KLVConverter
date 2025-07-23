using ExcelLibrary.SpreadSheet;
using KLVConverter.KLV;
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
            KLVReader reader = new(Logger);
            foreach (string datafile in args)
            {
                // check if file exist and open it
                if (File.Exists(datafile))
                {
                    Logger.LogInformation("Process {file} datafile", datafile);
                    List<List<KLVData>> data = reader.ReadFile(datafile);

                    Workbook workbook = new Workbook();
                    Worksheet rawWorksheet = new Worksheet("RawKLVData");
                    Worksheet processedWorksheet = new Worksheet("ProcessedST0601Data");
                    for (int row = 0; row < data.Count; row++)
                    {
                        for (int indexData = 0; indexData < data[row].Count; indexData++)
                        {
                            KLVData localData = data[row][indexData];
                            rawWorksheet.Cells[row + 1, 0] = new Cell(row);
                            processedWorksheet.Cells[row + 1, 0] = new Cell(row);
                            rawWorksheet.Cells[row + 1, localData.Key] = new Cell(string.Join(",", localData.Value));
                        }
                    }
                    rawWorksheet.Cells[0, 0] = new Cell("Id\\Tag");
                    processedWorksheet.Cells[0, 0] = new Cell("Id\\Tag");
                    for (int tagIndex = 1; tagIndex < 128; tagIndex++)
                    {
                        rawWorksheet.Cells[0, tagIndex] = new Cell(tagIndex);
                        processedWorksheet.Cells[0, tagIndex] = new Cell(tagIndex);
                    }
                    workbook.Worksheets.Add(rawWorksheet);
                    workbook.Worksheets.Add(processedWorksheet);
                    workbook.Save("./"+Path.GetFileName(datafile)+".xlsx");

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