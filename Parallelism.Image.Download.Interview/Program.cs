using System.Text.Json;
using Parallelism.Image.Download.Interview.Models;


int selectedItemIndex= ConsoleHelper.MultipleChoice( "Do you want to input as input?", "Do you want to enter as json file?");
Input input = new();

if (selectedItemIndex == 0)
{
    Console.WriteLine("Enter the number of images to download:");
    input.Count = int.Parse(Console.ReadLine());

    Console.WriteLine("Enter the maximum parallel download limit:");
    input.Parallelism = int.Parse(Console.ReadLine());

    Console.WriteLine($"Enter the save path (default: {input.SavePath})");
    string inputSavePath = Console.ReadLine();

    if (!string.IsNullOrEmpty(inputSavePath))
        input.SavePath = inputSavePath;

}
else
{
jsonFile:
    Console.WriteLine("Please write the json file path!");

    string jsonPath = Console.ReadLine();
    if (string.IsNullOrEmpty(jsonPath))
    {
        goto jsonFile;
    }

    string json = File.ReadAllText(jsonPath);
    input = JsonSerializer.Deserialize<Input>(json);

}


var downloadImageExecuter = new DownloadImageExecuter(input);

Console.CancelKeyPress += delegate (object? sender, ConsoleCancelEventArgs e) {
    e.Cancel = true;

    downloadImageExecuter.ClearAllImages(input.SavePath);
};

downloadImageExecuter.DownloadImageEvent += Console.WriteLine;

downloadImageExecuter.Execute();

downloadImageExecuter.DownloadImageEvent -= Console.WriteLine;
