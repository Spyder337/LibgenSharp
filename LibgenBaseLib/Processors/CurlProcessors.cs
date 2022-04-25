using LibgenSharp.Exceptions;

namespace LibgenSharp.Processors;

public class CurlProcessors
{
    public class ZLibraryCurlProcessor : DownloadCurlProcessor
    {
        private string _subLinkXpath = @"/html/body/table/tbody/tr[2]/td/div/div/div/div[2]/div[2]/div[1]/div[1]/div/a";
        public ZLibraryCurlProcessor()
        {
            _linkXpath = @"/html/body/table/tbody/tr[2]/td/div/div/div/div[2]/div[2]/div/table/tbody/tr/td[2]/table/tbody/tr[1]/td/h3/a";
        }

        public override bool TryDownload(string url, string path)
        {
            var doc = GetDoc(url);
            if (doc == null)
            {
                throw new PageNotFoundException();
            }
            var subLink = GetLink(doc, _subLinkXpath);
            return base.TryDownload(subLink, path);
        }
    }

    public class LibgenLolCurlProcessor : DownloadCurlProcessor
    {
        public LibgenLolCurlProcessor()
        {
            _linkXpath = @"//*[@id=""download""]/h2/a";
        }
    }

    public class LibgenRockCurlProcessor : DownloadCurlProcessor
    {
        public LibgenRockCurlProcessor()
        {
            _linkXpath = @"//*[@id=""main""]/tbody/tr[1]/td[2]/a";
        }
    }
}