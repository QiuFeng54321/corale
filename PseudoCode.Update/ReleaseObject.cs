using Newtonsoft.Json;

namespace PseudoCode.Update;

public class Asset
{
    [JsonConstructor]
    public Asset()
    {
    }

    public string BrowserDownloadUrl { get; set; }
    public string Name { get; set; }
}

public class ReleaseObject
{
    [JsonConstructor]
    public ReleaseObject()
    {
    }

    public string TagName { get; set; }
    public string Name { get; set; }
    public List<Asset> Assets { get; set; }
}