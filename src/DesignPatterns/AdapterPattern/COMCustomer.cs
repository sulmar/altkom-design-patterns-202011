using System;

namespace CrystalDecisions.CrystalReports
{
    public class COMCustomer
    {
        public int Pobierz(int id)
        {
            throw new NotImplementedException();
        }
    }

    public interface ICustomerAdapter
    {
        int Get(int id);
    }



}
