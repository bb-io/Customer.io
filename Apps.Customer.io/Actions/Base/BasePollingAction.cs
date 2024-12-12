using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apps.Customer.io.Api;
using Apps.Customer.io.Invocables;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;

namespace Apps.Customer.io.Actions.Base
{
    public abstract class BasePollingAction : CustomerIoInvocable
    {
        protected readonly CustomerIoClient Client;

        protected BasePollingAction(InvocationContext invocationContext, IFileManagementClient fileManagementClient)
        : base(invocationContext)
        {
            Client = new CustomerIoClient(invocationContext.AuthenticationCredentialsProviders);
        }
    }
}
