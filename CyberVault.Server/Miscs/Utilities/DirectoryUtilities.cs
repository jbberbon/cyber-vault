using CyberVault.Server.DTO.Directory;

namespace CyberVault.Server.Miscs.Utilities;

public static class DirectoryUtilities
{
    public static string RemoveTailingForwardSlash(string path)
    {
        // Check if the path is empty or doesn't end with a forward slash
        if (string.IsNullOrEmpty(path) || !path.EndsWith('/'))
        {
            return path;
        }

        // Remove the trailing forward slash
        return path.Substring(0, path.Length - 1);
    }

    public static string RemoveRootDirectory(string rootDirectory, string path)
    {
        return path.StartsWith(rootDirectory + "/") ? path.Substring(rootDirectory.Length + 1) : path;
    }

    public static (string name, string serverAssignedId) SplitNameAndServerAssignedId(string folderOrBlobName)
    {
        var lastPlusIndex = folderOrBlobName.LastIndexOf('+');

        // If a '+' symbol is found, split into name and serverAssignedId
        if (lastPlusIndex != -1)
        {
            var name = folderOrBlobName.Substring(0, lastPlusIndex);
            var serverAssignedId = folderOrBlobName.Substring(lastPlusIndex + 1);
            serverAssignedId = RemoveTailingForwardSlash(serverAssignedId);

            return (name, serverAssignedId);
        }

        // If no '+' symbol is found, return the entire string as the name and an empty string as the serverAssignedId
        return (folderOrBlobName, string.Empty);
    }

    public static string PluckLastDirectoryElement(string path)
    {
        var lastSlashIndex = path.LastIndexOf('/');

        // If slash symbol is found
        return lastSlashIndex != -1 ? path.Substring(lastSlashIndex + 1) : path;
    }
    
    public static List<DirectoryArrayDto> ConvertPathToArray(string path)
    {
        var pathArray = new List<DirectoryArrayDto>();
        
        // If the path is empty, return an empty array
        if (string.IsNullOrEmpty(path))
        {
            return pathArray;
        }
        
        var splitString = path.Split('/').ToList();
        foreach (var t in splitString)
        {
            var (name, serverAssignedId) = SplitNameAndServerAssignedId(t);
            pathArray.Add(new DirectoryArrayDto
            {
                Name = name,
                ServerAssignedId = serverAssignedId
            });
        }

        return pathArray;
    }
}