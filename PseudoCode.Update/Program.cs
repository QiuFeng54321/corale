// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PseudoCode.Core.Parsing;

namespace PseudoCode.Update;

public class Program
{
    public const string PackageName = "PseudoCodePackage.pkg";
    public const string VsixName = "williamqiufeng.caie-pseudocode";
    public static readonly HttpClient HttpClient = new();

    public static readonly JsonSerializer JsonSerializer = new()
    {
        ContractResolver = new DefaultContractResolver
        {
            NamingStrategy = new SnakeCaseNamingStrategy()
        }
    };

    public static async Task<bool> DownloadAssetAsync(IEnumerable<ReleaseObject> objs, string s)
    {
        var obj = objs.FirstOrDefault(o => o.Assets.Any(a => a.Name == s));
        if (obj == null)
        {
            return false;
        }

        var latestVersion = Version.Parse(obj.TagName);
        var currentVersion = typeof(NewCompiler).Assembly.GetName().Version;
        Console.WriteLine($"Current version of {s} is {currentVersion}, Latest version is {latestVersion}");
        if (latestVersion <= currentVersion)
        {
            Console.WriteLine("Current version is already the latest!");
            return false;
        }

        var asset = obj.Assets.FirstOrDefault(a => a.Name == s);
        if (asset == null)
        {
            Console.WriteLine($"{s} not found!");
            // Environment.Exit(-1);
            return false;
        }

        Console.WriteLine($"Downloading from url \"{asset.BrowserDownloadUrl}\"");
        var response = await HttpClient.GetStreamAsync(asset.BrowserDownloadUrl);
        await response.CopyToAsync(File.Create(s));
        return true;
    }

    public static async Task Main()
    {
        Console.WriteLine("Fetching latest version of PseudoCode...");
        var resultStr =
            await HttpClient.GetStringAsync(
                "https://gitee.com/api/v5/repos/williamcraft/pseudocode-releases/releases?page=1&per_page=20&direction=desc");
        // Console.WriteLine(resultStr);
        var resultObjs = JsonSerializer.Deserialize<ReleaseObject[]>(new JsonTextReader(new StringReader(resultStr)));

        if (await DownloadAssetAsync(resultObjs, PackageName))
        {
            var p = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "open",
                    WorkingDirectory = Environment.CurrentDirectory,
                    Arguments = PackageName
                }
            };
            p.Start();
            await p.WaitForExitAsync();
        }

        var vsixProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "code",
                WorkingDirectory = Environment.CurrentDirectory,
                Arguments = $"--install-extension {VsixName} --force"
            }
        };
        vsixProcess.Start();
        await vsixProcess.WaitForExitAsync();


        Console.WriteLine("Process ended");
    }
}