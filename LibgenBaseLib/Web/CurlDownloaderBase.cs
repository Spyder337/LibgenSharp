using CurlThin;
using CurlThin.Enums;
using CurlThin.Helpers;

namespace LibgenBaseLib.Web
{
    public abstract class CurlDownloaderBase : IDownloader
    {
        public abstract string[] XPaths { get; }
        public string UserAgent { get => "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.97 Safari/537.11"; }

        public abstract Task<bool> TryDownload(string url, string destPath);
    }

    public class CurlDownloaderLol : CurlDownloaderBase
    {
        public override string[] XPaths => throw new NotImplementedException();

        public override async Task<bool> TryDownload(string url, string destPath)
        {
            return await Task.Run(() =>
            {
                var global = CurlNative.Init();
                CurlThin.Native.CurlResources.Init();
                var ca_cert_path = CurlThin.Native.CurlResources.CaBundlePath;
                if (global != CURLcode.OK) return false;

                var dataCopier = new DataCallbackCopier();
                var easy = CurlNative.Easy.Init();
                CurlNative.Easy.SetOpt(easy, CURLoption.CAINFO, ca_cert_path);
                CurlNative.Easy.SetOpt(easy, CURLoption.URL, url);
                CurlNative.Easy.SetOpt(easy, CURLoption.WRITEDATA, dataCopier.DataHandler);
                var res = CurlNative.Easy.Perform(easy);
                if (res == CURLcode.OK)
                {
                    using var fs = File.Create(destPath);
                    fs.Write(dataCopier.Stream.ToArray());
                    return true;
                }
                CurlNative.Easy.Cleanup(easy.DangerousGetHandle());
                CurlNative.Cleanup();

                return false;
            });
        }
    }
}
