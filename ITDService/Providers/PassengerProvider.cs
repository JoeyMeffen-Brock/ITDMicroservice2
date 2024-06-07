using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrockSolutions.DapperWrapper;
using BrockSolutions.ITDService.Data;
using BrockSolutions.ITDService.Options;
using BrockSolutions.SmartSuite.Events;
using Microsoft.Extensions.Options;

namespace BrockSolutions.ITDService.Providers
{
    public class PassengerProvider
    {
        private readonly IDapperQueryExecutor _dapperQueryExecutor;
        private readonly ILogger _logger;
        private readonly IOptionsMonitor<ITDServiceParameters> _parameters;

        // This is a placeholder for a database of passengers. Replace this and all references to it with real database lookups when we have the database.
        private static readonly List<Passenger> PLACEHOLDER_PASSENGER_DATABASE = new List<Passenger> { new Passenger() };

        private static readonly string CHECKED_BAG_TYPE = "Checked";

        protected StateProvider _stateProvider;

        public PassengerProvider
        (
            ILogger<PassengerProvider> logger,
            IDapperQueryExecutor queryExecutor,
            IOptionsMonitor<ITDServiceParameters> parameters
        )
        {
            _dapperQueryExecutor = queryExecutor;
            _logger = logger;
            _parameters = parameters;
            _stateProvider = new JSONStateProvider();
        }

        public virtual Passenger UpdatePassengerFromBagCreated(BagCreated bagCreatedEvent) 
        {
            try
            {
                Passenger passengerToUpdate = _stateProvider.GetPassengerByBookingID(bagCreatedEvent.BookingId);
                if (!passengerToUpdate.Bags.Any(bag => bag.BagId == bagCreatedEvent.BagId)) {
                    passengerToUpdate.Bags.Add(new Bag()
                    {
                        BagId = bagCreatedEvent.BagId,
                        //TODO: might be able to simplify this to just check if any leg has a checked bag
                        CheckedBag = bagCreatedEvent.BagFlightLegs.FirstOrDefault(leg => leg.FlightDepartureStationCode == bagCreatedEvent.StationCode)?.BagType == CHECKED_BAG_TYPE,
                    });
                    //TODO: assuming that we don't need to add the flight legs to the passenger if they already exist, but could ask
                }
                _stateProvider.UpdatePassengerByBookingID(passengerToUpdate);
                return passengerToUpdate;
            } catch (StateProvider.PassengerNotFoundException)  
            {
                //passenger not found, create a new passenger
                Passenger newPassenger = new Passenger()
                {
                    BookingID = bagCreatedEvent.BookingId,
                    Bags = new List<Bag>()
                    {
                        new Bag()
                        {
                            BagId = bagCreatedEvent.BagId,
                            CheckedBag = bagCreatedEvent.BagFlightLegs.FirstOrDefault(leg => leg.FlightDepartureStationCode == bagCreatedEvent.StationCode)?.BagType == CHECKED_BAG_TYPE,
                        }
                    },
                    FlightLegs = bagCreatedEvent.FlightLegs.Select(leg => new Flight()
                    {
                        FlightNumber = leg.FlightNumber,
                        CarrierCode = leg.FlightCarrierCode,
                        DepartureDateLocal = leg.FlightDepartureDateLocal,
                        Market = Flight.FlightMarket.Domestic,  //TODO: Replace with actual logic when we have access to the flight market
                    }).ToList()
                };
                _stateProvider.AddPassenger(newPassenger);
                return newPassenger;
            }
        }

        public virtual Passenger UpdatePassengerFromBagItineraryChanged(BagItineraryChanged itineraryChangedEvent)
        {
            Passenger passengerToUpdate;
            try
            {
                passengerToUpdate = _stateProvider.GetPassengerByBookingID(itineraryChangedEvent.BookingId);
                //TODO: double-check that MasterBagId is equivalent to BagId
                if (!passengerToUpdate.Bags.Any(bag => bag.BagId == itineraryChangedEvent.MasterBagId))
                {
                    //bag not in record, add it
                    passengerToUpdate.Bags.Add(new Bag()
                    {
                        BagId = itineraryChangedEvent.MasterBagId,
                        //TODO: may need to add some check using station if it gets added to this event later
                        CheckedBag = itineraryChangedEvent.BagFlightLegs.Any(leg => leg.BagType == CHECKED_BAG_TYPE),
                    });
                } else
                {
                    //bag in record, update it
                    passengerToUpdate.Bags.First(bag => bag.BagId == itineraryChangedEvent.MasterBagId).CheckedBag = itineraryChangedEvent.BagFlightLegs.Any(leg => leg.BagType == CHECKED_BAG_TYPE);
                }
                //TODO: Not sure about this one
                //update flight legs to match new itinerary
                passengerToUpdate.FlightLegs = itineraryChangedEvent.FlightLegs.Select(leg => new Flight()
                {
                    FlightNumber = leg.FlightNumber,
                    CarrierCode = leg.FlightCarrierCode,
                    DepartureDateLocal = leg.FlightDepartureDateLocal,
                    Market = Flight.FlightMarket.Domestic,  //TODO: Replace with actual logic when we have access to the flight market
                }).ToList();
                _stateProvider.UpdatePassengerByBookingID(passengerToUpdate);
                return passengerToUpdate;
            }
            catch (StateProvider.PassengerNotFoundException)
            {
                //passenger not found, create a new passenger
                passengerToUpdate = new Passenger()
                {
                    BookingID = itineraryChangedEvent.BookingId,
                    Bags = new List<Bag>()
                    {
                        new Bag()
                        {
                            BagId = itineraryChangedEvent.MasterBagId,
                            //TODO: may need to add some check using station if it gets added to this event later
                            CheckedBag = itineraryChangedEvent.BagFlightLegs.Any(leg => leg.BagType == CHECKED_BAG_TYPE),
                        }
                    },
                    FlightLegs = itineraryChangedEvent.FlightLegs.Select(leg => new Flight()
                    {
                        FlightNumber = leg.FlightNumber,
                        CarrierCode = leg.FlightCarrierCode,
                        DepartureDateLocal = leg.FlightDepartureDateLocal,
                        Market = Flight.FlightMarket.Domestic,  //TODO: Replace with actual logic when we have access to the flight market
                    }).ToList()
                };
                _stateProvider.AddPassenger(passengerToUpdate);
                return passengerToUpdate;
            }
        }

        public virtual Passenger? UpdatePassengerFromBagPropertyChanged(BagPropertyChanged propertyChangedEvent)
        {
            Passenger passengerToUpdate;
            try
            {
                passengerToUpdate = _stateProvider.GetPassengerByBagID(propertyChangedEvent.MasterBagId);
                //TODO: double-check that MasterBagId is equivalent to BagId
                if (!passengerToUpdate.Bags.Any(bag => bag.BagId == propertyChangedEvent.MasterBagId))
                {
                    //bag not in record, add it
                    passengerToUpdate.Bags.Add(new Bag()
                    {
                        BagId = propertyChangedEvent.MasterBagId,
                        //TODO: modify stuff based on the properties changed
                        //CheckedBag = propertyChangedEvent.BagFlightLegs.Any(leg => leg.BagType == "CheckedBag"),
                    });
                }
                else
                {
                    //bag in record, update it
                    //TODO: modify stuff based on the properties changed
                    //passengerToUpdate.Bags.First(bag => bag.BagId == propertyChangedEvent.MasterBagId).CheckedBag = propertyChangedEvent.BagFlightLegs.Any(leg => leg.BagType == "CheckedBag");
                }
                _stateProvider.UpdatePassengerByBagID(passengerToUpdate, propertyChangedEvent.MasterBagId);
                return passengerToUpdate;
            }
            catch (StateProvider.PassengerNotFoundException)
            {
                //passenger not found, create a new passenger

                //No bookingID in this event, so we can't really create a new passenger
                /*passengerToUpdate = new Passenger()
                {
                    BookingID = propertyChangedEvent.BookingId,
                    Bags = new List<Bag>()
                    {
                        new Bag()
                        {
                            BagId = propertyChangedEvent.MasterBagId,
                            CheckedBag = propertyChangedEvent.BagFlightLegs.FirstOrDefault(leg => leg.FlightDepartureStationCode == propertyChangedEvent.StationCode)?.BagType == "Checked",
                        }
                    },
                    FlightLegs = propertyChangedEvent.FlightLegs.Select(leg => new Flight()
                    {
                        FlightNumber = leg.FlightNumber,
                        CarrierCode = leg.FlightCarrierCode,
                        DepartureDate = leg.FlightDepartureDateLocal,
                        Market = Flight.FlightMarket.Domestic,  //TODO: Replace with actual logic when we have access to the flight market
                    }).ToList()
                };*/
                return null;
            }
        }
    }
}
