using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Compute.Fluent.Models;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;

namespace AzureVMManagement
{
    class AzureVMManager
    {
        static internal string _resourcegroupname = "AZ204resourcegroup";
        static internal Region _region = Region.USCentral;
        static internal string _vmname = "AZ204VM-2";

        internal static IAzure Authenticate()
        {
            var credentials = SdkContext.AzureCredentialsFactory
                            .FromServicePrincipal("6b979ff2-135c-4e31-8674-ebcaf858021b", "d_5EQk.98p5NlU_3oDXbmhvWls-EX_1ZjW", "ea670b0e-9cea-4880-b32d-78d1e34f97b0", AzureEnvironment.AzureGlobalCloud);

            var azure = Azure
                .Configure()
                .WithLogLevel(HttpLoggingDelegatingHandler.Level.Basic)
                .Authenticate(credentials)
                .WithSubscription("ec7c4b76-8ce4-4881-bc83-5e13ee9c7080");

            return azure;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            IAzure azure = AzureVMManager.Authenticate();

            if (!azure.ResourceGroups.Contain(AzureVMManager._resourcegroupname))
            {
                var resourceGroup = azure.ResourceGroups.Define(AzureVMManager._resourcegroupname)
                                    .WithRegion(AzureVMManager._region)
                                    .Create();
            }

            var network = azure.Networks.Define("sampleVirtualNetwork")
                          .WithRegion(AzureVMManager._region)
                          .WithExistingResourceGroup(AzureVMManager._resourcegroupname)
                          .WithAddressSpace("10.0.0.0/16")
                          .WithSubnet("sampleSubNet", "10.0.0.0/24")
                          .Create();

            var publicIPAddress = azure.PublicIPAddresses.Define("samplePublicIP-2")
                                .WithRegion(AzureVMManager._region)
                                .WithExistingResourceGroup(AzureVMManager._resourcegroupname)
                                .WithDynamicIP()
                                .Create();

            var networkInterface = azure.NetworkInterfaces.Define("sampleNetWorkInterface-2")
                                    .WithRegion(AzureVMManager._region)
                                    .WithExistingResourceGroup(AzureVMManager._resourcegroupname)
                                    .WithExistingPrimaryNetwork(network)
                                    .WithSubnet("sampleSubNet")
                                    .WithPrimaryPrivateIPAddressDynamic()
                                    .WithExistingPrimaryPublicIPAddress(publicIPAddress)
                                    .Create();

            var availabilitySet = azure.AvailabilitySets.Define("sampleAvailabilitySet")
                                    .WithRegion(AzureVMManager._region)
                                    .WithExistingResourceGroup(AzureVMManager._resourcegroupname)
                                    .WithSku(AvailabilitySetSkuTypes.Aligned)
                                    .Create();

            azure.VirtualMachines.Define(AzureVMManager._vmname)
                                    .WithRegion(AzureVMManager._region)
                                    .WithExistingResourceGroup(AzureVMManager._resourcegroupname)
                                    .WithExistingPrimaryNetworkInterface(networkInterface)
                                    .WithLatestWindowsImage("MicrosoftWindowsServer", "WindowsServer", "2012-R2-Datacenter")
                                    .WithAdminUsername("sampleUser")
                                    .WithAdminPassword("Sample123467")
                                    .WithComputerName(AzureVMManager._vmname)
                                    .WithExistingAvailabilitySet(availabilitySet)
                                    .WithSize(VirtualMachineSizeTypes.BasicA0)
                                    .Create();

        }
    }
}
