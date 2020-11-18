using MediatorPattern.Events;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MediatorPattern.Handlers
{
    public class SendMessageHandler : INotificationHandler<AddCustomerEvent>
    {
        public Task Handle(AddCustomerEvent notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Send sms to {notification.Customer.FullName}");

            return Task.CompletedTask;
        }
    }
}
