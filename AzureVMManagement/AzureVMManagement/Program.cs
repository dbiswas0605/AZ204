using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;

namespace AzureVMManagement
{
    class AzureVMManager
    {
        const string _resourcegroupname = "";
        Region _region = Region.USCentral;
        const string _vmname = "AZ204VM";

        internal void Authenticate()
        {
            var credittials = SdkContext.AzureCredentialsFactory
                            .FromServicePrincipal("clientid", "clientsecret", "tenantid", AzureEnvironment.AzureGlobalCloud);



        }

        internal void createVM()
        {

        }



    }





    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
