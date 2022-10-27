// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.IO.Compression;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PseudoCode.Core.Parsing;

namespace PseudoCode.Update;

public class Program
{
    public const string PackageName = "PseudoCodePackage.pkg";
    public const string VsixName = "williamqiufeng.caie-pseudocode";
    public const string WinZipName = "pseudocode.zip";
    public static readonly HttpClient HttpClient = new();

    public static readonly JsonSerializer JsonSerializer = new()
    {
        ContractResolver = new DefaultContractResolver
        {
            NamingStrategy = new SnakeCaseNamingStrategy()
        }
    };

    public static readonly Version CurrentVersion = typeof(PseudoFileCompiler).Assembly.GetName().Version;

    private static async Task RunProcessAsync(Process p)
    {
        p.Start();
        await p.WaitForExitAsync();
    }

    private static async Task<bool> DownloadAssetAsync(IEnumerable<ReleaseObject> objs, string s)
    {
        var obj = objs.FirstOrDefault(o => o.Assets.Any(a => a.Name == s));
        if (obj == null)
        {
            return false;
        }

        var latestVersion = Version.Parse(obj.TagName);
        Console.WriteLine($"Current version of {s} is {CurrentVersion}, Latest version is {latestVersion}");
        if (latestVersion <= CurrentVersion)
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
        var selfExe = Assembly.GetExecutingAssembly().Location;
        Console.WriteLine(selfExe);

        if (OperatingSystem.IsWindows())
        {
            if (await DownloadAssetAsync(resultObjs, WinZipName))
            {
                File.Move(selfExe, $"{selfExe}.bak");
                ZipFile.ExtractToDirectory(WinZipName,
                    Path.GetDirectoryName(selfExe) ?? throw new InvalidOperationException(), true);
            }
        }
        else
        {
            if (await DownloadAssetAsync(resultObjs, PackageName))
                await RunProcessAsync(new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "open",
                        WorkingDirectory = Environment.CurrentDirectory,
                        Arguments = PackageName
                    }
                });
        }

        await RunProcessAsync(new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "code",
                WorkingDirectory = Environment.CurrentDirectory,
                Arguments = $"--install-extension {VsixName} --force"
            }
        });

        Console.WriteLine("Process ended");
    }
}