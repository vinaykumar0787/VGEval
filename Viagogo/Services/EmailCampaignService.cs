using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Viagogo.Contracts;
using Viagogo.Models;

namespace Viagogo.Services
{
    internal class EmailCampaignService : IEmailCampaignService
    {
        public IEnumerable<Event> GetEventsBasedOnCity(Customer customer, List<Event> events)
        {
            // ### Get List of events from event list by using a LINQ and using customer's city in where clause
            // or, a join if we also have a list of customers
            // #1. TASK 
            var query = from result in events
                        where result.City.Contains(customer.City)
                        select result;
            return query;
        }

        public IEnumerable<Event> GetEventsBasedOnCityDistance(Customer customer, List<Event> events)
        {
            // #2. Task - Send 5 closest events to the customer's location to the email
            // ### Linq/Lambda to calculate all events from customer's city and then sort and pick top 5 by distance.
            var eventsWithDistance = events.Select(e => new { Event = e, Distance = ViaGogoCommon.GetDistance(customer.City, e.City) })
                                        .OrderBy((e) => e.Distance)
                                        .Select((e) => e.Event).Take(5);
            return eventsWithDistance;
        }

        public IEnumerable<Event> GetEventsBasedOnCityDistanceExceptionHandled(Customer customer, List<Event> events, Dictionary<(string, string), int> cityDistances)
        {
            // #3. Task - If the GetDistance method is an API call which could fail or is too expensive, how will u improve the code written in 2 ?
            // ### In case the call is expensive and susceptible to fail, we can use Patterns like retry and also since the distance between
            // cities are constant, we can keep a dictionary and store it in a Global/Cache environment. For ex,
            var eventsWithDistance = events.Select(e => new { Event = e, Distance = GetDistanceCustom(customer.City, e.City, cityDistances) })
                            .OrderBy((e) => e.Distance)
                            .Select((e) => e.Event).Take(5);
            return eventsWithDistance;
        }

        public IEnumerable<Event> GetEventsBasedOnCityDistanceAndPrice(Customer customer, List<Event> events, Dictionary<(string, string), int> cityDistances)
        {
            // #2. Task - Send 5 closest events to the customer's location to the email
            // ### Linq/Lambda to calculate all events from customer's city and then sort and pick top 5 by distance.
            var eventsWithDistance = events.Select(e => new { Event = e, Price = ViaGogoCommon.GetPrice(e), Distance = GetDistanceCustom(customer.City, e.City, cityDistances) })
                            .OrderBy((e) => e.Distance).ThenBy((e) => e.Price)
                            .Select((e) => e.Event).Take(5);
            return eventsWithDistance;
        }


        // ### Custom Methods created
        /// <summary>
        /// Takes a dictionary of prefilled Global cityDistances variable, use if a distance already exists else call api and populate it
        /// </summary>
        /// <param name="fromCity"></param>
        /// <param name="toCity"></param>
        /// <param name="cityDistances"></param>
        public int GetDistanceCustom(string fromCity, string toCity, Dictionary<(string, string), int> cityDistances)
        {
            if (cityDistances.TryGetValue((fromCity, toCity), out var distance))
            {
                return distance;
            }
            else
            {
                // #4. Task - If the API fails, we can use a Retry pattern and then based on business wise agreement can skip the event, or break
                // the flow and log it - Need to confirm
                // 1. What should be the logging mecahnism?
                // 2. Can we use event-based architecture to handle this and store the distances already?
                // 3. Should we log the error and continue skipping the current cities or just stop the flow after throwing an exception?

                int retries = 3; // Global Retry count - Store at Global Settings
                while (retries > 0)
                {

                    try
                    {
                        int newDistance = ViaGogoCommon.GetDistance(fromCity, toCity);
                        cityDistances.Add((fromCity, toCity), newDistance);
                        return newDistance;
                    }
                    catch (Exception)
                    {
                        retries--;
                    }
                }
                throw new Exception($"Failed after 3 retries");
            }
        }

    }
}
