using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstract_Factory
{
    public abstract class AbstractA
    {
        public abstract void Run();
    }

    public abstract class AbstractB
    {
        public abstract void Interact(AbstractA a);
    }
}
