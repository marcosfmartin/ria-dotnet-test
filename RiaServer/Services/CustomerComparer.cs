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

            var compareFirstName = string.Compare(x.FirstName, y.LastName);
            if (compareFirstName != 0)
            {
                return compareFirstName;
            }

            var compareLastName = string.Compare(x.FirstName, y.LastName);
            return compareLastName;
        }
    }
}

