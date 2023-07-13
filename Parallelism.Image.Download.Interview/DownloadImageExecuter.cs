using System.Net;
using Parallelism.Image.Download.Interview.Models;

public class DownloadImageExecuter
{
    private static readonly string IMAGE_URL = "https://picsum.photos/200/300";
    private readonly int _maxImageDownloadCount = 0;
    private readonly int _parallelismCount = 0;
    private readonly string _imageSavePath;
    private int _downloadedImageCount = 0;
    private readonly CancellationTokenSource cancellationToken = new CancellationTokenSource();
    private object countLock = new object();

    public DownloadImageExecuter(Input input)
    {
        if (input is null)
            throw new ArgumentNullException(nameof(input));

        _maxImageDownloadCount = input.Count;
        _parallelismCount = input.Parallelism;

        if (!Directory.Exists(input.SavePath))
            Directory.CreateDirectory(input.SavePath);

        if (!input.SavePath.EndsWith('/'))
            input.SavePath += "/";

        _imageSavePath = input.SavePath;
    }

    public delegate void DownloadImageEventHandler(string message);
    public event DownloadImageEventHandler? DownloadImageEvent;

    /// <summary>
    /// Represents the parallel action to be executed for the image download process
    /// </summary>
    private void ImageDownloadAction()
    {
        lock (countLock)
        {
            if (_downloadedImageCount > _maxImageDownloadCount)
            {
                cancellationToken.CancelAfter(1);
                return;
            }

            ImageDownloadEventHandler(_downloadedImageCount++, DownloadImageEvent);
        }

        Thread.Sleep(900);// I added a sleep function to indicate the initiation of parallel threads based on the specified number
    }

    /// <summary>
    /// Refers to the method called to trigger the image download event
    /// </summary>
    public void Execute()
    {
        if (DownloadImageEvent is null)
            throw new ArgumentNullException(nameof(DownloadImageEvent));

        Console.Clear();
        Console.WriteLine($"Downloading {_maxImageDownloadCount} images ({_parallelismCount} parallel downloads at most)");

        Action[] actions = Enumerable.Repeat(ImageDownloadAction, _parallelismCount).ToArray();

        for (int j = 1; _downloadedImageCount <= _maxImageDownloadCount; j++)
        {
            try
            {
                Parallel.Invoke(new ParallelOptions
                {
                    CancellationToken = cancellationToken.Token,
                }, actions);
            }
            catch (Exception)
            {
                break;
            }
        }
    }

    /// <summary>
    /// It cleans up downloaded images
    /// </summary>
    /// <param name="imagesPath">File path to save the image</param>
    public void ClearAllImages(string imagesPath)
    {
        DirectoryInfo di = new(imagesPath);

        di.GetFiles()
          .ToList()
          .ForEach((file) => file.Delete());

        Environment.Exit(0);
    }

    /// <summary>
    /// Deletes the last line in the console
    /// </summary>
    private static void ClearLastConsoleLine()
    {
        Console.SetCursorPosition(0, 1);
    }

    /// <summary>
    /// Represents an event used for downloading images
    /// </summary>
    /// <param name="downloadedImageCount">The parameter indicates the total number of downloaded images</param>
    /// <param name="eventHandler">Handler event</param>
    private void ImageDownloadEventHandler(int downloadedImageCount, DownloadImageEventHandler? eventHandler)
    {

        DownloadFile($"{_imageSavePath}{downloadedImageCount + 1}.jpg", new Uri(IMAGE_URL));

        eventHandler?.Invoke($"Progress: {downloadedImageCount}/{_maxImageDownloadCount}");

        ClearLastConsoleLine();
    }

    /// <summary>
    /// Downloads a file based on the provided URL and saves it to the specified file path
    /// </summary>
    /// <param name="path">File path to save the image</param>
    /// <param name="uri">Uri to download the image</param>
    private void DownloadFile(string path, Uri uri)
    {
        using (WebClient webClient = new())
        {
            webClient.DownloadFile(uri, path);
        }
    }
}