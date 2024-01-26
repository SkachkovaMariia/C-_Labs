using System;
using System.IO;

public delegate void FileMonitorDelegate(FileEventArgs e);

public class FileEventArgs : EventArgs
{
    public string FilePath { get; }
    public DateTime Date { get; }
    public double FileSize { get; }

    public FileEventArgs(string filePath, DateTime date, double fileSize)
    {
        FilePath = filePath;
        Date = date;
        FileSize = Math.Round(fileSize / 1024);
    }
}

public class FileMonitor
{
    public event FileMonitorDelegate OnFileCreated;
    public event FileMonitorDelegate OnFileDeleted;
    public event FileMonitorDelegate OnFileModified;
    public event FileMonitorDelegate OnFileRenamed;

    private readonly FileSystemWatcher fileSystemWatcher;
    private string[] allowedFileExtensions;

    public FileMonitor(string directoryPath, string[] allowedFileExtensions)
    {
        try
        {
            this.allowedFileExtensions = allowedFileExtensions;

            fileSystemWatcher = new FileSystemWatcher(directoryPath);

            fileSystemWatcher.Created += (sender, e) =>
            {
                if (IsFileAllowed(e.FullPath))
                {
                    OnFileCreated?.Invoke(new FileEventArgs(e.FullPath, DateTime.Now, new FileInfo(e.FullPath).Length));
                }
            };

            fileSystemWatcher.Deleted += (sender, e) =>
            {
                if (IsFileAllowed(e.FullPath))
                {
                    OnFileDeleted?.Invoke(new FileEventArgs(e.FullPath, DateTime.Now, -1));
                }
            };

            fileSystemWatcher.Changed += (sender, e) =>
            {
                if (IsFileAllowed(e.FullPath))
                {
                    OnFileModified?.Invoke(new FileEventArgs(e.FullPath, DateTime.Now, new FileInfo(e.FullPath).Length));
                }
            };

            fileSystemWatcher.Renamed += (sender, e) =>
            {
                if (IsFileAllowed(e.FullPath))
                {
                    OnFileRenamed?.Invoke(new FileEventArgs(e.FullPath, DateTime.Now, new FileInfo(e.FullPath).Length));
                }
            };

            fileSystemWatcher.EnableRaisingEvents = true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error FileMonitor: {ex.Message}");
        }
    }

    public void SubscribeToEvents()
    {
        try
        {
            OnFileCreated += args => Console.WriteLine($"File created: {args.FilePath}, Size: {args.FileSize}, Time: {args.Date}");
            OnFileDeleted += args => Console.WriteLine($"File deleted: {args.FilePath}, Time: {args.Date}");
            OnFileModified += args => Console.WriteLine($"File edited: {args.FilePath}, Size: {args.FileSize}, Time: {args.Date}");
            OnFileRenamed += args => Console.WriteLine($"File renamed: {args.FilePath}, Size: {args.FileSize}, Time: {args.Date}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error SubscribeToEvents: {ex.Message}");
        }
    }

    private bool IsFileAllowed(string filePath)
    {
        try
        {
            if (allowedFileExtensions == null || allowedFileExtensions.Length == 0)
            {
                return true;
            }

            string fileExtension = Path.GetExtension(filePath);
            return Array.Exists(allowedFileExtensions, ext => ext.Equals(fileExtension, StringComparison.OrdinalIgnoreCase));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error IsFileAllowed: {ex.Message}");
            return false;
        }
    }
}

class Program
{
    static void Main()
    {
        string monitoredDirectory = "D:\\УДУ\\C#\\6\\MonitorFolder";
        string[] allowedExtensions = { ".txt", ".docx", ".pdf" };
        FileMonitor fileMonitor = new FileMonitor(monitoredDirectory, allowedExtensions);

        Console.WriteLine($"Monitor system started for: {monitoredDirectory}");

        fileMonitor.SubscribeToEvents();

        Console.ReadLine();
    }
}
