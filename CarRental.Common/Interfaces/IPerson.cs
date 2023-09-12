using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Common.Interfaces;

public interface IPerson
{
    public int GetSecurityNumber();
    public string GetFirstname();
    public string GetLastname();
    public string GetFullInfo();
}
