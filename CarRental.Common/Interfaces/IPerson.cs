namespace CarRental.Common.Interfaces;

public interface IPerson
{
    public string GetSecurityNumber();
    public string GetSecurityFormat(bool usFormat);
    public string GetFirstname();
    public string GetLastname();
    public string GetFullInfo();
}
