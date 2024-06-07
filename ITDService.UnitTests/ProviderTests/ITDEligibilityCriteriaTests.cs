using BrockSolutions.DapperWrapper;
using BrockSolutions.ITDService.Options;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Moq;
using BrockSolutions.ITDService.Providers;
using BrockSolutions.ITDService.Data;
using BrockSolutions.SmartSuite.Events;

namespace BrockSolutions.ITDService.UnitTests
{
    public class ITDEligibilityCriteriaTests
    {
        [Fact]
        public void validInput_itdIsEligible()
        {
            ITDEligibilityProvider provider = TestHelpers.CreateITDProvider();

            Passenger validPassenger = new Passenger()
            {
                FlightLegs = new List<Flight>()
                {
                   new Flight()
                   {
                        DepartureStation = "LAX",
                        ArrivalStation = "YYZ",
                        CarrierCode = "AC",
                        DepartureDateLocal = new DateTime(2021, 12, 31).Ticks,
                        FlightNumber = "666",
                        Market = Flight.FlightMarket.Domestic
                   },
                   new Flight()
                   {
                        DepartureStation = "YYZ",
                        ArrivalStation = "YVR",
                        CarrierCode = "AC",
                        DepartureDateLocal = new DateTime(2022, 1, 1).Ticks,
                        FlightNumber = "777",
                        Market = Flight.FlightMarket.Domestic
                   }
                },
                Bags = new List<Bag>()
                {
                    new Bag()
                    {
                        BagId = 1,
                        CheckedBag = true,
                    }
                },
                MarkedAsIneligible = false,
                HasBoardedBSM = false,
                ScannedAtSmartGate = false,
                HasBDXMessage = false
            };

            bool result = provider.CheckIfPassengerisITDEligible(validPassenger, "YYZ");
            Assert.True(result);
        }

        [Fact]
        public void inboundFlightNotEligible_notITDEligible()
        {
            ITDEligibilityProvider provider = TestHelpers.CreateITDProvider();

            Passenger ineligibleRoutePassenger = new Passenger()
            {
                FlightLegs = new List<Flight>()
                {
                   new Flight()
                   {
                        DepartureStation = "DEN",
                        ArrivalStation = "YYZ",
                        CarrierCode = "AC",
                        DepartureDateLocal = new DateTime(2021, 12, 31).Ticks,
                        FlightNumber = "666",
                        Market = Flight.FlightMarket.Domestic
                   },
                   new Flight()
                   {
                        DepartureStation = "YYZ",
                        ArrivalStation = "YVR",
                        CarrierCode = "AC",
                        DepartureDateLocal = new DateTime(2022, 1, 1).Ticks,
                        FlightNumber = "777",
                        Market = Flight.FlightMarket.Domestic
                   }
                },
                Bags = new List<Bag>()
                {
                    new Bag()
                    {
                        BagId = 1,
                        CheckedBag = true,
                    }
                },
                MarkedAsIneligible = false,
                HasBoardedBSM = false,
                ScannedAtSmartGate = false,
                HasBDXMessage = false
            };

            bool result = provider.CheckIfPassengerisITDEligible(ineligibleRoutePassenger, "YYZ");
            Assert.False(result);
        }

        [Fact]
        public void notDomesticConnection_notITDEligible()
        {
            ITDEligibilityProvider provider = TestHelpers.CreateITDProvider();
            Passenger notDomesticPassenger = new Passenger()
            {
                FlightLegs = new List<Flight>()
                {
                   new Flight()
                   {
                        DepartureStation = "LAX",
                        ArrivalStation = "YYZ",
                        CarrierCode = "AC",
                        DepartureDateLocal = new DateTime(2021, 12, 31).Ticks,
                        FlightNumber = "666",
                        Market = Flight.FlightMarket.Domestic
                   },
                   new Flight()
                   {
                        DepartureStation = "YYZ",
                        ArrivalStation = "YVR",
                        CarrierCode = "AC",
                        DepartureDateLocal = new DateTime(2022, 1, 1).Ticks,
                        FlightNumber = "777",
                        Market = Flight.FlightMarket.International
                   }
                },
                Bags = new List<Bag>()
                {
                    new Bag()
                    {
                        BagId = 1,
                        CheckedBag = true,
                    }
                },
                MarkedAsIneligible = false,
                HasBoardedBSM = false,
                ScannedAtSmartGate = false,
                HasBDXMessage = false
            };

            bool result = provider.CheckIfPassengerisITDEligible(notDomesticPassenger, "YYZ");
            Assert.False(result);
        }

        [Fact]
        public void ineligibleCarrier_notITDEligible()
        {
            ITDEligibilityProvider provider = TestHelpers.CreateITDProvider();

            Passenger badCarrierPassenger = new Passenger()
            {
                FlightLegs = new List<Flight>()
                {
                   new Flight()
                   {
                        DepartureStation = "LAX",
                        ArrivalStation = "YYZ",
                        CarrierCode = "VA",
                        DepartureDateLocal = new DateTime(2021, 12, 31).Ticks,
                        FlightNumber = "666",
                        Market = Flight.FlightMarket.Domestic
                   },
                   new Flight()
                   {
                        DepartureStation = "YYZ",
                        ArrivalStation = "YVR",
                        CarrierCode = "VA",
                        DepartureDateLocal = new DateTime(2022, 1, 1).Ticks,
                        FlightNumber = "777",
                        Market = Flight.FlightMarket.Domestic
                   }
                },
                Bags = new List<Bag>()
                {
                    new Bag()
                    {
                        BagId = 1,
                        CheckedBag = true,
                    }
                },
                MarkedAsIneligible = false,
                HasBoardedBSM = false,
                ScannedAtSmartGate = false,
                HasBDXMessage = false
            };

            bool result = provider.CheckIfPassengerisITDEligible(badCarrierPassenger, "YYZ");
            Assert.False(result);
        }

        [Fact]
        public void noCheckedBags_notITDEligible()
        {
            ITDEligibilityProvider provider = TestHelpers.CreateITDProvider();

            Passenger noCheckedBagPassenger = new Passenger()
            {
                FlightLegs = new List<Flight>()
                {
                   new Flight()
                   {
                        DepartureStation = "LAX",
                        ArrivalStation = "YYZ",
                        CarrierCode = "AC",
                        DepartureDateLocal = new DateTime(2021, 12, 31).Ticks,
                        FlightNumber = "666",
                        Market = Flight.FlightMarket.Domestic
                   },
                   new Flight()
                   {
                        DepartureStation = "YYZ",
                        ArrivalStation = "YVR",
                        CarrierCode = "AC",
                        DepartureDateLocal = new DateTime(2022, 1, 1).Ticks,
                        FlightNumber = "777",
                        Market = Flight.FlightMarket.Domestic
                   }
                },
                Bags = new List<Bag>()
                {
                    new Bag()
                    {
                        BagId = 1,
                        CheckedBag = false,
                    }
                },
                MarkedAsIneligible = false,
                HasBoardedBSM = false,
                ScannedAtSmartGate = false,
                HasBDXMessage = false
            };

            bool result = provider.CheckIfPassengerisITDEligible(noCheckedBagPassenger, "YYZ");
            Assert.False(result);
        }

        [Fact]
        public void inputMarkedAsIneligible_notITDEligible()
        {
            ITDEligibilityProvider provider = TestHelpers.CreateITDProvider();

            Passenger markedIneligiblePassenger = new Passenger()
            {
                FlightLegs = new List<Flight>()
                {
                   new Flight()
                   {
                        DepartureStation = "LAX",
                        ArrivalStation = "YYZ",
                        CarrierCode = "AC",
                        DepartureDateLocal = new DateTime(2021, 12, 31).Ticks,
                        FlightNumber = "666",
                        Market = Flight.FlightMarket.Domestic
                   },
                   new Flight()
                   {
                        DepartureStation = "YYZ",
                        ArrivalStation = "YVR",
                        CarrierCode = "AC",
                        DepartureDateLocal = new DateTime(2022, 1, 1).Ticks,
                        FlightNumber = "777",
                        Market = Flight.FlightMarket.Domestic
                   }
                },
                Bags = new List<Bag>()
                {
                    new Bag()
                    {
                        BagId = 1,
                        CheckedBag = true,
                    }
                },
                MarkedAsIneligible = true,
                HasBoardedBSM = false,
                ScannedAtSmartGate = false,
                HasBDXMessage = false
            };

            bool result = provider.CheckIfPassengerisITDEligible(markedIneligiblePassenger, "YYZ");
            Assert.False(result);
        }

        [Fact]
        public void ineligibleFlight_notITDEligible()
        {
            ITDEligibilityProvider provider = TestHelpers.CreateITDProvider();

            Passenger ineligibleFlightPassenger = new Passenger()
            {
                FlightLegs = new List<Flight>()
                {
                   new Flight()
                   {
                        DepartureStation = "LAX",
                        ArrivalStation = "YYZ",
                        CarrierCode = "AC",
                        DepartureDateLocal = new DateTime(2021, 12, 31).Ticks,
                        FlightNumber = "666",
                        Market = Flight.FlightMarket.Domestic
                   },
                   new Flight()
                   {
                        DepartureStation = "YYZ",
                        ArrivalStation = "YVR",
                        CarrierCode = "AC",
                        DepartureDateLocal = new DateTime(2022, 1, 1).Ticks,
                        FlightNumber = "999",
                        Market = Flight.FlightMarket.Domestic
                   }
                },
                Bags = new List<Bag>()
                {
                    new Bag()
                    {
                        BagId = 1,
                        CheckedBag = true,
                    }
                },
                MarkedAsIneligible = false,
                HasBoardedBSM = false,
                ScannedAtSmartGate = false,
                HasBDXMessage = false
            };

            bool result = provider.CheckIfPassengerisITDEligible(ineligibleFlightPassenger, "YYZ");
            Assert.False(result);
        }
    }
}