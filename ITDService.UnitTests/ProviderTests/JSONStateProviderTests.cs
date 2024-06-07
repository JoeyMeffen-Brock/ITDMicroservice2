using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrockSolutions.ITDService.Data;
using BrockSolutions.ITDService.Providers;
using Newtonsoft.Json;


namespace BrockSolutions.ITDService.UnitTests.ProviderTests
{
    public class JSONStateProviderTests
    {

        [Fact]
        public void addPassengerToFile_canRetrievePassenger()
        {
            try
            {
                Passenger testPassenger = new Passenger()
                {
                    BookingID = 10,
                    Bags = new List<Bag> {
                        new Bag { BagId = 1, CheckedBag = true },
                        new Bag { BagId = 2, CheckedBag = false },
                    },
                    FlightLegs = new List<Flight> {
                        new Flight { DepartureDateLocal = 1000, FlightNumber = "1234", CarrierCode = "AC", Market = Flight.FlightMarket.Domestic },
                        new Flight { DepartureDateLocal = 5000, FlightNumber = "5678", CarrierCode = "VA", Market = Flight.FlightMarket.International },
                    },
                    MarkedAsIneligible = false,
                    HasBoardedBSM = true,
                    ScannedAtSmartGate = true,
                    HasBDXMessage = false,
                };
                JSONStateProvider jsonStateProvider = new JSONStateProvider();
                jsonStateProvider.AddPassenger(testPassenger);
                string resultJson = File.ReadAllText(JSONStateProvider.FILE_NAME);
                List<Passenger> resultList = JsonConvert.DeserializeObject<List<Passenger>>(resultJson);
                Assert.Single(resultList);
                Assert.Equal(testPassenger, resultList[0]);
            } finally
            {
                File.Delete(JSONStateProvider.FILE_NAME);
            }
        }

        [Fact]
        public void getPassengerByBookingID_returnsCorrectPassenger()
        {
            try
            {
                Passenger testPassenger1 = new Passenger()
                {
                    BookingID = 10,
                    Bags = new List<Bag> {
                        new Bag { BagId = 1, CheckedBag = true },
                        new Bag { BagId = 2, CheckedBag = false },
                    },
                    FlightLegs = new List<Flight> {
                        new Flight { DepartureDateLocal = 1000, FlightNumber = "1234", CarrierCode = "AC", Market = Flight.FlightMarket.Domestic },
                        new Flight { DepartureDateLocal = 5000, FlightNumber = "5678", CarrierCode = "VA", Market = Flight.FlightMarket.International },
                    },
                    MarkedAsIneligible = false,
                    HasBoardedBSM = true,
                    ScannedAtSmartGate = true,
                    HasBDXMessage = false,
                };

                Passenger testPassenger2 = new Passenger()
                {
                    BookingID = 20,
                    Bags = new List<Bag> {
                        new Bag { BagId = 3, CheckedBag = true },
                        new Bag { BagId = 4, CheckedBag = false },
                    },
                    FlightLegs = new List<Flight> {
                        new Flight { DepartureDateLocal = 1000, FlightNumber = "1234", CarrierCode = "AC", Market = Flight.FlightMarket.Domestic },
                        new Flight { DepartureDateLocal = 5000, FlightNumber = "5678", CarrierCode = "VA", Market = Flight.FlightMarket.International },
                    },
                    MarkedAsIneligible = false,
                    HasBoardedBSM = false,
                    ScannedAtSmartGate = false,
                    HasBDXMessage = false,
                };

                File.WriteAllText(JSONStateProvider.FILE_NAME, JsonConvert.SerializeObject(new List<Passenger> { testPassenger1, testPassenger2 }));

                JSONStateProvider jsonStateProvider = new JSONStateProvider();
                Passenger outputPassenger = jsonStateProvider.GetPassengerByBookingID(10);
                Assert.Equal(testPassenger1, outputPassenger);
            }
            finally
            {
                File.Delete(JSONStateProvider.FILE_NAME);
            }
        }

        [Fact]
        public void getPassengerByBookingIDWithoutFile_throwCouldNotReadException()
        {
            try
            {
                JSONStateProvider jsonStateProvider = new JSONStateProvider();
                try
                {
                    Passenger outputPassenger = jsonStateProvider.GetPassengerByBookingID(10);
                    Assert.Fail("failed to trigger exception");
                }
                catch (FileNotFoundException e)
                {

                } 
            }
            finally
            {
                File.Delete(JSONStateProvider.FILE_NAME);
            }
        }

        [Fact]
        public void getPassengerByBookingIDWithBadFile_throwJsonReaderException()
        {
            try
            {
                File.WriteAllText(JSONStateProvider.FILE_NAME, "Bad data for the JSONStateProvider file");
                JSONStateProvider jsonStateProvider = new JSONStateProvider();
                try
                {
                    Passenger outputPassenger = jsonStateProvider.GetPassengerByBookingID(55);
                    Assert.Fail("failed to trigger exception");
                }
                catch (JsonReaderException e)
                {

                }
            }
            finally
            {
                File.Delete(JSONStateProvider.FILE_NAME);
            }
        }

        [Fact]
        public void getNonexistentPassengerByBookingID_throwNotFoundException()
        {
            try
            {
                Passenger testPassenger = new Passenger()
                {
                    BookingID = 10,
                    Bags = new List<Bag> {
                        new Bag { BagId = 1, CheckedBag = true },
                        new Bag { BagId = 2, CheckedBag = false },
                    },
                    FlightLegs = new List<Flight> {
                        new Flight { DepartureDateLocal = 1000, FlightNumber = "1234", CarrierCode = "AC", Market = Flight.FlightMarket.Domestic },
                        new Flight { DepartureDateLocal = 5000, FlightNumber = "5678", CarrierCode = "VA", Market = Flight.FlightMarket.International },
                    },
                    MarkedAsIneligible = false,
                    HasBoardedBSM = true,
                    ScannedAtSmartGate = true,
                    HasBDXMessage = false,
                };
                File.WriteAllText(JSONStateProvider.FILE_NAME, JsonConvert.SerializeObject(new List<Passenger> { testPassenger }));
                JSONStateProvider jsonStateProvider = new JSONStateProvider();
                try
                {
                    Passenger outputPassenger = jsonStateProvider.GetPassengerByBookingID(55);
                    Assert.Fail("failed to trigger exception");
                } catch (StateProvider.PassengerNotFoundException e)
                {
                    Assert.Equal("Could not find passenger with booking ID 55", e.Message);
                }
            }
            finally
            {
                File.Delete(JSONStateProvider.FILE_NAME);
            }
        }

        [Fact]
        public void getPassengerByBagID_returnsCorrectPassenger()
        {
            try
            {
                Passenger testPassenger1 = new Passenger()
                {
                    BookingID = 10,
                    Bags = new List<Bag> {
                        new Bag { BagId = 1, CheckedBag = true },
                        new Bag { BagId = 2, CheckedBag = false },
                    },
                    FlightLegs = new List<Flight> {
                        new Flight { DepartureDateLocal = 1000, FlightNumber = "1234", CarrierCode = "AC", Market = Flight.FlightMarket.Domestic },
                        new Flight { DepartureDateLocal = 5000, FlightNumber = "5678", CarrierCode = "VA", Market = Flight.FlightMarket.International },
                    },
                    MarkedAsIneligible = false,
                    HasBoardedBSM = true,
                    ScannedAtSmartGate = true,
                    HasBDXMessage = false,
                };

                Passenger testPassenger2 = new Passenger()
                {
                    BookingID = 20,
                    Bags = new List<Bag> {
                        new Bag { BagId = 3, CheckedBag = true },
                        new Bag { BagId = 4, CheckedBag = false },
                    },
                    FlightLegs = new List<Flight> {
                        new Flight { DepartureDateLocal = 1000, FlightNumber = "1234", CarrierCode = "AC", Market = Flight.FlightMarket.Domestic },
                        new Flight { DepartureDateLocal = 5000, FlightNumber = "5678", CarrierCode = "VA", Market = Flight.FlightMarket.International },
                    },
                    MarkedAsIneligible = false,
                    HasBoardedBSM = false,
                    ScannedAtSmartGate = false,
                    HasBDXMessage = false,
                };

                File.WriteAllText(JSONStateProvider.FILE_NAME, JsonConvert.SerializeObject(new List<Passenger> { testPassenger1, testPassenger2 }));

                JSONStateProvider jsonStateProvider = new JSONStateProvider();
                Passenger outputPassenger = jsonStateProvider.GetPassengerByBagID(3);
                Assert.Equal(testPassenger2, outputPassenger);
            }
            finally
            {
                File.Delete(JSONStateProvider.FILE_NAME);
            }
        }

        [Fact]
        public void getPassengerByBagIDWithoutFile_throwCouldNotReadException()
        {
            try
            {
                JSONStateProvider jsonStateProvider = new JSONStateProvider();
                try
                {
                    Passenger outputPassenger = jsonStateProvider.GetPassengerByBagID(1);
                    Assert.Fail("failed to trigger exception");
                }
                catch (FileNotFoundException e)
                {

                }
            }
            finally
            {
                File.Delete(JSONStateProvider.FILE_NAME);
            }
        }

        [Fact]
        public void getPassengerByBagIDWithBadFile_throwJsonReaderException()
        {
            try
            {
                File.WriteAllText(JSONStateProvider.FILE_NAME, "Bad data for the JSONStateProvider file");
                JSONStateProvider jsonStateProvider = new JSONStateProvider();
                try
                {
                    Passenger outputPassenger = jsonStateProvider.GetPassengerByBagID(1);
                    Assert.Fail("failed to trigger exception");
                }
                catch (JsonReaderException e)
                {

                }
            }
            finally
            {
                File.Delete(JSONStateProvider.FILE_NAME);
            }
        }

        [Fact]
        public void getNonexistentPassengerByBagID_throwNotFoundException()
        {
            try
            {
                Passenger testPassenger = new Passenger()
                {
                    BookingID = 10,
                    Bags = new List<Bag> {
                        new Bag { BagId = 1, CheckedBag = true },
                        new Bag { BagId = 2, CheckedBag = false },
                    },
                    FlightLegs = new List<Flight> {
                        new Flight { DepartureDateLocal = 1000, FlightNumber = "1234", CarrierCode = "AC", Market = Flight.FlightMarket.Domestic },
                        new Flight { DepartureDateLocal = 5000, FlightNumber = "5678", CarrierCode = "VA", Market = Flight.FlightMarket.International },
                    },
                    MarkedAsIneligible = false,
                    HasBoardedBSM = true,
                    ScannedAtSmartGate = true,
                    HasBDXMessage = false,
                };
                File.WriteAllText(JSONStateProvider.FILE_NAME, JsonConvert.SerializeObject(new List<Passenger> { testPassenger }));
                JSONStateProvider jsonStateProvider = new JSONStateProvider();
                try
                {
                    Passenger outputPassenger = jsonStateProvider.GetPassengerByBagID(5);
                    Assert.Fail("failed to trigger exception");
                }
                catch (StateProvider.PassengerNotFoundException e)
                {
                    Assert.Equal("Could not find passenger with bag ID 5", e.Message);
                }
            }
            finally
            {
                File.Delete(JSONStateProvider.FILE_NAME);
            }
        }

        [Fact]
        public void updatePassengerByBookingID_fileCorrectlyChanged()
        {
            try
            {
                Passenger testPassenger1 = new Passenger()
                {
                    BookingID = 10,
                    Bags = new List<Bag> {
                        new Bag { BagId = 1, CheckedBag = true },
                        new Bag { BagId = 2, CheckedBag = false },
                    },
                    FlightLegs = new List<Flight> {
                        new Flight { DepartureDateLocal = 1000, FlightNumber = "1234", CarrierCode = "AC", Market = Flight.FlightMarket.Domestic },
                        new Flight { DepartureDateLocal = 5000, FlightNumber = "5678", CarrierCode = "VA", Market = Flight.FlightMarket.International },
                    },
                    MarkedAsIneligible = false,
                    HasBoardedBSM = true,
                    ScannedAtSmartGate = true,
                    HasBDXMessage = false,
                };

                Passenger testPassenger2 = new Passenger()
                {
                    BookingID = 20,
                    Bags = new List<Bag> {
                        new Bag { BagId = 3, CheckedBag = true },
                        new Bag { BagId = 4, CheckedBag = false },
                    },
                    FlightLegs = new List<Flight> {
                        new Flight { DepartureDateLocal = 1000, FlightNumber = "1234", CarrierCode = "AC", Market = Flight.FlightMarket.Domestic },
                        new Flight { DepartureDateLocal = 5000, FlightNumber = "5678", CarrierCode = "VA", Market = Flight.FlightMarket.International },
                    },
                    MarkedAsIneligible = false,
                    HasBoardedBSM = false,
                    ScannedAtSmartGate = false,
                    HasBDXMessage = false,
                };

                
                File.WriteAllText(JSONStateProvider.FILE_NAME, JsonConvert.SerializeObject(new List<Passenger> { testPassenger1, testPassenger2 }));

                testPassenger1.MarkedAsIneligible = true;
                testPassenger1.HasBDXMessage = true;

                JSONStateProvider jsonStateProvider = new JSONStateProvider();
                jsonStateProvider.UpdatePassengerByBookingID(testPassenger1);

                string resultJson = File.ReadAllText(JSONStateProvider.FILE_NAME);
                List<Passenger> resultList = JsonConvert.DeserializeObject<List<Passenger>>(resultJson);
                Assert.Equal(2, resultList.Count);
                Assert.Contains(testPassenger1, resultList);
                Assert.Contains(testPassenger2, resultList);
            }
            finally
            {
                File.Delete(JSONStateProvider.FILE_NAME);
            }
        }

        [Fact]
        public void updatePassengerByBookingIDWithBadFile_throwJsonReaderException()
        {
            try
            {
                File.WriteAllText(JSONStateProvider.FILE_NAME, "Bad data for the JSONStateProvider file");
                JSONStateProvider jsonStateProvider = new JSONStateProvider();
                try
                {
                    Passenger outputPassenger = jsonStateProvider.GetPassengerByBookingID(55);
                    Assert.Fail("failed to trigger exception");
                }
                catch (JsonReaderException e)
                {

                }
            }
            finally
            {
                File.Delete(JSONStateProvider.FILE_NAME);
            }
        }

        [Fact]
        public void updateNonexistentPassengerByBookingID_throwNotFoundException()
        {
            try
            {
                Passenger testPassenger = new Passenger()
                {
                    BookingID = 10,
                    Bags = new List<Bag> {
                        new Bag { BagId = 1, CheckedBag = true },
                        new Bag { BagId = 2, CheckedBag = false },
                    },
                    FlightLegs = new List<Flight> {
                        new Flight { DepartureDateLocal = 1000, FlightNumber = "1234", CarrierCode = "AC", Market = Flight.FlightMarket.Domestic },
                        new Flight { DepartureDateLocal = 5000, FlightNumber = "5678", CarrierCode = "VA", Market = Flight.FlightMarket.International },
                    },
                    MarkedAsIneligible = false,
                    HasBoardedBSM = true,
                    ScannedAtSmartGate = true,
                    HasBDXMessage = false,
                };
                JSONStateProvider jsonStateProvider = new JSONStateProvider();
                File.WriteAllText(JSONStateProvider.FILE_NAME, JsonConvert.SerializeObject(new List<Passenger> { testPassenger }));
                testPassenger.BookingID = 55;
                testPassenger.MarkedAsIneligible = true;
                testPassenger.HasBDXMessage = true;
                try
                {
                    jsonStateProvider.UpdatePassengerByBookingID(testPassenger);
                    Assert.Fail();
                } catch (StateProvider.PassengerNotFoundException e)
                {
                    Assert.Equal("Could not find passenger with booking ID 55", e.Message);
                }
            }
            finally
            {
                File.Delete(JSONStateProvider.FILE_NAME);
            }
        }

        [Fact]
        public void updatePassengerByBagID_fileCorrectlyChanged()
        {
            try
            {
                Passenger testPassenger1 = new Passenger()
                {
                    BookingID = 10,
                    Bags = new List<Bag> {
                        new Bag { BagId = 1, CheckedBag = true },
                        new Bag { BagId = 2, CheckedBag = false },
                    },
                    FlightLegs = new List<Flight> {
                        new Flight { DepartureDateLocal = 1000, FlightNumber = "1234", CarrierCode = "AC", Market = Flight.FlightMarket.Domestic },
                        new Flight { DepartureDateLocal = 5000, FlightNumber = "5678", CarrierCode = "VA", Market = Flight.FlightMarket.International },
                    },
                    MarkedAsIneligible = false,
                    HasBoardedBSM = true,
                    ScannedAtSmartGate = true,
                    HasBDXMessage = false,
                };

                Passenger testPassenger2 = new Passenger()
                {
                    BookingID = 20,
                    Bags = new List<Bag> {
                        new Bag { BagId = 3, CheckedBag = true },
                        new Bag { BagId = 4, CheckedBag = false },
                    },
                    FlightLegs = new List<Flight> {
                        new Flight { DepartureDateLocal = 1000, FlightNumber = "1234", CarrierCode = "AC", Market = Flight.FlightMarket.Domestic },
                        new Flight { DepartureDateLocal = 5000, FlightNumber = "5678", CarrierCode = "VA", Market = Flight.FlightMarket.International },
                    },
                    MarkedAsIneligible = false,
                    HasBoardedBSM = false,
                    ScannedAtSmartGate = false,
                    HasBDXMessage = false,
                };


                File.WriteAllText(JSONStateProvider.FILE_NAME, JsonConvert.SerializeObject(new List<Passenger> { testPassenger1, testPassenger2 }));

                testPassenger1.MarkedAsIneligible = true;
                testPassenger1.HasBDXMessage = true;

                JSONStateProvider jsonStateProvider = new JSONStateProvider();
                jsonStateProvider.UpdatePassengerByBagID(testPassenger1, 2);

                string resultJson = File.ReadAllText(JSONStateProvider.FILE_NAME);
                List<Passenger> resultList = JsonConvert.DeserializeObject<List<Passenger>>(resultJson);
                Assert.Equal(2, resultList.Count);
                Assert.Contains(testPassenger1, resultList);
                Assert.Contains(testPassenger2, resultList);
            }
            finally
            {
                File.Delete(JSONStateProvider.FILE_NAME);
            }
        }

        [Fact]
        public void updatePassengerByBagIDWithBadFile_throwJsonReaderException()
        {
            try
            {
                File.WriteAllText(JSONStateProvider.FILE_NAME, "Bad data for the JSONStateProvider file");
                JSONStateProvider jsonStateProvider = new JSONStateProvider();
                try
                {
                    Passenger outputPassenger = jsonStateProvider.GetPassengerByBagID(1);
                    Assert.Fail("failed to trigger exception");
                }
                catch (JsonReaderException e)
                {

                }
            }
            finally
            {
                File.Delete(JSONStateProvider.FILE_NAME);
            }
        }

        [Fact]
        public void updateNonexistentPassengerByBagID_throwNotFoundException()
        {
            try
            {
                Passenger testPassenger = new Passenger()
                {
                    BookingID = 10,
                    Bags = new List<Bag> {
                        new Bag { BagId = 1, CheckedBag = true },
                        new Bag { BagId = 2, CheckedBag = false },
                    },
                    FlightLegs = new List<Flight> {
                        new Flight { DepartureDateLocal = 1000, FlightNumber = "1234", CarrierCode = "AC", Market = Flight.FlightMarket.Domestic },
                        new Flight { DepartureDateLocal = 5000, FlightNumber = "5678", CarrierCode = "VA", Market = Flight.FlightMarket.International },
                    },
                    MarkedAsIneligible = false,
                    HasBoardedBSM = true,
                    ScannedAtSmartGate = true,
                    HasBDXMessage = false,
                };
                JSONStateProvider jsonStateProvider = new JSONStateProvider();
                File.WriteAllText(JSONStateProvider.FILE_NAME, JsonConvert.SerializeObject(new List<Passenger> { testPassenger }));
                testPassenger.BookingID = 55;
                testPassenger.MarkedAsIneligible = true;
                testPassenger.HasBDXMessage = true;
                try
                {
                    jsonStateProvider.UpdatePassengerByBagID(testPassenger, 5);
                    Assert.Fail();
                }
                catch (StateProvider.PassengerNotFoundException e)
                {
                    Assert.Equal("Could not find passenger with bag ID 5", e.Message);
                }
            }
            finally
            {
                File.Delete(JSONStateProvider.FILE_NAME);
            }
        }

        [Fact]
        public void deletePassengerByBookingID_passengerRemovedFromFile( )
        {
            Passenger testPassenger1 = new Passenger()
            {
                BookingID = 10,
                Bags = new List<Bag> {
                        new Bag { BagId = 1, CheckedBag = true },
                        new Bag { BagId = 2, CheckedBag = false },
                    },
                FlightLegs = new List<Flight> {
                        new Flight { DepartureDateLocal = 1000, FlightNumber = "1234", CarrierCode = "AC", Market = Flight.FlightMarket.Domestic },
                        new Flight { DepartureDateLocal = 5000, FlightNumber = "5678", CarrierCode = "VA", Market = Flight.FlightMarket.International },
                    },
                MarkedAsIneligible = false,
                HasBoardedBSM = true,
                ScannedAtSmartGate = true,
                HasBDXMessage = false,
            };

            Passenger testPassenger2 = new Passenger()
            {
                BookingID = 20,
                Bags = new List<Bag> {
                        new Bag { BagId = 3, CheckedBag = true },
                        new Bag { BagId = 4, CheckedBag = false },
                    },
                FlightLegs = new List<Flight> {
                        new Flight { DepartureDateLocal = 1000, FlightNumber = "1234", CarrierCode = "AC", Market = Flight.FlightMarket.Domestic },
                        new Flight { DepartureDateLocal = 5000, FlightNumber = "5678", CarrierCode = "VA", Market = Flight.FlightMarket.International },
                    },
                MarkedAsIneligible = false,
                HasBoardedBSM = false,
                ScannedAtSmartGate = false,
                HasBDXMessage = false,
            };
            JSONStateProvider jsonStateProvider = new JSONStateProvider();
            File.WriteAllText(JSONStateProvider.FILE_NAME, JsonConvert.SerializeObject(new List<Passenger> { testPassenger1, testPassenger2 }));
            jsonStateProvider.DeletePassengerByBookingID(10);
            string resultJson = File.ReadAllText(JSONStateProvider.FILE_NAME);
            List<Passenger> resultList = JsonConvert.DeserializeObject<List<Passenger>>(resultJson);
            Assert.Single(resultList);
            Assert.Equal(testPassenger2, resultList[0]);
        }

        [Fact]
        public void deletePassengerWithBadFile_throwJsonReaderException()
        {
            try
            {
                File.WriteAllText(JSONStateProvider.FILE_NAME, "Bad data for the JSONStateProvider file");
                JSONStateProvider jsonStateProvider = new JSONStateProvider();
                try
                {
                    jsonStateProvider.DeletePassengerByBookingID(55);
                    Assert.Fail("failed to trigger exception");
                }
                catch (JsonReaderException e)
                {

                }
            }
            finally
            {
                File.Delete(JSONStateProvider.FILE_NAME);
            }
        }

        [Fact]
        public void deleteNonexistentPassenger_throwNotFoundException()
        {
            try
            {
                Passenger testPassenger = new Passenger()
                {
                    BookingID = 10,
                    Bags = new List<Bag> {
                        new Bag { BagId = 1, CheckedBag = true },
                        new Bag { BagId = 2, CheckedBag = false },
                    },
                    FlightLegs = new List<Flight> {
                        new Flight { DepartureDateLocal = 1000, FlightNumber = "1234", CarrierCode = "AC", Market = Flight.FlightMarket.Domestic },
                        new Flight { DepartureDateLocal = 5000, FlightNumber = "5678", CarrierCode = "VA", Market = Flight.FlightMarket.International },
                    },
                    MarkedAsIneligible = false,
                    HasBoardedBSM = true,
                    ScannedAtSmartGate = true,
                    HasBDXMessage = false,
                };
                JSONStateProvider jsonStateProvider = new JSONStateProvider();
                File.WriteAllText(JSONStateProvider.FILE_NAME, JsonConvert.SerializeObject(new List<Passenger> { testPassenger }));
                try
                {
                    jsonStateProvider.DeletePassengerByBookingID(55);
                    Assert.Fail();
                }
                catch (StateProvider.PassengerNotFoundException e)
                {
                    Assert.Equal("Could not find passenger with booking ID 55", e.Message);
                }
            }
            finally
            {
                File.Delete(JSONStateProvider.FILE_NAME);
            }
        }
    }
}
