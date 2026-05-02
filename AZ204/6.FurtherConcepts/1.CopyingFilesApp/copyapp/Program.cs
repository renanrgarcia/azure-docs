string[] fileNames = { "fileA.txt", "fileB.txt", "fileC.txt" };
string destinationPath = @"C:\tmp6";

foreach (string file in fileNames)
    File.Copy($"C:\\tmp4\\{file}", $"{destinationPath}\\{file}", true);

Console.WriteLine("Files copied");