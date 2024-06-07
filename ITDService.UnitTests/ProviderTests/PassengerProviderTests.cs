using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrockSolutions.DapperWrapper;
using BrockSolutions.ITDService.Data;
//using BrockSolutions.ITDService.Events;
using BrockSolutions.ITDService.Options;
using BrockSolutions.ITDService.Providers;
using BrockSolutions.SmartSuite.Events;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace BrockSolutions.ITDService.UnitTests.ProviderTests
{
    public class PassengerProviderTests
    {
        [Fact]
        public void checkedBagCreated_addPassengerWithCheckedBag()
        {
            BagCreated bagCreated = new BagCreated()
            {
                BookingId = 5,
                BagId = 2,
                StationCode = "YYZ",
            };
            bagCreated.BagFlightLegs.Add(new BagFlightLeg()
            {
                FlightDepartureStationCode = "YYZ",
                BagType = "Checked"
            });
            bagCreated.FlightLegs.Add(new FlightLeg()
            {
                FlightDepartureStationCode = "YYZ",
                FlightArrivalStationCode = "YVR",
                FlightCarrierCode = "AC",
                FlightDepartureDateLocal = new DateTime(2022, 1, 1).Ticks,
                FlightNumber = "777",
            });

            PassengerTestHarness passengerTestHarness = (PassengerTestHarness) CreatePassengerProvider();
            passengerTestHarness.UpdatePassengerFromBagCreated(bagCreated);

            Passenger expectedPassenger = new Passenger()
            {
                BookingID = 5,
                Bags = new List<Bag>()
                {
                    new Bag()
                    {
                        BagId = 2,
                        CheckedBag = true
                    }
                },
                FlightLegs = new List<Flight>()
                {
                    new Flight()
                    {
                        FlightNumber = "777",
                        CarrierCode = "AC",
                        DepartureDateLocal = new DateTime(2022, 1, 1).Ticks,
                        Market = Flight.FlightMarket.Domestic   //TODO: update this when we have access to the flight market
                    }
                }
            };

            List<Passenger> actualPassengers = passengerTestHarness.GetTestPassengers();
            Assert.Single(actualPassengers);
            Assert.Equal(expectedPassenger, actualPassengers[0]);
        }

        [Fact]
        public void uncheckedBagCreated_addPassengerWithoutCheckedBag()
        {
            BagCreated bagCreated = new BagCreated
            {
                BookingId = 5,
                BagId = 2,
                StationCode = "YYZ",
            };
            bagCreated.BagFlightLegs.Add(new BagFlightLeg()
            {
                FlightDepartureStationCode = "YYZ",
                BagType = "MobilityAid"
            });
            bagCreated.FlightLegs.Add(new FlightLeg()
            {
                FlightDepartureStationCode = "YYZ",
                FlightArrivalStationCode = "YVR",
                FlightCarrierCode = "AC",
                FlightDepartureDateLocal = new DateTime(2022, 1, 1).Ticks,
                FlightNumber = "777",
            });

            PassengerTestHarness passengerTestHarness = (PassengerTestHarness)CreatePassengerProvider();
            passengerTestHarness.UpdatePassengerFromBagCreated(bagCreated);

            Passenger expectedPassenger = new Passenger()
            {
                BookingID = 5,
                Bags = new List<Bag>()
                {
                    new Bag()
                    {
                        BagId = 2,
                        CheckedBag = false
                    }
                },
                FlightLegs = new List<Flight>()
                {
                    new Flight()
                    {
                        FlightNumber = "777",
                        CarrierCode = "AC",
                        DepartureDateLocal = new DateTime(2022, 1, 1).Ticks,
                        Market = Flight.FlightMarket.Domestic   //TODO: update this when we have access to the flight market
                    }
                }
            };

            List<Passenger> actualPassengers = passengerTestHarness.GetTestPassengers();
            Assert.Single(actualPassengers);
            Assert.Equal(expectedPassenger, actualPassengers[0]);
        }

        [Fact]
        public void checkedBagCreatedForExistingPassenger_addCheckedBagToPassenger()
        {
            PassengerTestHarness passengerTestHarness = (PassengerTestHarness)CreatePassengerProvider();

            Passenger existingPassenger = new Passenger()
            {
                BookingID = 5,
                Bags = new List<Bag>()
                {
                    new Bag()
                    {
                        BagId = 1,
                        CheckedBag = false
                    }
                }
            };
            passengerTestHarness.AddTestPassenger(existingPassenger);

            BagCreated bagCreated = new BagCreated
            {
                BookingId = 5,
                BagId = 2,
                StationCode = "YYZ",
            };
            bagCreated.BagFlightLegs.Add(new BagFlightLeg
            {
                FlightDepartureStationCode = "YYZ",
                BagType = "Checked"
            });

            
            passengerTestHarness.UpdatePassengerFromBagCreated(bagCreated);

            Passenger expectedPassenger = new Passenger()
            {
                BookingID = 5,
                Bags = new List<Bag>()
                {
                    new Bag()
                    {
                        BagId = 1,
                        CheckedBag = false
                    },
                    new Bag()
                    {
                        BagId = 2,
                        CheckedBag = true
                    }
                }
            };

            List<Passenger> actualPassengers = passengerTestHarness.GetTestPassengers();
            Assert.Single(actualPassengers);
            Assert.Equal(expectedPassenger, actualPassengers[0]);
        }

        [Fact]
        public void uncheckedBagCreatedForExistingPassenger_addUncheckedBagToPassenger()
        {
            PassengerTestHarness passengerTestHarness = (PassengerTestHarness)CreatePassengerProvider();

            Passenger existingPassenger = new Passenger()
            {
                BookingID = 5,
                Bags = new List<Bag>()
                {
                    new Bag()
                    {
                        BagId = 1,
                        CheckedBag = false
                    }
                }
            };
            passengerTestHarness.AddTestPassenger(existingPassenger);

            BagCreated bagCreated = new BagCreated
            {
                BookingId = 5,
                BagId = 2,
                StationCode = "YYZ",
            };
            bagCreated.BagFlightLegs.Add(new BagFlightLeg
            {
                FlightDepartureStationCode = "YYZ",
                BagType = "MobilityAid"
            });


            passengerTestHarness.UpdatePassengerFromBagCreated(bagCreated);

            Passenger expectedPassenger = new Passenger()
            {
                BookingID = 5,
                Bags = new List<Bag>()
                {
                    new Bag()
                    {
                        BagId = 1,
                        CheckedBag = false
                    },
                    new Bag()
                    {
                        BagId = 2,
                        CheckedBag = false
                    }
                }
            };

            List<Passenger> actualPassengers = passengerTestHarness.GetTestPassengers();
            Assert.Single(actualPassengers);
            Assert.Equal(expectedPassenger, actualPassengers[0]);
        }

        [Fact]
        public void bagItineraryChangedForPassengerWithMultipleBags_updateCorrectBag()
        {
            PassengerTestHarness passengerTestHarness = (PassengerTestHarness)CreatePassengerProvider();

            Passenger existingPassenger = new Passenger()
            {
                BookingID = 5,
                Bags = new List<Bag>()
                {
                    new Bag()
                    {
                        BagId = 1,
                        CheckedBag = false
                    },
                    new Bag()
                    {
                        BagId = 2,
                        CheckedBag = false
                    }
                },
                FlightLegs = new List<Flight>()
                {
                    new Flight()
                    {
                        FlightNumber = "666",
                        CarrierCode = "VA",
                        DepartureDateLocal = new DateTime(2021, 1, 1).Ticks,
                        Market = Flight.FlightMarket.International   //TODO: update this when we have access to the flight market
                    }
                }
            };
            passengerTestHarness.AddTestPassenger(existingPassenger);

            BagItineraryChanged bagItineraryChanged = new BagItineraryChanged
            {
                BookingId = 5,
                MasterBagId = 2,
            };
            bagItineraryChanged.BagFlightLegs.Add(new BagFlightLeg
            {
                BagType = "Checked"
            });
            bagItineraryChanged.FlightLegs.Add(new FlightLeg()
            {
                FlightDepartureStationCode = "YYZ",
                FlightArrivalStationCode = "YVR",
                FlightCarrierCode = "AC",
                FlightDepartureDateLocal = new DateTime(2022, 1, 1).Ticks,
                FlightNumber = "777",
            });

            passengerTestHarness.UpdatePassengerFromBagItineraryChanged(bagItineraryChanged);

            Passenger expectedPassenger = new Passenger()
            {
                BookingID = 5,
                Bags = new List<Bag>()
                {
                    new Bag()
                    {
                        BagId = 1,
                        CheckedBag = false
                    },
                    new Bag()
                    {
                        BagId = 2,
                        CheckedBag = true
                    }
                },
                FlightLegs = new List<Flight>()
                {
                    new Flight()
                    {
                        FlightNumber = "777",
                        CarrierCode = "AC",
                        DepartureDateLocal = new DateTime(2022, 1, 1).Ticks,
                        Market = Flight.FlightMarket.Domestic   //TODO: update this when we have access to the flight market
                    }
                }
            };

            List<Passenger> actualPassengers = passengerTestHarness.GetTestPassengers();
            Assert.Single(actualPassengers);
            Assert.Equal(expectedPassenger, actualPassengers[0]);
        }

        [Fact]
        public void bagItineraryChangedWhenMultiplePassengers_updateCorrectPassenger()
        {
            PassengerTestHarness passengerTestHarness = (PassengerTestHarness)CreatePassengerProvider();

            Passenger passengerToChange = new Passenger()
            {
                BookingID = 5,
                Bags = new List<Bag>()
                {
                    new Bag()
                    {
                        BagId = 1,
                        CheckedBag = false
                    }
                },
                FlightLegs = new List<Flight>()
                {
                    new Flight()
                    {
                        FlightNumber = "666",
                        CarrierCode = "VA",
                        DepartureDateLocal = new DateTime(2021, 1, 1).Ticks,
                        Market = Flight.FlightMarket.International   //TODO: update this when we have access to the flight market
                    }
                }
            };
            passengerTestHarness.AddTestPassenger(passengerToChange);

            Passenger unchangedPassenger = new Passenger()
            {
                BookingID = 6,
                Bags = new List<Bag>()
                {
                    new Bag()
                    {
                        BagId = 2,
                        CheckedBag = false
                    }
                },
                FlightLegs = new List<Flight>()
                {
                    new Flight()
                    {
                        FlightNumber = "888",
                        CarrierCode = "DA",
                        DepartureDateLocal = new DateTime(2022, 1, 1).Ticks,
                        Market = Flight.FlightMarket.Transborder   //TODO: update this when we have access to the flight market
                    }
                }
            };
            passengerTestHarness.AddTestPassenger(unchangedPassenger);

            BagItineraryChanged bagItineraryChanged = new BagItineraryChanged
            {
                BookingId = 5,
                MasterBagId = 1,
            };
            bagItineraryChanged.BagFlightLegs.Add(new BagFlightLeg
            {
                BagType = "Checked"
            });
            bagItineraryChanged.FlightLegs.Add(new FlightLeg()
            {
                FlightDepartureStationCode = "YYZ",
                FlightArrivalStationCode = "YVR",
                FlightCarrierCode = "AC",
                FlightDepartureDateLocal = new DateTime(2022, 1, 1).Ticks,
                FlightNumber = "777",
            });

            passengerTestHarness.UpdatePassengerFromBagItineraryChanged(bagItineraryChanged);

            Passenger expectedPassenger = new Passenger()
            {
                BookingID = 5,
                Bags = new List<Bag>()
                {
                    new Bag()
                    {
                        BagId = 1,
                        CheckedBag = true
                    }
                },
                FlightLegs = new List<Flight>()
                {
                    new Flight()
                    {
                        FlightNumber = "777",
                        CarrierCode = "AC",
                        DepartureDateLocal = new DateTime(2022, 1, 1).Ticks,
                        Market = Flight.FlightMarket.Domestic   //TODO: update this when we have access to the flight market
                    }
                }
            };

            List<Passenger> actualPassengers = passengerTestHarness.GetTestPassengers();
            Assert.Equal(2, actualPassengers.Count);
            Assert.Contains(expectedPassenger, actualPassengers);
            Assert.Contains(unchangedPassenger, actualPassengers);
        }

        [Fact]
        public void bagItineraryChangedToCheckedForExistingBag_updateBag()
        {
            PassengerTestHarness passengerTestHarness = (PassengerTestHarness)CreatePassengerProvider();

            Passenger existingPassenger = new Passenger()
            {
                BookingID = 5,
                Bags = new List<Bag>()
                {
                    new Bag()
                    {
                        BagId = 1,
                        CheckedBag = false
                    }
                },
                FlightLegs = new List<Flight>()
                {
                    new Flight()
                    {
                        FlightNumber = "666",
                        CarrierCode = "VA",
                        DepartureDateLocal = new DateTime(2021, 1, 1).Ticks,
                        Market = Flight.FlightMarket.International   //TODO: update this when we have access to the flight market
                    }
                }
            };
            passengerTestHarness.AddTestPassenger(existingPassenger);

            BagItineraryChanged bagItineraryChanged = new BagItineraryChanged
            {
                BookingId = 5,
                MasterBagId = 1,
            };
            bagItineraryChanged.BagFlightLegs.Add(new BagFlightLeg
            {
                BagType = "Checked"
            });
            bagItineraryChanged.FlightLegs.Add(new FlightLeg()
            {
                FlightDepartureStationCode = "YYZ",
                FlightArrivalStationCode = "YVR",
                FlightCarrierCode = "AC",
                FlightDepartureDateLocal = new DateTime(2022, 1, 1).Ticks,
                FlightNumber = "777",
            });

            passengerTestHarness.UpdatePassengerFromBagItineraryChanged(bagItineraryChanged);

            Passenger expectedPassenger = new Passenger()
            {
                BookingID = 5,
                Bags = new List<Bag>()
                {
                    new Bag()
                    {
                        BagId = 1,
                        CheckedBag = true
                    }
                },
                FlightLegs = new List<Flight>()
                {
                    new Flight()
                    {
                        FlightNumber = "777",
                        CarrierCode = "AC",
                        DepartureDateLocal = new DateTime(2022, 1, 1).Ticks,
                        Market = Flight.FlightMarket.Domestic   //TODO: update this when we have access to the flight market
                    }
                }
            };

            List<Passenger> actualPassengers = passengerTestHarness.GetTestPassengers();
            Assert.Single(actualPassengers);
            Assert.Equal(expectedPassenger, actualPassengers[0]);
        }

        [Fact]
        public void bagItineraryChangedToUncheckedForExistingBag_updateBag()
        {
            PassengerTestHarness passengerTestHarness = (PassengerTestHarness)CreatePassengerProvider();

            Passenger existingPassenger = new Passenger()
            {
                BookingID = 5,
                Bags = new List<Bag>()
                {
                    new Bag()
                    {
                        BagId = 1,
                        CheckedBag = true
                    }
                },
                FlightLegs = new List<Flight>()
                {
                    new Flight()
                    {
                        FlightNumber = "666",
                        CarrierCode = "VA",
                        DepartureDateLocal = new DateTime(2021, 1, 1).Ticks,
                        Market = Flight.FlightMarket.International   //TODO: update this when we have access to the flight market
                    }
                }
            };
            passengerTestHarness.AddTestPassenger(existingPassenger);

            BagItineraryChanged bagItineraryChanged = new BagItineraryChanged
            {
                BookingId = 5,
                MasterBagId = 1,
            };
            bagItineraryChanged.BagFlightLegs.Add(new BagFlightLeg
            {
                BagType = "MobilityAid"
            });
            bagItineraryChanged.FlightLegs.Add(new FlightLeg()
            {
                FlightDepartureStationCode = "YYZ",
                FlightArrivalStationCode = "YVR",
                FlightCarrierCode = "AC",
                FlightDepartureDateLocal = new DateTime(2022, 1, 1).Ticks,
                FlightNumber = "777",
            });

            passengerTestHarness.UpdatePassengerFromBagItineraryChanged(bagItineraryChanged);

            Passenger expectedPassenger = new Passenger()
            {
                BookingID = 5,
                Bags = new List<Bag>()
                {
                    new Bag()
                    {
                        BagId = 1,
                        CheckedBag = false
                    }
                },
                FlightLegs = new List<Flight>()
                {
                    new Flight()
                    {
                        FlightNumber = "777",
                        CarrierCode = "AC",
                        DepartureDateLocal = new DateTime(2022, 1, 1).Ticks,
                        Market = Flight.FlightMarket.Domestic   //TODO: update this when we have access to the flight market
                    }
                }
            };

            List<Passenger> actualPassengers = passengerTestHarness.GetTestPassengers();
            Assert.Single(actualPassengers);
            Assert.Equal(expectedPassenger, actualPassengers[0]);
        }

        [Fact]
        public void bagItineraryChangedToCheckedForNewBag_addCheckedBag()
        {
            PassengerTestHarness passengerTestHarness = (PassengerTestHarness)CreatePassengerProvider();

            BagItineraryChanged bagItineraryChanged = new BagItineraryChanged
            {
                BookingId = 5,
                MasterBagId = 1,
            };
            bagItineraryChanged.BagFlightLegs.Add(new BagFlightLeg
            {
                BagType = "Checked"
            });
            bagItineraryChanged.FlightLegs.Add(new FlightLeg()
            {
                FlightDepartureStationCode = "YYZ",
                FlightArrivalStationCode = "YVR",
                FlightCarrierCode = "AC",
                FlightDepartureDateLocal = new DateTime(2022, 1, 1).Ticks,
                FlightNumber = "777",
            });

            passengerTestHarness.UpdatePassengerFromBagItineraryChanged(bagItineraryChanged);

            Passenger expectedPassenger = new Passenger()
            {
                BookingID = 5,
                Bags = new List<Bag>()
                {
                    new Bag()
                    {
                        BagId = 1,
                        CheckedBag = true
                    }
                },
                FlightLegs = new List<Flight>()
                {
                    new Flight()
                    {
                        FlightNumber = "777",
                        CarrierCode = "AC",
                        DepartureDateLocal = new DateTime(2022, 1, 1).Ticks,
                        Market = Flight.FlightMarket.Domestic   //TODO: update this when we have access to the flight market
                    }
                }
            };

            List<Passenger> actualPassengers = passengerTestHarness.GetTestPassengers();
            Assert.Single(actualPassengers);
            Assert.Equal(expectedPassenger, actualPassengers[0]);
        }

        [Fact]
        public void bagItineraryChangedToUncheckedForNewBag_addUncheckedBag()
        {
            PassengerTestHarness passengerTestHarness = (PassengerTestHarness)CreatePassengerProvider();

            BagItineraryChanged bagItineraryChanged = new BagItineraryChanged
            {
                BookingId = 5,
                MasterBagId = 1,
            };
            bagItineraryChanged.BagFlightLegs.Add(new BagFlightLeg
            {
                BagType = "MobilityAid"
            });
            bagItineraryChanged.FlightLegs.Add(new FlightLeg()
            {
                FlightDepartureStationCode = "YYZ",
                FlightArrivalStationCode = "YVR",
                FlightCarrierCode = "AC",
                FlightDepartureDateLocal = new DateTime(2022, 1, 1).Ticks,
                FlightNumber = "777",
            });

            passengerTestHarness.UpdatePassengerFromBagItineraryChanged(bagItineraryChanged);

            Passenger expectedPassenger = new Passenger()
            {
                BookingID = 5,
                Bags = new List<Bag>()
                {
                    new Bag()
                    {
                        BagId = 1,
                        CheckedBag = false
                    }
                },
                FlightLegs = new List<Flight>()
                {
                    new Flight()
                    {
                        FlightNumber = "777",
                        CarrierCode = "AC",
                        DepartureDateLocal = new DateTime(2022, 1, 1).Ticks,
                        Market = Flight.FlightMarket.Domestic   //TODO: update this when we have access to the flight market
                    }
                }
            };

            List<Passenger> actualPassengers = passengerTestHarness.GetTestPassengers();
            Assert.Single(actualPassengers);
            Assert.Equal(expectedPassenger, actualPassengers[0]);
        }

        public static PassengerProvider CreatePassengerProvider()
        {
            Mock<ILogger<PassengerProvider>> logger = new Mock<ILogger<PassengerProvider>>();
            Mock<IDapperQueryExecutor> queryExecutor = new Mock<IDapperQueryExecutor>();
            Mock<IOptionsMonitor<ITDServiceParameters>> parameters = new Mock<IOptionsMonitor<ITDServiceParameters>>();
            return new PassengerTestHarness(logger.Object, queryExecutor.Object, parameters.Object);
        }



        public class PassengerTestHarness : PassengerProvider
        {

            public PassengerTestHarness(ILogger<PassengerProvider> logger, IDapperQueryExecutor queryExecutor, IOptionsMonitor<ITDServiceParameters> parameters) : base(logger, queryExecutor, parameters)
            {
                _stateProvider = new TestStateProvider();
            }

            public void AddTestPassenger(Passenger passenger)
            {
                ((TestStateProvider) _stateProvider).AddPassenger(passenger);
            }

            public List<Passenger> GetTestPassengers()
            {
                return ((TestStateProvider)_stateProvider).FakeDatabase;
            }

            /*public override Passenger? GetPassengerByBookingID(long passengerID)
            {
                return _fakeDatabase.FirstOrDefault(p => p.BookingID == passengerID);
            }

            public override bool AddPassengerToDatabase(Passenger passenger)
            {
                _fakeDatabase.Add(passenger);
                return true;
            }*/
        }


        public class TestStateProvider : IStateProvider
        {
            public List<Passenger> FakeDatabase = new List<Passenger>();
            public TestStateProvider() : base()
            {
            }

            public List<Passenger> AddPassenger(Passenger passenger)
            {
                FakeDatabase.Add(passenger);
                return FakeDatabase;
            }

            public Passenger GetPassengerByBookingID(long bookingID)
            {
                Passenger? passenger = FakeDatabase.FirstOrDefault(passenger => passenger.BookingID == bookingID);
                if (passenger == null)
                {
                    throw new IStateProvider.PassengerNotFoundException("Could not find passenger with booking ID " + bookingID);
                }
                return passenger;
            }

            public Passenger GetPassengerByBagID(long bagID)
            {
                Passenger? passenger = FakeDatabase.FirstOrDefault(passenger => passenger.Bags.Any(b => b.BagId == bagID));
                if (passenger == null)
                {
                    throw new IStateProvider.PassengerNotFoundException("Could not find passenger with bag ID " + bagID);
                }
                return passenger;
            }

            public void UpdatePassengerByBookingID(Passenger passengerToUpdate)
            {
                Passenger? existingPassenger = FakeDatabase.FirstOrDefault(passenger => passenger.BookingID == passenger.BookingID);
                if (existingPassenger != null)
                {
                    FakeDatabase.Remove(existingPassenger);
                    FakeDatabase.Add(passengerToUpdate);
                } else
                {
                    throw new IStateProvider.PassengerNotFoundException("Could not find passenger with booking ID " + passengerToUpdate.BookingID);
                }
            }

            public void UpdatePassengerByBagID(Passenger passengerToUpdate, long bagID)
            {
                Passenger? existingPassenger = FakeDatabase.FirstOrDefault(passenger => passenger.Bags.Any(bag => bag.BagId == bagID));
                if (existingPassenger != null)
                {
                    FakeDatabase.Remove(existingPassenger);
                    FakeDatabase.Add(existingPassenger);
                } else
                {
                    throw new IStateProvider.PassengerNotFoundException("Could not find passenger with bag ID " + bagID);
                }
            }

            public void DeletePassengerByBookingID(long bookingID)
            {
                Passenger? existingPassenger = FakeDatabase.FirstOrDefault(passenger => passenger.BookingID == bookingID);
                if (existingPassenger != null)
                {
                    FakeDatabase.Remove(existingPassenger);
                } else
                {
                    throw new IStateProvider.PassengerNotFoundException("Could not find passenger with booking ID " + bookingID);
                }
            }
        }
    }
}
