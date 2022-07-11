using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Kafka
{
    public class EventData
    {
        public string TopicName { get; set; }


        public string Message { get; set; }


        public DateTime EventTime { get; set; }
    }
}
