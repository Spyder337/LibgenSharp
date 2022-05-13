namespace LibgenBaseLib.Web
{
    public interface IDownloader
    {
        public string[] XPaths { get; }
        public string MainXPath { get => XPaths[0]; }

        /// <summary>
        /// Attempts to download a file from the internet.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="destPath"></param>
        /// <returns></returns>
        public Task<bool> TryDownload(string url, string destPath);
    }
}
