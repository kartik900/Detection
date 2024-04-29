using System.IO.Compression;
using System.Security.Cryptography;

class Program
{
    static void Main(string[] args)
    {
        //// Download the CSV file only once
        //string csvData = DownloadString(url);

        //// Transform CSV data into a list of strings
        //List<string> dataList = ParseCsv(csvData);

        // Display the list of strings

        List<string> stringList =
        [
            "25f9de8ba11300bc0c551d963817fbe5",
            "96ab66f2368cf458dbc8574216155898",
            "fa94282be9a3a5ea19268d2aebb29a05",
            "940d3ccda68f07c88ae424d1571ea17"
        ];
        Console.WriteLine("hey");
        // Folder to monitor
        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        string backupPath = "C:/Users/naker/OneDrive/Backup";
        Console.WriteLine("file path"+folderPath);
        
        // string knownHashesFilePath = @"C:\Users\karthik\Desktop\hash.txt";
        List<string> knownHashes = ["940d3ccda68f07c88ae424d1571ea17"];

        FileSystemWatcher watcher = new();
        watcher.Path = folderPath;

        // Watch for changes in the directory
        watcher.NotifyFilter = NotifyFilters.FileName;

        // Subscribe to the event handler for new files
        watcher.Filter = "*.exe"; // Filter only .exe files
        watcher.Created += (sender, e) =>
        {
            //copy to onedrive here
            string filePath = e.FullPath;
            string fileName = Path.GetFileName(filePath);
            string fileHash = CalculateFileHash(filePath);
            //  if (knownHashes.Contains(fileHash))
            //  {
                
                 WriteLogFile("C:/Backup/fileMonitor.txt", $"{filePath}");
                BackupAndZipDirectory(folderPath,backupPath);
                    File.Delete(filePath);
              MessageBox.Show($"Intrusion Detected : {filePath} \n Restore files at:  C:/Users/naker/OneDrive/Backup", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
            // }
        };
        watcher.EnableRaisingEvents = true;
        while (true)
            Thread.Sleep(5000);
    }
       public static void WriteLogFile(string logFilePath, string logMessage)
    {
        try
        {
            // Create or open the log file for writing
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                // Write the log message to the file, along with the current timestamp
                writer.WriteLine($"{logMessage}  {DateTime.Now}");
            }

            Console.WriteLine("Log file created successfully and log written.");
        }
        catch (Exception ex)
        {
            // Handle any exceptions that might occur during log file writing
            Console.WriteLine($"An error occurred while writing to the log file: {ex.Message}");
        }
    }
    static string CalculateFileHash(string filePath)
    {
        Thread.Sleep(1500);
        using var md5 = MD5.Create();
        using var stream = File.OpenRead(filePath);
        byte[] hash = md5.ComputeHash(stream);
        return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
    }

    static List<string> LoadKnownHashes(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine("Known hashes file not found.");
            return [];
        }
        return File.ReadAllLines(filePath).Select(line => line.Trim()).ToList();
    }

    static void BackupAndZipDirectory(string sourceDir, string destDir)
    {
        if (!Directory.Exists(sourceDir))
        {
            Console.WriteLine($"Source directory '{sourceDir}' does not exist.");
            return;
        }

        if (!Directory.Exists(destDir))
        {
            Directory.CreateDirectory(destDir);
        }

        string zipFileName = $"Backup_{DateTime.Now:yyyyMMdd_HHmmss}.zip";
        string zipFilePath = Path.Combine(destDir, zipFileName);

        using (ZipArchive archive = ZipFile.Open(zipFilePath, ZipArchiveMode.Create))
        {
            string[] files = Directory.GetFiles(sourceDir);

            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                string destFile = Path.Combine(destDir, fileName);
                // File.Copy(file, destFile, true); //
                Console.WriteLine($"Copied '{file}' to '{destFile}'.");

                archive.CreateEntryFromFile(file, fileName);
            }
        }

        Console.WriteLine($"Backup files zipped to '{zipFilePath}'.");
    }


    static void BackupDirectory(string sourceDir, string destDir)
    {
        if (!Directory.Exists(sourceDir))
        {
            Console.WriteLine($"Source directory '{sourceDir}' does not exist.");
            return;
        }

        if (!Directory.Exists(destDir))
        {
            Directory.CreateDirectory(destDir);
        }

        string[] files = Directory.GetFiles(sourceDir);

        foreach (string file in files)
        {
            string fileName = Path.GetFileName(file);
            string destFile = Path.Combine(destDir, fileName);
            File.Copy(file, destFile, true); 
            Console.WriteLine($"Copied '{file}' to '{destFile}'.");
        }

        
    }
    //################################################################
    //# MalwareBazaar full malware samples dump (CSV)                #
    //# Last updated: 2024-04-13 09:41:49 UTC                        #
    //#                                                              #
    //# Terms Of Use: https://bazaar.abuse.ch/faq/#tos               #
    //# For questions please contact bazaar [at] abuse.ch            #
    //################################################################
    //#

    static string DownloadString(string url)
    {
        using HttpClient client = new ();
        return client.GetStringAsync(url).Result;
    }

    static List<string> ParseCsv(string csvData)
    {
        List<string> dataList = new List<string>();

        // Split CSV data by new line character
        string[] lines = csvData.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

        // Add each line (column value) to the list
        foreach (string line in lines)
        {
            dataList.Add(line);
        }

        return dataList;
    }
}
