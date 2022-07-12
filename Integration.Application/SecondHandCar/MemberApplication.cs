using Integration.Application.Contracts.SecondHandCar;
using Integration.MediatR.cmd.SecondHandCar;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Application.SecondHandCar
{
    public class MemberApplication : IMember
    {
        //private readonly IMediator _mediator;
        //public MemberApplication(IMediator mediator)
        //{
        //    _mediator = mediator;
        //}

        public Task GetAsync()
        {
            var commond = new Membercmd
            {
                Name = "测试"
            };
            return Task.CompletedTask;
        }
    }
}
