using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstract_Factory
{
    public abstract class AbstarctFactory
    {
        public abstract AbstractA CreateA();
        public abstract AbstractB CreateB();
    }
}
