using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrockSolutions.SmartSuite.Events;
using BrockSolutions.ITDService.Mappers;
using BrockSolutions.ITDService.Data;

namespace BrockSolutions.ITDService.UnitTests.MapperTests
{
    public class EligibilityMapperTests
    {
        [Fact]
        public void bagCreatedEventWithCheckedBags_mapToEligibilityInputWithCheckedBag()
        {
            BagFlightLeg bagFlightLeg = new BagFlightLeg {
                BagType = "CheckedBag",
                FlightDepartureStationCode = "YYZ",
            };
            FlightLeg flightLeg = new FlightLeg
            {
                FlightNumber = "123",
            };
            BagCreated bagCreatedWithCheckedBags = new BagCreated
            {
                StationCode = "YYZ",
            };
            bagCreatedWithCheckedBags.BagFlightLegs.Add(bagFlightLeg);
            bagCreatedWithCheckedBags.FlightLegs.Add(flightLeg);

            ITDEligibilityInput expectedResult = new ITDEligibilityInput()
            {
                HasCheckedBag = true,
                FlightLegs = bagCreatedWithCheckedBags.FlightLegs.ToList(),
                StationCode = "YYZ",
                ITDEligibilityString = "ITDY",
            };
            ITDEligibilityInput actualResult = EligibilityMapper.MapToEligibilityInput(bagCreatedWithCheckedBags);
            Assert.Equal(expectedResult.HasCheckedBag, actualResult.HasCheckedBag);
            Assert.Equal(expectedResult.StationCode, actualResult.StationCode);
            Assert.Equal(expectedResult.ITDEligibilityString, actualResult.ITDEligibilityString);
            Assert.Equal(expectedResult.FlightLegs[0].FlightNumber, actualResult.FlightLegs[0].FlightNumber);
            Assert.Equal(expectedResult.FlightLegs.Count, actualResult.FlightLegs.Count);
        }

        [Fact]
        public void bagCreatedEventWithoutCheckedBags_mapToEligibilityInputWithoutCheckedBag()
        {
            BagFlightLeg bagFlightLeg = new BagFlightLeg
            {
                BagType = "MobilityAid",
                FlightDepartureStationCode = "YYZ",
            };
            FlightLeg flightLeg = new FlightLeg
            {
                FlightNumber = "123",
            };
            BagCreated bagCreatedWithoutCheckedBags = new BagCreated
            {
                StationCode = "YYZ",
            };
            bagCreatedWithoutCheckedBags.BagFlightLegs.Add(bagFlightLeg);
            bagCreatedWithoutCheckedBags.FlightLegs.Add(flightLeg);

            ITDEligibilityInput expectedResult = new ITDEligibilityInput()
            {
                HasCheckedBag = false,
                FlightLegs = bagCreatedWithoutCheckedBags.FlightLegs.ToList(),
                StationCode = "YYZ",
                ITDEligibilityString = "ITDY",
            };
            ITDEligibilityInput actualResult = EligibilityMapper.MapToEligibilityInput(bagCreatedWithoutCheckedBags);
            Assert.Equal(expectedResult.HasCheckedBag, actualResult.HasCheckedBag);
            Assert.Equal(expectedResult.StationCode, actualResult.StationCode);
            Assert.Equal(expectedResult.ITDEligibilityString, actualResult.ITDEligibilityString);
            Assert.Equal(expectedResult.FlightLegs[0].FlightNumber, actualResult.FlightLegs[0].FlightNumber);
            Assert.Equal(expectedResult.FlightLegs.Count, actualResult.FlightLegs.Count);
        }

        [Fact]
        public void bagCreatedEventWithNoBags_mapToEligibilityInputWithoutCheckedBag()
        {
            FlightLeg flightLeg = new FlightLeg
            {
                FlightNumber = "123",
            };
            BagCreated bagCreatedWithNoBags = new BagCreated
            {
                StationCode = "YYZ",
            };
            bagCreatedWithNoBags.FlightLegs.Add(flightLeg);

            ITDEligibilityInput expectedResult = new ITDEligibilityInput()
            {
                HasCheckedBag = false,
                FlightLegs = bagCreatedWithNoBags.FlightLegs.ToList(),
                StationCode = "YYZ",
                ITDEligibilityString = "ITDY",
            };
            ITDEligibilityInput actualResult = EligibilityMapper.MapToEligibilityInput(bagCreatedWithNoBags);
            Assert.Equal(expectedResult.HasCheckedBag, actualResult.HasCheckedBag);
            Assert.Equal(expectedResult.StationCode, actualResult.StationCode);
            Assert.Equal(expectedResult.ITDEligibilityString, actualResult.ITDEligibilityString);
            Assert.Equal(expectedResult.FlightLegs[0].FlightNumber, actualResult.FlightLegs[0].FlightNumber);
            Assert.Equal(expectedResult.FlightLegs.Count, actualResult.FlightLegs.Count);
        }

        [Fact]
        public void bagCreatedEventWithCheckedBagsForOtherFlights_mapToEligibilityInputWithoutCheckedBag()
        {
            BagFlightLeg firstBagFlightLeg = new BagFlightLeg
            {
                BagType = "CheckedBag",
                FlightDepartureStationCode = "DEN",
            };
            BagFlightLeg secondBagFlightLeg = new BagFlightLeg
            {
                BagType = "MobilityAid",
                FlightDepartureStationCode = "YYZ",
            };
            FlightLeg firstFlightLeg = new FlightLeg
            {
                FlightNumber = "123",
            };
            FlightLeg secondFlightLeg = new FlightLeg
            {
                FlightNumber = "456",
            };
            BagCreated bagCreatedWithCheckedBags = new BagCreated
            {
                StationCode = "YYZ",
            };
            bagCreatedWithCheckedBags.BagFlightLegs.Add(firstBagFlightLeg);
            bagCreatedWithCheckedBags.FlightLegs.Add(firstFlightLeg);
            bagCreatedWithCheckedBags.BagFlightLegs.Add(secondBagFlightLeg);
            bagCreatedWithCheckedBags.FlightLegs.Add(secondFlightLeg);

            ITDEligibilityInput expectedResult = new ITDEligibilityInput()
            {
                HasCheckedBag = false,
                FlightLegs = bagCreatedWithCheckedBags.FlightLegs.ToList(),
                StationCode = "YYZ",
                ITDEligibilityString = "ITDY",
            };
            ITDEligibilityInput actualResult = EligibilityMapper.MapToEligibilityInput(bagCreatedWithCheckedBags);
            Assert.Equal(expectedResult.HasCheckedBag, actualResult.HasCheckedBag);
            Assert.Equal(expectedResult.StationCode, actualResult.StationCode);
            Assert.Equal(expectedResult.ITDEligibilityString, actualResult.ITDEligibilityString);
            Assert.Equal(expectedResult.FlightLegs.Count, actualResult.FlightLegs.Count);
            Assert.Equal(expectedResult.FlightLegs[0].FlightNumber, actualResult.FlightLegs[0].FlightNumber);
            Assert.Equal(expectedResult.FlightLegs[1].FlightNumber, actualResult.FlightLegs[1].FlightNumber);
        }

        /*[Fact]
        public void bagItineraryUpdatedEvent()
        {
            BagFlightLeg bagFlightLeg = new BagFlightLeg
            {
                BagType = "CheckedBag",
                FlightDepartureStationCode = "YYZ",
            };
            FlightLeg flightLeg = new FlightLeg
            {
                FlightNumber = "123",
            };
            BagCreated bagCreatedWithCheckedBags = new BagCreated
            {
                StationCode = "YYZ",
            };
            bagCreatedWithCheckedBags.BagFlightLegs.Add(bagFlightLeg);
            bagCreatedWithCheckedBags.FlightLegs.Add(flightLeg);

            ITDEligibilityInput expectedResult = new ITDEligibilityInput()
            {
                HasCheckedBag = true,
                FlightLegs = bagCreatedWithCheckedBags.FlightLegs.ToList(),
                StationCode = "YYZ",
                ITDEligibilityString = "ITDY",
            };
            ITDEligibilityInput actualResult = EligibilityMapper.MapToEligibilityInput(bagCreatedWithCheckedBags);
            Assert.Equal(expectedResult.HasCheckedBag, actualResult.HasCheckedBag);
            Assert.Equal(expectedResult.StationCode, actualResult.StationCode);
            Assert.Equal(expectedResult.ITDEligibilityString, actualResult.ITDEligibilityString);
            Assert.Equal(expectedResult.FlightLegs[0].FlightNumber, actualResult.FlightLegs[0].FlightNumber);
            Assert.Equal(expectedResult.FlightLegs.Count, actualResult.FlightLegs.Count);
        }*/
    }
}
