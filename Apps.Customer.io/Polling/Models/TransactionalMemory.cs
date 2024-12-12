using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Customer.io.Polling.Models
{
    public class TransactionalMemory
    {
        public DateTime? LastPollingTime { get; set; }
        public bool Triggered { get; set; }
    }
}
