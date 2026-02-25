
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Threading.Tasks;

namespace Ledger.BitUI
{
    public static class HardwareInventory
    {
        private const ulong BytesPerGigabyte = 1073741824UL /*0x40000000*/;

        public static List<CpuInfo> GetCPUs()
        {
            CpuQueryState state = new CpuQueryState();
            state.cpus = new ConcurrentBag<CpuInfo>();

            using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("select * from Win32_Processor"))
            {
                Parallel.ForEach<ManagementObject>(
                    managementObjectSearcher.Get().Cast<ManagementObject>(),
                    new Action<ManagementObject>(state.AddCpuFromWmiObject)
                );
            }

            return state.cpus.ToList<CpuInfo>();
        }

        public static List<GpuInfo> GetGPUs()
        {
            List<GpuInfo> gpus = new List<GpuInfo>();

            using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("select * from Win32_VideoController"))
            {
                foreach (ManagementObject managementObject in managementObjectSearcher.Get())
                {
                    gpus.Add(new GpuInfo()
                    {
                        Name = managementObject["Name"]?.ToString(),
                        DeviceID = managementObject["DeviceID"]?.ToString(),
                        AdapterRAM = HardwareInventory.ConvertToGB(managementObject["AdapterRAM"]),
                        DriverVersion = managementObject["DriverVersion"]?.ToString(),
                        VideoProcessor = managementObject["VideoProcessor"]?.ToString(),
                        VideoMemoryType = managementObject["VideoMemoryType"]?.ToString()
                    });
                }
            }

            return gpus;
        }

        public static List<RamStickInfo> GetRAMSticks()
        {
            List<RamStickInfo> ramSticks = new List<RamStickInfo>();

            using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("select * from Win32_PhysicalMemory"))
            {
                foreach (ManagementObject managementObject in managementObjectSearcher.Get())
                {
                    ramSticks.Add(new RamStickInfo()
                    {
                        CapacityGB = (uint)(ulong.Parse(managementObject["Capacity"].ToString()) / 1073741824UL /*0x40000000*/),
                        SpeedMHz = uint.Parse(managementObject["Speed"].ToString()),
                        Manufacturer = managementObject["Manufacturer"]?.ToString(),
                        PartNumber = managementObject["PartNumber"]?.ToString(),
                        SerialNumber = managementObject["SerialNumber"]?.ToString()
                    });
                }
            }

            return ramSticks;
        }

        public static List<DiskInfo> GetDisks()
        {
            List<DiskInfo> disks = new List<DiskInfo>();

            using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("select * from Win32_DiskDrive"))
            {
                foreach (ManagementObject managementObject in managementObjectSearcher.Get())
                {
                    disks.Add(new DiskInfo()
                    {
                        Model = managementObject["Model"]?.ToString(),
                        SizeGB = ulong.Parse(managementObject["Size"].ToString()) / 1073741824UL /*0x40000000*/,
                        InterfaceType = managementObject["InterfaceType"]?.ToString(),
                        MediaType = managementObject["MediaType"]?.ToString(),
                        Partitions = managementObject["Partitions"]?.ToString()
                    });
                }
            }

            return disks;
        }

        private static ulong ConvertToGB(object value)
        {
            return value == null ? 0UL : ulong.Parse(value.ToString()) / 1073741824UL /*0x40000000*/;
        }

        // Reconstructed compiler-generated closure type for GetCPUs()
        private sealed class CpuQueryState
        {
            public ConcurrentBag<CpuInfo> cpus;

            public void AddCpuFromWmiObject(ManagementObject managementObject)
            {
              
                this.cpus.Add(new CpuInfo()
                {
                    Name = managementObject["Name"]?.ToString(),
                    ProcessorId = managementObject["ProcessorId"]?.ToString(),
                    DeviceID = managementObject["DeviceID"]?.ToString(),
                    Manufacturer = managementObject["Manufacturer"]?.ToString(),
                    CurrentClockSpeed = managementObject["CurrentClockSpeed"]?.ToString(),
                    NumberOfCores = managementObject["NumberOfCores"]?.ToString(),
                    NumberOfLogicalProcessors = managementObject["NumberOfLogicalProcessors"]?.ToString(),
                    Architecture = managementObject["Architecture"]?.ToString()
                });
            }
        }

        public class GpuInfo
        {
            public string Name { get; set; }

            public string DeviceID { get; set; }

            public ulong AdapterRAM { get; set; }

            public string DriverVersion { get; set; }

            public string VideoProcessor { get; set; }

            public string VideoMemoryType { get; set; }

            public override string ToString() => $"{this.Name} - {this.AdapterRAM} GB RAM";
        }

        public class CpuInfo
        {
            public string Name { get; set; }

            public string ProcessorId { get; set; }

            public string DeviceID { get; set; }

            public string Manufacturer { get; set; }

            public string CurrentClockSpeed { get; set; }

            public string NumberOfCores { get; set; }

            public string NumberOfLogicalProcessors { get; set; }

            public string Architecture { get; set; }

            public override string ToString() => $"{this.Name} - {this.NumberOfCores} Cores";
        }

        public class RamStickInfo
        {
            public uint CapacityGB { get; set; }

            public uint SpeedMHz { get; set; }

            public string Manufacturer { get; set; }

            public string PartNumber { get; set; }

            public string SerialNumber { get; set; }

            public override string ToString() => $"{this.CapacityGB} GB RAM @ {this.SpeedMHz} MHz";
        }

        public class DiskInfo
        {
            public string Model { get; set; }

            public ulong SizeGB { get; set; }

            public string InterfaceType { get; set; }

            public string MediaType { get; set; }

            public string Partitions { get; set; }

            public override string ToString() => $"{this.Model} - {this.SizeGB} GB";
        }
    }
}
