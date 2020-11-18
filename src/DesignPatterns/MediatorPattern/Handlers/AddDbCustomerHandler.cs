using MediatorPattern.Events;
using MediatorPattern.IServices;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MediatorPattern.Handlers
{
    public class AddDbCustomerHandler : INotificationHandler<AddCustomerEvent>
    {
        private readonly ICustomerRepository customerRepository;

        public AddDbCustomerHandler(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }

        public Task Handle(AddCustomerEvent notification, CancellationToken cancellationToken)
        {
            customerRepository.Add(notification.Customer);

            return Task.CompletedTask;
        }
    }
}
