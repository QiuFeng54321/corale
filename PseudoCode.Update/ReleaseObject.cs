namespace PseudoCode.Update;

public class Asset
{
    public string BrowserDownloadUrl { get; set; }
    public string Name { get; set; }
}
public class ReleaseObject
{
    public string TagName { get; set; }
    public string Name { get; set; }
    public List<Asset> Assets { get; set; }
}