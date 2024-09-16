using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace ZL.SemanticKernelDemo.Plugins
{
    public class JLPlugin
    {
        [KernelFunction("GetCustomerInfo")]
        [Description("Retrieve customer email based on the given customer ID.")]
        public async Task<Customer> GetCustomerInfoAsync(string customerId)
        {
            return new Customer { Email = "justin.li@testing.com" };
        }

        public class Customer 
        { 
            public string Email { get; set; }  
        }

    }
}
