using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.Models
{
    public class CountryInfo
    {
        public int Id { get; set; }
        public string CountryName { get; set; }
        public int NumberOfState { get; set; }
        //public List<CountryInfo> countryInfos { get; set; }
    }
    
}



namespace SharedModels.Models.CountryInfoList
{
    public class Countries
    {
        public List<CountryInfo> CountryInfos { get; set; }
    }

}