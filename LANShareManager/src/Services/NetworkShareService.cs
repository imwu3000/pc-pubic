using System;
using System.Collections.Generic;
using System.Management;
using System.IO;

namespace LANShareManager.Services
{
    public class SharedFolderInfo
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
        // public uint Type { get; set; } // Could be useful for debugging
    }

    public class NetworkShareService
    {
        public List<SharedFolderInfo> GetSharedFolders()
        {
            var sharedFolders = new List<SharedFolderInfo>();
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Share");
                foreach (ManagementObject share in searcher.Get())
                {
                    // Type 0 is a disk drive, which is what we typically want for folder shares
                    uint shareType = Convert.ToUInt32(share["Type"]);
                    if (shareType == 0) // Disk Drive
                    {
                        sharedFolders.Add(new SharedFolderInfo
                        {
                            Name = share["Name"]?.ToString(),
                            Path = share["Path"]?.ToString(),
                            Description = share["Description"]?.ToString() // Or "Caption"
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                // Log exception (Console.WriteLine for now)
                Console.WriteLine($"Error getting shared folders: {ex.Message}");
            }
            return sharedFolders;
        }

        public bool ShareFolder(string path, string shareName, string description)
        {
            if (!Directory.Exists(path))
            {
                Console.WriteLine($"Error: Path not found - {path}");
                return false;
            }

            try
            {
                ManagementClass managementClass = new ManagementClass("Win32_Share");
                ManagementBaseObject inParams = managementClass.GetMethodParameters("Create");

                inParams["Path"] = path;
                inParams["Name"] = shareName;
                inParams["Type"] = 0; // 0 for Disk Drive
                inParams["Description"] = description;
                // For default permissions, access parameter can be null.
                // For specific permissions, a SECURITY_DESCRIPTOR would be needed here.
                // inParams["Access"] = null;

                ManagementBaseObject outParams = managementClass.InvokeMethod("Create", inParams, null);

                uint returnValue = (uint)outParams["ReturnValue"];
                if (returnValue == 0) // Success
                {
                    Console.WriteLine($"Folder '{path}' shared as '{shareName}'.");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Error sharing folder. WMI Create failed with code: {returnValue}");
                    // You can check Win32_Share Create method return codes for specific errors
                    // e.g., 2 = Access Denied, 8 = Unknown Failure, 21 = Invalid Parameter, etc.
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sharing folder '{path}': {ex.Message}");
                return false;
            }
        }

        public bool UnshareFolder(string shareName)
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher($"SELECT * FROM Win32_Share WHERE Name = '{shareName}'");
                ManagementObjectCollection objects = searcher.Get();

                if (objects.Count == 0)
                {
                    Console.WriteLine($"Share '{shareName}' not found.");
                    return false;
                }

                foreach (ManagementObject share in objects)
                {
                    uint returnValue = (uint)share.InvokeMethod("Delete", null, null)["ReturnValue"];
                    if (returnValue == 0) // Success
                    {
                        Console.WriteLine($"Share '{shareName}' deleted successfully.");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine($"Error deleting share '{shareName}'. WMI Delete failed with code: {returnValue}");
                        return false;
                    }
                }
                return false; // Should not be reached if share was found
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error unsharing folder '{shareName}': {ex.Message}");
                return false;
            }
        }
    }
}
