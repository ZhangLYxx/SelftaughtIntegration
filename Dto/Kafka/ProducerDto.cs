using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCmember.Dto.Kafka
{
    public class ProducerDto
    {
        public string TopicName { get; set; }

        public string Message { get; set; }
    }
}
