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
        if (path.StartsWith(rootDirectory + "/"))
        {
            return path.Substring(rootDirectory.Length + 1);
        }

        return path; // No change if it doesn't start with the root directory
    }

    public static (string name, string serverAssignedId) SplitNameAndServerAssignedId(string folderOrBlobName)
    {
        int lastPlusIndex = folderOrBlobName.LastIndexOf('+');

        // If a '+' symbol is found, split into name and serverAssignedId
        if (lastPlusIndex != -1)
        {
            string name = folderOrBlobName.Substring(0, lastPlusIndex);
            string serverAssignedId = folderOrBlobName.Substring(lastPlusIndex + 1);
            serverAssignedId = RemoveTailingForwardSlash(serverAssignedId);

            return (name, serverAssignedId);
        }

        // If no '+' symbol is found, return the entire string as the name and an empty string as the serverAssignedId
        return (folderOrBlobName, string.Empty);
    }

    public static string PluckLastDirectoryElement(string path)
    {
        int lastSlashIndex = path.LastIndexOf('/');
        
        // If slash symbol is found
        if (lastSlashIndex != -1)
        {
            return path.Substring(lastSlashIndex + 1);
        }
        
        // If no slash symbol found
        return path;
    }
}