using LibgenSharp.Exceptions;

namespace LibgenSharp.Processors;

public class WebClientProcessors
{
    public class ZLibraryWebClientProcessor : DownloadWebClientProcessor
    {
        private string _subLinkXpath = @"/html/body/table/tbody/tr[2]/td/div/div/div/div[2]/div[2]/div[1]/div[1]/div/a";
        public ZLibraryWebClientProcessor()
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

    public class LibgenLolWebClientProcessor : DownloadWebClientProcessor
    {
        public LibgenLolWebClientProcessor()
        {
            _linkXpath = @"//*[@id=""download""]/h2/a";
        }
    }

    public class LibgenRockWebClientProcessor : DownloadWebClientProcessor
    {
        public LibgenRockWebClientProcessor()
        {
            _linkXpath = @"//*[@id=""main""]/tbody/tr[1]/td[2]/a";
        }
    }
}