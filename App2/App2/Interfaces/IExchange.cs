using App2.Models.Json;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace App2.Interfaces
{
    public interface IExchange
    {
        void SaveCurrency();
        Info Calculate(string originalCode, string destinationCode, double amount);
     
    }
}
