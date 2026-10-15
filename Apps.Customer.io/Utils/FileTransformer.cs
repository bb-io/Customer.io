using System.Text;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Utils.Extensions.Files;
using Blackbird.Filters.Bilingual.Xliff2;
using Blackbird.Filters.Transformations;

namespace Apps.Customer.io.Utils;

public static class FileTransformer
{
    public static async Task<Stream> ToHtml(Stream fileStream, FileReference inputFile)
    { 
        var bytes = await fileStream.GetByteData();

        if (!Xliff2Serializer.IsXliff2(new MemoryStream(bytes), out _)) 
            return new MemoryStream(bytes);
        
        var loadResult = Transformation.Load(new MemoryStream(bytes), inputFile.Name);
        if (!loadResult.Success)
            throw new PluginMisconfigurationException(loadResult.Error);

        var targetLoadResult = loadResult.Value.Target();
        return !targetLoadResult.Success 
            ? throw new PluginMisconfigurationException(targetLoadResult.Error) 
            : targetLoadResult.Value.ToStream();
    }

    public static async Task<string> ToHtmlString(Stream fileStream, FileReference inputFile)
    {
        await using var stream = await ToHtml(fileStream, inputFile);
        using var reader = new StreamReader(stream, Encoding.UTF8);
        return await reader.ReadToEndAsync();
    }
}