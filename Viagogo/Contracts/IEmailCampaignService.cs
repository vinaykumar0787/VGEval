using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Viagogo.Models;

namespace Viagogo.Contracts
{
    internal interface IEmailCampaignService
    {
        IEnumerable<Event> GetEventsBasedOnCity(Customer customer, List<Event> events);
        IEnumerable<Event> GetEventsBasedOnCityDistance(Customer customer, List<Event> events);
        int GetDistanceCustom(string fromCity, string toCity, Dictionary<(string, string), int> cityDistances);
        IEnumerable<Event> GetEventsBasedOnCityDistanceExceptionHandled(Customer customer, List<Event> events, Dictionary<(string, string), int> cityDistances);
        IEnumerable<Event> GetEventsBasedOnCityDistanceAndPrice(Customer customer, List<Event> events, Dictionary<(string, string), int> cityDistances);

    }
}
