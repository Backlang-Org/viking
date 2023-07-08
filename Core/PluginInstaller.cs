using System.IO.Compression;
using NuGet.Common;
using NuGet.Packaging;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace viking.Core;

public static class PluginInstaller
{
    public static IAsyncEnumerable<(string Title, NuGetVersion Version)> GetAvailablePlugins()
    {
        return Search("backlang");
    }

    public static async IAsyncEnumerable<(string Title, NuGetVersion Version)> Search(string term)
    {
        var cache = new SourceCacheContext();
        var repository = Repository.Factory.GetCoreV3("https://api.nuget.org/v3/index.json");
        var resource = await repository.GetResourceAsync<PackageSearchResource>();
        var searchFilter = new SearchFilter(includePrerelease: false);

        var result =
             resource.SearchAsync(term, searchFilter,
            0, 20, NullLogger.Instance, CancellationToken.None).Result;

        result = result.Where(_ => _.Tags.Contains("backend") || _.Tags.Contains("plugin"));

        foreach (var pck in result)
        {
            yield return (pck.Title, pck.GetVersionsAsync().Result.Last().Version);
        }
    }

    public static async void Install(string pckName)
    {
        var cache = new SourceCacheContext();
        SourceRepository repository = Repository.Factory.GetCoreV3("https://api.nuget.org/v3/index.json");
        FindPackageByIdResource resource = await repository.GetResourceAsync<FindPackageByIdResource>();

        var latestVersion = GetLatestVersion(cache, resource, pckName);

        using MemoryStream packageStream = new MemoryStream();

        resource.CopyNupkgToStreamAsync(
            pckName,
            latestVersion,
            packageStream,
            cache,
            NullLogger.Instance,
            CancellationToken.None).Wait();

        var pluginsDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Backlang", "Plugins", pckName);

        if (!Directory.Exists(pluginsDir))
        {
            Directory.CreateDirectory(pluginsDir);
        }

        resource.CopyNupkgToStreamAsync(
            pckName,
            latestVersion,
            packageStream,
            cache,
            NullLogger.Instance,
            CancellationToken.None).Wait();

        using PackageArchiveReader packageReader = new PackageArchiveReader(packageStream);
        NuspecReader nuspecReader = await packageReader.GetNuspecReaderAsync(CancellationToken.None);

        var deps = nuspecReader.GetDependencyGroups().ToArray()[0].Packages;
        //ToDo: install dependency packages that are not from backlang

        using var archive = new ZipArchive(packageStream, ZipArchiveMode.Read);
        var lib = archive.Entries.Where(_ => _.FullName.StartsWith("lib/"));

        foreach (var libItem in lib)
        {
            libItem.ExtractToFile(Path.Combine(pluginsDir, libItem.Name), true);
        }
    }
    
    public static NuGetVersion GetLatestVersion(SourceCacheContext cache, FindPackageByIdResource resource, string sdkId)
    {
        return resource.GetAllVersionsAsync(
            sdkId,
            cache,
            NullLogger.Instance,
            CancellationToken.None).Result.Last();
    }
}