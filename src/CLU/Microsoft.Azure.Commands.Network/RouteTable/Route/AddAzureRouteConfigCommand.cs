﻿// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Management.Automation;
using Microsoft.Azure.Commands.Network.Models;

namespace Microsoft.Azure.Commands.Network
{
    [Cmdlet(VerbsCommon.Add, "AzureRmRouteConfig"), OutputType(typeof(PSRouteTable))]
    [CliCommandAlias("network route add")]
    public class AddAzureRouteConfigCommand : AzureRouteConfigBase
    {
        [Parameter(
            Mandatory = true,
            ValueFromPipeline = true,
            HelpMessage = "The RouteTable")]
        public PSRouteTable RouteTable { get; set; }

        public override void ExecuteCmdlet()
        {
            base.ExecuteCmdlet();

            // Verify if the Route exists in the RouteTable
            var route = this.RouteTable.Routes.SingleOrDefault(resource => string.Equals(resource.Name, this.Name, System.StringComparison.CurrentCultureIgnoreCase));

            if (route != null)
            {
                throw new ArgumentException("Route with the specified name already exists");
            }
            
            route = new PSRoute();

            route.Name = this.Name;
            route.AddressPrefix = this.AddressPrefix;
            route.NextHopType = this.NextHopType;
            route.NextHopIpAddress = this.NextHopIpAddress;

            this.RouteTable.Routes.Add(route);

            WriteObject(this.RouteTable);
        }
    }
}