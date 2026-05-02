

string[] fileNames = { "fileA.txt", "fileB.txt", "fileC.txt" };
string destinationPath = "/mnt/destination/"; // In a Linux-based container, the file paths would be different. For example, you might use "/app/files/" as the source path and "/mnt/destination/" as the destination path.

foreach (string file in fileNames)
    File.Copy($"files/{file}", $"{destinationPath}{file}", true);
Console.WriteLine("Files copied");