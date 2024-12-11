using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Customer.io.Polling.Models
{
    public class BroadcastEventResponse
    {
        public List<Broadcast> Broadcasts { get; set; }
    }

    public class Broadcast
    {
        public int Id { get; set; }
        public string DeduplicateId { get; set; }
        public long Created { get; set; }
        public string Type { get; set; }
        public long Updated { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public string State { get; set; }
        public List<string> Actions { get; set; }
        public List<string> Tags { get; set; }
        public long FirstStarted { get; set; }
        public string CreatedBy { get; set; }
    }
}
