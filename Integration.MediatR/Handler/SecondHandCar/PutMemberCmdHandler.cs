using Integration.MediatR.cmd.SecondHandCar;
using MediatR;

namespace Integration.MediatR.Handler.SecondHandCar
{
    public class PutMemberCmdHandler : INotificationHandler<Membercmd>
    {
        public Task Handle(Membercmd notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
