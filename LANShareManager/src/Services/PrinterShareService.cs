using System;
using System.Collections.Generic;
using System.Management;

namespace LANShareManager.Services
{
    public class PrinterInfo
    {
        public string Name { get; set; }
        public bool IsShared { get; set; }
        public string ShareName { get; set; }
        public string Status { get; set; } // Corresponds to PrinterStatus
        public bool IsDefault { get; set; }
    }

    public class PrinterShareService
    {
        private string GetPrinterStatusDescription(ushort printerStatus)
        {
            // Win32_Printer PrinterStatus property values
            // This is a simplified mapping. A more comprehensive one might be needed.
            switch (printerStatus)
            {
                case 1: return "Other";
                case 2: return "Unknown";
                case 3: return "Idle";
                case 4: return "Printing";
                case 5: return "Warmup";
                case 6: return "Stopped printing";
                case 7: return "Offline";
                default: return "N/A";
            }
        }

        public List<PrinterInfo> GetPrinters()
        {
            var printers = new List<PrinterInfo>();
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Printer");
                foreach (ManagementObject printer in searcher.Get())
                {
                    printers.Add(new PrinterInfo
                    {
                        Name = printer["Name"]?.ToString(),
                        IsShared = Convert.ToBoolean(printer["Shared"]),
                        ShareName = printer["ShareName"]?.ToString(),
                        Status = GetPrinterStatusDescription(Convert.ToUInt16(printer["PrinterStatus"])),
                        IsDefault = Convert.ToBoolean(printer["Default"])
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting printers: {ex.Message}");
            }
            return printers;
        }

        public bool SetPrinterShare(string printerName, string shareName, bool share)
        {
            try
            {
                // It's important to escape single quotes in the printer name for the WQL query
                string query = $"SELECT * FROM Win32_Printer WHERE Name = '{printerName.Replace("'", "''")}'";
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
                ManagementObjectCollection objects = searcher.Get();

                if (objects.Count == 0)
                {
                    Console.WriteLine($"Printer '{printerName}' not found.");
                    return false;
                }

                foreach (ManagementObject printer in objects) // Should be only one
                {
                    printer["Shared"] = share;
                    if (share)
                    {
                        printer["ShareName"] = shareName;
                    }
                    // If unsharing, Win32_Printer automatically nullifies ShareName.
                    // Explicitly setting it to null might cause issues if it's not allowed by WMI or the specific driver.
                    // else { printer["ShareName"] = null; } // Let WMI handle clearing ShareName

                    printer.Put(); // Save the changes
                    Console.WriteLine($"Printer '{printerName}' {(share ? "shared as '" + shareName + "'" : "unshared")}.");
                    return true;
                }
                return false; // Should ideally not be reached if a printer was found
            }
            catch (ManagementException mex) // More specific exception for WMI issues
            {
                 Console.WriteLine($"WMI Error {(share ? "sharing" : "unsharing")} printer '{printerName}': {mex.Message} (Error Code: {mex.ErrorCode})");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error {(share ? "sharing" : "unsharing")} printer '{printerName}': {ex.Message}");
                return false;
            }
        }

        public bool SharePrinter(string printerName, string shareName)
        {
            if (string.IsNullOrWhiteSpace(shareName))
            {
                Console.WriteLine("Share name cannot be empty when sharing a printer.");
                return false;
            }
            return SetPrinterShare(printerName, shareName, true);
        }

        public bool UnsharePrinter(string printerName)
        {
            // When unsharing, shareName is not strictly needed for SetPrinterShare,
            // but passing null is consistent with its signature.
            return SetPrinterShare(printerName, null, false);
        }
    }
}
