using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Quartz.WebApi
{
    public class DelayTask
    {
        public class WheelTask<T>
        {
            public T Data { get; set; }
            public Func<T,Task> Handle { get; set; }
        }

        public class TimeWheel<T>
        {
            int secondSlot = 0;
            DateTime wheelTime { get { return new DateTime(1, 1, 1, 0, 0, secondSlot); } }
            Dictionary<int, ConcurrentQueue<WheelTask<T>>> secondTaskQueue;
        }
    }
}
