using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using BrockSolutions.ITDService.Data;
using Newtonsoft.Json;

namespace BrockSolutions.ITDService.Providers
{
    public class JSONStateProvider : StateProvider
    {
        public const string FILE_NAME = "bookings.json";

        public class CouldNotParseJSONFileException : Exception
        {
            public CouldNotParseJSONFileException(string message) : base(message)
            {
            }
        }

        public List<Passenger> AddPassenger(Passenger passenger)
        {
            if (!File.Exists(FILE_NAME))
            {
                File.Create(FILE_NAME).Close();
            }

            string? oldJson = File.ReadAllText(FILE_NAME);
            List<Passenger>? passengers = null;

            if (oldJson != null)
            {
                passengers = JsonConvert.DeserializeObject<List<Passenger>>(oldJson);
            }

            if (passengers == null)
            {
                passengers = new List<Passenger>();
            }

            passengers.Add(passenger);
            string newJson = JsonConvert.SerializeObject(passengers);

            File.WriteAllText(FILE_NAME, newJson);
            return passengers;
        }

        public Passenger GetPassengerByBookingID(long bookingID)
        {
            string convertedPassengers = File.ReadAllText(FILE_NAME);
            List<Passenger>? passengerList = JsonConvert.DeserializeObject<List<Passenger>>(convertedPassengers);
            if (passengerList == null)
            {
                throw new StateProvider.PassengerNotFoundException("Could not find passenger with booking ID " + bookingID);
            }

            Passenger? resultPassenger = passengerList.FirstOrDefault(passenger => passenger.BookingID == bookingID);
            if (resultPassenger == null)
            {
                throw new StateProvider.PassengerNotFoundException("Could not find passenger with booking ID " + bookingID);
            }

            return resultPassenger;
        }

        public Passenger GetPassengerByBagID(long bagID)
        {
            string convertedPassengers = File.ReadAllText(FILE_NAME);
            List<Passenger>? passengerList = JsonConvert.DeserializeObject<List<Passenger>>(convertedPassengers);
            if (passengerList == null)
            {
                throw new StateProvider.PassengerNotFoundException("Could not find passenger with bag ID " + bagID);
            }

            Passenger? existingPassenger = passengerList.FirstOrDefault(passenger => passenger.Bags.Any(bag => bag.BagId == bagID));
            if (existingPassenger == null)
            {
                throw new StateProvider.PassengerNotFoundException("Could not find passenger with bag ID " + bagID);
            }

            return existingPassenger;
        }

        public void UpdatePassengerByBookingID(Passenger passengerToUpdate)
        {
            string convertedPassengers = File.ReadAllText(FILE_NAME);

            List<Passenger>? passengerList = JsonConvert.DeserializeObject<List<Passenger>>(convertedPassengers);
            if (passengerList == null)
            {
                throw new StateProvider.PassengerNotFoundException("Could not find passenger with booking ID " + passengerToUpdate.BookingID);
            }

            Passenger? existingPassenger = passengerList.FirstOrDefault(passenger => passenger.BookingID == passengerToUpdate.BookingID);
            if (existingPassenger == null)
            {
                throw new StateProvider.PassengerNotFoundException("Could not find passenger with booking ID " + passengerToUpdate.BookingID);
            }

            passengerList.Remove(existingPassenger);
            passengerList.Add(passengerToUpdate);
            string updatedConvertedPassengers = JsonConvert.SerializeObject(passengerList);
            File.WriteAllText(FILE_NAME, updatedConvertedPassengers);
        }

        public void UpdatePassengerByBagID(Passenger passengerToUpdate, long bagID)
        {
            string convertedPassengers = File.ReadAllText(FILE_NAME);

            List<Passenger>? passengerList = JsonConvert.DeserializeObject<List<Passenger>>(convertedPassengers);
            if (passengerList == null)
            {
                throw new StateProvider.PassengerNotFoundException("Could not find passenger with bag ID " + bagID);
            }

            Passenger? resultPassenger = passengerList.FirstOrDefault(passenger => passenger.Bags.Any(bag => bag.BagId == bagID));
            if (resultPassenger == null)
            {
                throw new StateProvider.PassengerNotFoundException("Could not find passenger with bag ID " + bagID);
            }

            passengerList.Remove(resultPassenger);
            passengerList.Add(passengerToUpdate);
            string updatedConvertedPassengers = JsonConvert.SerializeObject(passengerList);
            File.WriteAllText(FILE_NAME, updatedConvertedPassengers);
        }

        public void DeletePassengerByBookingID(long bookingID)
        {
            string convertedPassengers = File.ReadAllText(FILE_NAME);

            List<Passenger>? passengerList = JsonConvert.DeserializeObject<List<Passenger>>(convertedPassengers);
            if (passengerList == null)
            {
                throw new StateProvider.PassengerNotFoundException("Could not find passenger with booking ID " + bookingID);
            }

            Passenger? resultPassenger = passengerList.FirstOrDefault(passenger => passenger.BookingID == bookingID);
            if (resultPassenger == null)
            {
                throw new StateProvider.PassengerNotFoundException("Could not find passenger with booking ID " + bookingID);
            }

            passengerList.Remove(resultPassenger);
            string updatedConvertedPassengers = JsonConvert.SerializeObject(passengerList);
            File.WriteAllText(FILE_NAME, updatedConvertedPassengers);
        }
    }
}
