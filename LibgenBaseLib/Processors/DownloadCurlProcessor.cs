
using CurlThin;
using CurlThin.Enums;
using CurlThin.Helpers;

namespace LibgenSharp.Processors;

public class DownloadCurlProcessor : DownloadProcessor
{
    public override bool TryDownload(string link, string path)
    {
        var global = CurlNative.Init();
        CurlThin.Native.CurlResources.Init();
        var ca_cert_path = CurlThin.Native.CurlResources.CaBundlePath;
        if (global != CURLcode.OK) return false;
        
        var dataCopier = new DataCallbackCopier();
        var easy = CurlNative.Easy.Init();
        CurlNative.Easy.SetOpt(easy, CURLoption.CAINFO, ca_cert_path);
        CurlNative.Easy.SetOpt(easy, CURLoption.URL, link);
        CurlNative.Easy.SetOpt(easy, CURLoption.NOPROGRESS, 0);
        CurlNative.Easy.SetOpt(easy, CURLoption.WRITEDATA, dataCopier.DataHandler);
        var res = CurlNative.Easy.Perform(easy);
        if (res == CURLcode.OK)
        {
            using var fs = File.Create(path);
            fs.Write(dataCopier.Stream.ToArray());
            return true;
        }
        CurlNative.Easy.Cleanup(easy.DangerousGetHandle());
        CurlNative.Cleanup();

        return false;
    }
}