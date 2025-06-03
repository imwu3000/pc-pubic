using System;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace LANShareManager.Services
{
    public class UserPermissionInfo
    {
        public string IdentityReference { get; set; }
        public string FileSystemRights { get; set; }
        public string AccessControlType { get; set; }
    }

    public class FilePermissionService
    {
        public List<UserPermissionInfo> GetPathPermissions(string path)
        {
            var permissions = new List<UserPermissionInfo>();
            if (!File.Exists(path) && !Directory.Exists(path))
            {
                Console.WriteLine($"Error: Path not found - {path}");
                return permissions;
            }

            try
            {
                FileSystemSecurity security;
                if (File.Exists(path))
                {
                    security = new FileSecurity(path, AccessControlSections.Access);
                }
                else
                {
                    security = new DirectorySecurity(path, AccessControlSections.Access);
                }

                AuthorizationRuleCollection accessRules = security.GetAccessRules(true, true, typeof(NTAccount));

                foreach (FileSystemAccessRule rule in accessRules)
                {
                    permissions.Add(new UserPermissionInfo
                    {
                        IdentityReference = rule.IdentityReference.Value,
                        FileSystemRights = rule.FileSystemRights.ToString(),
                        AccessControlType = rule.AccessControlType.ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting permissions for '{path}': {ex.Message}");
            }
            return permissions;
        }

        public bool ModifyPathPermission(string path, string identity, string fileSystemRightsString, string accessControlTypeString, bool addRule)
        {
            if (!File.Exists(path) && !Directory.Exists(path))
            {
                Console.WriteLine($"Error: Path not found - {path}");
                return false;
            }

            try
            {
                if (!Enum.TryParse<FileSystemRights>(fileSystemRightsString, true, out FileSystemRights rights))
                {
                    Console.WriteLine($"Error: Invalid FileSystemRights - {fileSystemRightsString}");
                    return false;
                }

                if (!Enum.TryParse<AccessControlType>(accessControlTypeString, true, out AccessControlType controlType))
                {
                    Console.WriteLine($"Error: Invalid AccessControlType - {accessControlTypeString}");
                    return false;
                }

                NTAccount account = new NTAccount(identity);
                FileSystemAccessRule newRule = new FileSystemAccessRule(account, rights, controlType);

                if (File.Exists(path))
                {
                    FileSecurity fileSecurity = new FileSecurity(path, AccessControlSections.Access);
                    if (addRule)
                        fileSecurity.AddAccessRule(newRule);
                    else
                        fileSecurity.RemoveAccessRule(newRule); // For exact match. RemoveAccessRuleSpecific might be needed for more complex scenarios.
                    File.SetAccessControl(path, fileSecurity);
                }
                else // It's a directory
                {
                    DirectorySecurity directorySecurity = new DirectorySecurity(path, AccessControlSections.Access);
                    if (addRule)
                        directorySecurity.AddAccessRule(newRule);
                    else
                        directorySecurity.RemoveAccessRule(newRule);
                    Directory.SetAccessControl(path, directorySecurity);
                }
                Console.WriteLine($"Permission '{(addRule ? "added" : "removed")}' for '{identity}' on '{path}'.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error modifying permission for '{identity}' on '{path}': {ex.Message}");
                return false;
            }
        }

        public bool AddPathPermission(string path, string identity, string fileSystemRightsString, string accessControlTypeString)
        {
            return ModifyPathPermission(path, identity, fileSystemRightsString, accessControlTypeString, true);
        }

        public bool RemovePathPermission(string path, string identity, string fileSystemRightsString, string accessControlTypeString)
        {
            return ModifyPathPermission(path, identity, fileSystemRightsString, accessControlTypeString, false);
        }
    }
}
