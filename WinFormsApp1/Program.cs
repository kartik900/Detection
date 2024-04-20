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
            "fa94282be9a3a5ea19268d2aebb29a05"
        ];
        // Folder to monitor
        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        string knownHashesFilePath = @"C:\Users\karthik\Desktop\hash.txt";
        List<string> knownHashes = stringList;

        FileSystemWatcher watcher = new();
        watcher.Path = folderPath;

        // Watch for changes in the directory
        watcher.NotifyFilter = NotifyFilters.FileName;

        // Subscribe to the event handler for new files
        watcher.Filter = "*.exe"; // Filter only .exe files
        watcher.Created += (sender, e) =>
        {

            string filePath = e.FullPath;
            string fileName = Path.GetFileName(filePath);
            string fileHash = CalculateFileHash(filePath);
            if (knownHashes.Contains(fileHash))
            {
                MessageBox.Show($"Intrusion Detected : {filePath}.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        };
        watcher.EnableRaisingEvents = true;
        while (true)
            Thread.Sleep(5000);
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
