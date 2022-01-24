using App2.Interfaces;
using App2.Models;
using App2.Models.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net;

namespace App2.Repositories
{
    public class ExchangeRepo : IExchange
    {
        private readonly ExchangeDetailContext _context;
        public ExchangeRepo(ExchangeDetailContext context)
        {
            _context = context;
        }

        public Info Calculate(string originalCode, string destinationCode, double amount)
        {
            ExchangeDetail? originalDetail = null;
            ExchangeDetail? destinationDetail = null;
            SqlParameter[] parms = null;
            Info ans = new Info();
            ans.originalCode = originalCode;
            ans.destinationCode = destinationCode;
            string sqlStr = "exec getCurrency @Code";
            string? jsonStr = null;
            List<RootObject>? res = null;

            //get result from online
            try
            {
                var webClient = new WebClient();
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                jsonStr = webClient.DownloadString("https://nbg.gov.ge/gw/api/ct/monetarypolicy/currencies/ka/json");
                res = JsonConvert.DeserializeObject<List<RootObject>>(jsonStr);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            if (res != null)
            {
                var obj = res[0];
                if (!originalCode.Equals("GEL"))
                {
                    originalDetail = (from item in obj.Currencies
                                      where item.Code.Equals(originalCode)
                                      select new ExchangeDetail
                                      {
                                          Code = item.Code,
                                          Quantity = item.Quantity,
                                          RateFormated = item.RateFormated,
                                          DiffFormated = item.DiffFormated,
                                          Rate = item.Rate,
                                          Name = item.Name,
                                          Diff = item.Diff,
                                          Date = item.Date,
                                          ValidFromDate = item.ValidFromDate
                                      }).FirstOrDefault();

                    ans.conversionDate = originalDetail.Date;
                }

                if (!destinationCode.Equals("GEL"))
                {
                    destinationDetail = (from item in obj.Currencies
                                         where item.Code.Equals(destinationCode)
                                         select new ExchangeDetail
                                         {
                                             Code = item.Code,
                                             Quantity = item.Quantity,
                                             RateFormated = item.RateFormated,
                                             DiffFormated = item.DiffFormated,
                                             Rate = item.Rate,
                                             Name = item.Name,
                                             Diff = item.Diff,
                                             Date = item.Date,
                                             ValidFromDate = item.ValidFromDate
                                         }).FirstOrDefault();

                    ans.conversionDate = destinationDetail.Date;
                }
                
            }
            else
            {
                if (!originalCode.Equals("GEL"))
                {


                    parms = new SqlParameter[]
                        {

                        new SqlParameter { ParameterName = "@Code", Value = originalCode }
                        };
                    originalDetail = _context.ExchangeDetails.FromSqlRaw<ExchangeDetail>(sqlStr, parms).ToList()[0];
                    ans.conversionDate = originalDetail.Date;
                }

                if (!destinationCode.Equals("GEL"))
                {
                    parms = new SqlParameter[]
                    {
                    // Create parameter(s)    
                    new SqlParameter { ParameterName = "@Code", Value = destinationCode }
                    };

                    destinationDetail = _context.ExchangeDetails.FromSqlRaw<ExchangeDetail>(sqlStr, parms).ToList()[0];
                    ans.conversionDate = destinationDetail.Date;
                }
            }
            double result = 0, singleCost;
            if (originalCode.Equals(destinationCode))
            {
                result = amount;
                ans.originalRate = 1;
                ans.destinationRate = 1;
            }

            else if (originalDetail == null && destinationDetail != null)
            {
                singleCost = Convert.ToDouble(destinationDetail.Quantity / destinationDetail.RateFormated);
                result = amount * singleCost;
                ans.originalRate = singleCost;
                ans.destinationRate = Convert.ToDouble(destinationDetail.RateFormated / destinationDetail.Quantity);
            }
            else if (originalDetail != null && destinationDetail == null)
            {
                singleCost = Convert.ToDouble(originalDetail.RateFormated / originalDetail.Quantity);
                result = amount * singleCost;
                ans.originalRate = singleCost;
                ans.destinationRate = Convert.ToDouble(originalDetail.Quantity / originalDetail.RateFormated);
            }
            else
            {
                double GEL = amount * Convert.ToDouble(originalDetail.RateFormated) / Convert.ToDouble(originalDetail.Quantity);
                result = GEL * Convert.ToDouble(destinationDetail.Quantity) / Convert.ToDouble(destinationDetail.RateFormated);
                /*
                 5 Usd -> ? rubl
                 1 Usd -> rubl

                 1 Usd -> 3 gel
                 4 gel -> 100 rubl
                 */
                double oneOriginalToGEL = Convert.ToDouble(originalDetail.RateFormated) / Convert.ToDouble(originalDetail.Quantity);
                double oneOriginalToDest = oneOriginalToGEL * Convert.ToDouble(destinationDetail.Quantity) / Convert.ToDouble(destinationDetail.RateFormated);
                ans.originalRate = oneOriginalToDest;
                /*
                 1 rubl -> ? Usd
                 */
                double oneDestinationToGEL = Convert.ToDouble(destinationDetail.RateFormated) / Convert.ToDouble(destinationDetail.Quantity);
                double oneDestinationToOrig = oneDestinationToGEL * Convert.ToDouble(originalDetail.Quantity) / Convert.ToDouble(originalDetail.RateFormated);
                ans.destinationRate = oneDestinationToOrig;
            }

           
            ans.result = Math.Round(result, 4);
            ans.originalRate = Math.Round(ans.originalRate, 5);
            ans.destinationRate = Math.Round(ans.destinationRate, 5);
            return ans;
        }

        public void SaveCurrency()
        {
            try
            {
                var webClient = new WebClient();
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var jsonStr = webClient.DownloadString("https://nbg.gov.ge/gw/api/ct/monetarypolicy/currencies/ka/json");
                var res = JsonConvert.DeserializeObject<List<RootObject>>(jsonStr);

                var obj = res[0];
                ExchangeDetail? USD = (from item in obj.Currencies
                                       where item.Code.Equals("USD")
                                       select new ExchangeDetail
                                       {
                                           Code = item.Code,
                                           Quantity = item.Quantity,
                                           RateFormated = item.RateFormated,
                                           DiffFormated = item.DiffFormated,
                                           Rate = item.Rate,
                                           Name = item.Name,
                                           Diff = item.Diff,
                                           Date = item.Date,
                                           ValidFromDate = item.ValidFromDate
                                       }).FirstOrDefault();

                if (USD != null)
                    _context.ExchangeDetails.Add(USD);


                ExchangeDetail? Euro = (from item in obj.Currencies
                                        where item.Code.Equals("EUR")
                                        select new ExchangeDetail
                                        {
                                            Code = item.Code,
                                            Quantity = item.Quantity,
                                            RateFormated = item.RateFormated,
                                            DiffFormated = item.DiffFormated,
                                            Rate = item.Rate,
                                            Name = item.Name,
                                            Diff = item.Diff,
                                            Date = item.Date,
                                            ValidFromDate = item.ValidFromDate
                                        }).FirstOrDefault();

                if (Euro != null)
                    _context.ExchangeDetails.Add(Euro);

                ExchangeDetail? GBP = (from item in obj.Currencies
                                       where item.Code.Equals("GBP")
                                       select new ExchangeDetail
                                       {
                                           Code = item.Code,
                                           Quantity = item.Quantity,
                                           RateFormated = item.RateFormated,
                                           DiffFormated = item.DiffFormated,
                                           Rate = item.Rate,
                                           Name = item.Name,
                                           Diff = item.Diff,
                                           Date = item.Date,
                                           ValidFromDate = item.ValidFromDate
                                       }).FirstOrDefault();

                if (GBP != null)
                    _context.ExchangeDetails.Add(GBP);


                ExchangeDetail? RUB = (from item in obj.Currencies
                                       where item.Code.Equals("RUB")
                                       select new ExchangeDetail
                                       {
                                           Code = item.Code,
                                           Quantity = item.Quantity,
                                           RateFormated = item.RateFormated,
                                           DiffFormated = item.DiffFormated,
                                           Rate = item.Rate,
                                           Name = item.Name,
                                           Diff = item.Diff,
                                           Date = item.Date,
                                           ValidFromDate = item.ValidFromDate
                                       }).FirstOrDefault();

                if (RUB != null)
                    _context.ExchangeDetails.Add(RUB);

                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

        }


    }
}
