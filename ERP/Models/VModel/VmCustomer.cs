using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ERP.Models.POS;

namespace ERP.Models.VModel
{
    public class VmCustomer
    {
        PosCustomer _customer = new PosCustomer();

        public PosCustomer PosCustomer
        {
            get { return _customer; }
            set { _customer = value; }
        }

         PosCityOrNearestZone _cityOrNearestZone = new PosCityOrNearestZone();

        public PosCityOrNearestZone PosCityOrNearestZone
        {
            get { return _cityOrNearestZone; }
            set { _cityOrNearestZone = value; }
        }

        public PosRegion Region = new PosRegion();

        public PosRegion PosRegion
        {
            get { return Region; }
            set { Region = value; }
        }

         PosCustomerClass _customerClass = new PosCustomerClass();

        public PosCustomerClass PosCustomerClass
        {
            get { return _customerClass; }
            set { _customerClass = value; }
        }
    }
}