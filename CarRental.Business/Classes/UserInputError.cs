using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Business.Classes;

    public class UserInputError
    {
        public bool LicenseError { get; set; } = false;
        public bool OdometerError { get; set; } = false;
        public bool ManufacturerError { get; set; } = false;
        public bool VehicleTypeError { get; set; } = false;
        public bool CostDayError { get; set; } = false;
        public bool CostKmError { get; set; } = false;

        public void Clear()
        {
            LicenseError = false;
            OdometerError = false;
            ManufacturerError = false;
            VehicleTypeError = false;
            CostDayError = false;    
            CostKmError = false;
        }

    }

