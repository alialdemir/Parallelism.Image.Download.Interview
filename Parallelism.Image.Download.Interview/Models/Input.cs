namespace Parallelism.Image.Download.Interview.Models
{
	public class Input
	{

        /// <summary>
        /// Total number of images to be downloaded
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Number of images to be downloaded in parallel
        /// </summary>
		public int Parallelism { get; set; }

        /// <summary>
        /// File path where the images will be saved
        /// </summary>
		public string SavePath { get; set; } = "./outputs";
    }
}

