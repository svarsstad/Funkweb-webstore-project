/*
This an imaginary bank service to get currncy info. to limit the project this will be static.
*/

namespace Project_Backend.Services

{
    public class CurrencyService
    {

        public double GetExchangeRate(string currency)
        {
            //if reaver bucks(scrap)-product, demand much more
            if (currency == "SCR-P") { return 20; }
            if (currency == "REA-P") { return 50; }
            if (currency == "SPLI-C") { return 3; }
            if (currency == "IND-MET") { return 2; }

            //default:
            return 1;
        }
        public string GetCreditSymbol(string cur)
        {
            if (cur.StartsWith("CZK - IID"))
            {
                return "⌬";
            }
            else if (cur.StartsWith("SCR-P"))
            {
                return "✕";
            }
            else if (cur.StartsWith("REA-P"))
            {
                return "☠";
            }
            else if (cur.StartsWith("SPLI-C"))
            {
                return "⛓";
            }
            else if (cur.StartsWith("IND-MET"))
            {
                return "⚙";
            }
            else
            {
                return "⌬";
            }
        }
    }
}
