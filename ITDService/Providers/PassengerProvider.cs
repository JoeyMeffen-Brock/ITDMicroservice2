using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrockSolutions.DapperWrapper;
using BrockSolutions.ITDService.Options;
using Microsoft.Extensions.Options;

namespace BrockSolutions.ITDService.Providers
{
    public class PassengerProvider
    {
        private readonly IDapperQueryExecutor _dapperQueryExecutor;
        private readonly ILogger _logger;
        private readonly IOptionsMonitor<ITDServiceParameters> _parameters;

        // This is a placeholder for a database of passengers. Replace this and all references to it with real database lookups when we have the database.
        private static readonly List<Passenger> PLACEHOLDER_PASSENGER_DATABASE = new List<Passenger> { new Passenger(1, 0, false, false) };

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
        }


        public virtual bool AddPassengerToDatabase(Passenger passenger)
        {
            return true;
        }

        public virtual Passenger? GetPassengerByID(int passengerId)
        {
            return PLACEHOLDER_PASSENGER_DATABASE.FirstOrDefault(passenger => passenger.PassengerID == passengerId);
        }

        //Returns updated passenger, or null if not found
        public Passenger? UpdatePassengersFromBSM(BSM bsm)
        {
            Passenger? passengerToChange = GetPassengerByID(bsm.PassengerID);
            if (passengerToChange == null)
            {
                _logger.LogError($"Could not find passenger with ID {bsm.PassengerID} in database.");
                return null;
            }

            if (!bsm.IsITDEligible)
            {
                passengerToChange.HasIneligibleBSM = true;
            }

            if (bsm.BoardedBSM)
            {
                passengerToChange.HasBoardedBSM = true;
            }

            return passengerToChange;
        }

        public Passenger? UpdatePassengersFromBCBP(BCBP bcbp)
        {
            Passenger? passengerToChange = GetPassengerByID(bcbp.PassengerID);
            if (passengerToChange == null)
            {
                _logger.LogError($"Could not find passenger with ID {bcbp.PassengerID} in database.");
                return null;
            }

            if (!bcbp.IsITDEligible)
            {
                passengerToChange.HasIneligibleBCBP = true;
            }

            return passengerToChange;
        }
    }
}
