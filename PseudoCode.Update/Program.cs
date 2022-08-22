// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PseudoCode.Core.Runtime;

namespace PseudoCode.Update;

public class Program
{
    public static readonly HttpClient HttpClient = new();

    public static readonly JsonSerializer JsonSerializer = new()
    {
        ContractResolver = new DefaultContractResolver
        {
            NamingStrategy = new SnakeCaseNamingStrategy()
        }
    };

    public const string PackageName = "PseudoCodePackage.pkg";
    public const string VsixName = "caie-pseudocode.vsix";

    public static async Task<bool> DownloadAssetAsync(ReleaseObject obj, string s)
    {
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
        var resultStr =
            await HttpClient.GetStringAsync(
                "https://gitee.com/api/v5/repos/williamcraft/pseudocode-releases/releases/latest");
        Console.WriteLine(resultStr);
        var resultObj = JsonSerializer.Deserialize<ReleaseObject>(new JsonTextReader(new StringReader(resultStr)));
        var latestVersion = Version.Parse(resultObj.TagName);
        Console.WriteLine($"Current version is {PseudoProgram.Version}, Latest version is {latestVersion}");
        if (latestVersion <= PseudoProgram.Version)
        {
            Console.WriteLine("Current version is already the latest!");
            Environment.Exit(0);
        }

        if (await DownloadAssetAsync(resultObj, PackageName))
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

        if (await DownloadAssetAsync(resultObj, VsixName))
        {
            var p = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "code",
                    WorkingDirectory = Environment.CurrentDirectory,
                    Arguments = $"--install-extension {VsixName}"
                }
            };
            p.Start();
            await p.WaitForExitAsync();
        }


        
        Console.WriteLine("Process ended");
    }
}