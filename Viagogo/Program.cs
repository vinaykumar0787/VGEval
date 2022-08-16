using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Viagogo.Contracts;
using Viagogo.Models;
using Viagogo.Services;

namespace Viagogo
{

    public class Solution
    {
        static void Main(string[] args)
        {
            // DI to be used
            IEmailCampaignService _campaignService = new EmailCampaignService();

            var events = new List<Event>{
            new Event{ Name = "Phantom of the Opera", City = "New York"},
            new Event{ Name = "Metallica", City = "Los Angeles"},
            new Event{ Name = "Metallica", City = "New York"},
            new Event{ Name = "Metallica", City = "Boston"},
            new Event{ Name = "LadyGaGa", City = "New York"},
            new Event{ Name = "LadyGaGa", City = "Boston"},
            new Event{ Name = "LadyGaGa", City = "Chicago"},
            new Event{ Name = "LadyGaGa", City = "San Francisco"},
            new Event{ Name = "LadyGaGa", City = "Washington"}
            };

            //1. find out all events that arein cities of customer
            // then add to email.
            var customer = new Customer { Name = "Mr. Fake", City = "New York" };


            // #1. TASK 
            var customerEvents = _campaignService.GetEventsBasedOnCity(customer, events);

            foreach (var item in customerEvents)
            {
                // ### Should be a async method to avoid deadlocks based on external email service being called 
                // or, rather the AddToEmail method must accept a list of events and send them together in one call.
                ViaGogoCommon.AddToEmail(customer, item);
            } /**
            We want you to send an email to this customer with all events in their city
            * Just call AddToEmail(customer, event) for each event you think they should get
            */


            // #2. Task - Send 5 closest events to the customer's location to the email
            // ### Linq/Lambda to calculate all events from customer's city and then sort and pick top 5 by distance.
            var eventsWithDistance = _campaignService.GetEventsBasedOnCityDistance(customer, events);
            foreach (var item in eventsWithDistance)
            {
                ViaGogoCommon.AddToEmail(customer, item);
            }

            // #3. #4. Task - If the GetDistance method is an API call which could fail or is too expensive, how will u improve the code written in 2 ?
            // ### In case the call is expensive and susceptible to fail, we can use Patterns like retry and also since the distance between
            // cities are constant, we can keep a dictionary and store it in a Global/Cache environment. For ex,
            Dictionary<(string, string), int> cityDistances = new Dictionary<(string, string), int>();
            var eventsWithDistance1 = _campaignService.GetEventsBasedOnCityDistanceExceptionHandled(customer, events, cityDistances);
            foreach (var item in eventsWithDistance1)
            {
                ViaGogoCommon.AddToEmail(customer, item);
            }


            // #5. Task - If we also want to sort the resulting events by other fields like price, etc. to determine which
            // ones to send to the customer, how would you implement it?
            // ### We can use the solution done in above step and Sort by Price after sorting by Distance using LINQ/Lambda
            var eventsWithDistanceAndPrice = _campaignService.GetEventsBasedOnCityDistanceAndPrice(customer, events, cityDistances);
            foreach (var item in eventsWithDistance1)
            {
                ViaGogoCommon.AddToEmail(customer, item);
            }
        }        
    }
} 
/*
var customers = new List<Customer>{
new Customer{ Name = "Nathan", City = "New York"},
new Customer{ Name = "Bob", City = "Boston"},
new Customer{ Name = "Cindy", City = "Chicago"},
new Customer{ Name = "Lisa", City = "Los Angeles"}
};
*/