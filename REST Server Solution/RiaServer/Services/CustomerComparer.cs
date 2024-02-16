using RiaServer.Models;

namespace RiaServer.Services
{
    public class CustomerComparer : IComparer<Customer>
    {
        public int Compare(Customer? x, Customer? y)
        {
            if (x == null || y == null)
            {
                return 0;
            }

            var compareFirstName = string.Compare(x.FirstName, y.FirstName);
            if (compareFirstName != 0)
            {
                return compareFirstName;
            }

            var compareLastName = string.Compare(x.LastName, y.LastName);
            return compareLastName;
        }
    }
}

