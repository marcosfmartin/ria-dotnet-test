using RiaServer.Models;

namespace RiaServer.Services
{

    public class CustomerService
    {
        public void ValidateCustomerFields(Customer customer, List<Customer> cachedCustomers)
        {
            if (cachedCustomers.Find(x => x.Id == customer.Id) != null)
            {
                throw new BadHttpRequestException($"Id {customer.Id} is already taken");
            }
            if (customer.Age < 18)
            {
                throw new BadHttpRequestException($"Age cannot be under 18 for customer with id {customer.Id}");
            }
            if (customer.FirstName == null)
            {
                throw new BadHttpRequestException($"First Name missing for customer with id {customer.Id}");
            }
            if (customer.LastName == null)
            {
                throw new BadHttpRequestException($"Last Name missing for customer with id {customer.Id}");
            }
        }
        public void AddCustomerInOrder(Customer customer, List<Customer> cachedCustomers)
        {
            var customerComparer = new CustomerComparer();
            lock (cachedCustomers)
            {
                if (cachedCustomers.Count == 0)
                {
                    cachedCustomers.Add(customer);
                    return;
                }
                var compareToLastInList = customerComparer.Compare(cachedCustomers[cachedCustomers.Count - 1], customer);
                if (compareToLastInList <= 0)
                {
                    cachedCustomers.Add(customer);
                    return;
                }
                var compareToFirstInList = customerComparer.Compare(cachedCustomers[0], customer);
                if (compareToFirstInList >= 0)
                {
                    cachedCustomers.Insert(0, customer);
                    return;
                }
                int index = cachedCustomers.BinarySearch(customer, customerComparer);
                if (index < 0)
                    index = ~index;
                cachedCustomers.Insert(index, customer);
            }
        }
    }
}
