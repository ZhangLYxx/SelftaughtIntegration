using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.MediatR.cmd.SecondHandCar
{
    public class Membercmd: INotification
    {
        public string Name { get; set; }
    }
}
