namespace LibgenSharp.Processors;

public interface IProcessor<T>
{
    public void Process(out T result, params string[] args);
}

public interface IDownloadProcessor<T> : IProcessor<T>
{
    public bool TryDownload(string link, string path);
}