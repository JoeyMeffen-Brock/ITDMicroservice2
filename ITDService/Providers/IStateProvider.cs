using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrockSolutions.ITDService.Data;

namespace BrockSolutions.ITDService.Providers
{
    public interface IStateProvider
    {
        public List<Passenger> AddPassenger(Passenger passenger);
        public Passenger GetPassengerByBookingID(long bookingID);
        public Passenger GetPassengerByBagID(long bagID);
        public void UpdatePassengerByBookingID(Passenger passenger);
        public void UpdatePassengerByBagID(Passenger passenger, long bagID);
        public void DeletePassengerByBookingID(long bookingID);

        public class PassengerNotFoundException : Exception
        {
            public PassengerNotFoundException(string message) : base(message)
            {
            }
        }
    }
}
