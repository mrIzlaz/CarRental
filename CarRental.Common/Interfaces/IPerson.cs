namespace CarRental.Common.Interfaces;

public interface IPerson
{
    public string GetSecurityNumber();
    public string GetSecurityFormated(bool USFormat);
    public string GetFirstname();
    public string GetLastname();
    public string GetFullInfo();
}
