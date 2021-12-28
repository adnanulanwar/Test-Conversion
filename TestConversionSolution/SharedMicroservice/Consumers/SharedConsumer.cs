using MassTransit;
using SharedModels.Models;
using SharedModels.Models.CountryInfoList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedMicroservice.Consumers
{
    public class SharedConsumer : IConsumer<CountryInfo>
    {
        List<CountryInfo> countries = new List<CountryInfo>()
        {
            new CountryInfo(){ Id = 1, CountryName = "BD", NumberOfState = 5},
            new CountryInfo(){ Id = 2, CountryName = "BD", NumberOfState = 5},
            new CountryInfo(){ Id = 3, CountryName = "BD", NumberOfState = 5},
            new CountryInfo(){ Id = 4, CountryName = "BD", NumberOfState = 5},
            new CountryInfo(){ Id = 5, CountryName = "BD", NumberOfState = 5},
            new CountryInfo(){ Id = 6, CountryName = "BD", NumberOfState = 5},

        };

        public async Task Consume(ConsumeContext<CountryInfo> context)
        {
            //return context.RespondAsync(context.Message);
            await context.RespondAsync(GetCountries());
        }



        public List<CountryInfo> GetCountries()
        {
            Countries countryList = new Countries();
            countryList.CountryInfos = countries;
            return countryList.CountryInfos;
        }




        //public CountryInfo GetCountries()
        //{
        //    CountryInfo country = new CountryInfo();
        //    country.countryInfos = countries;
        //    return country;
        //}
    }
}
