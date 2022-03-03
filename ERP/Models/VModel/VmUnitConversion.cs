using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ERP.Models.POS;

namespace ERP.Models.VModel
{
    public class VmUnitConversion
    {
        public PosUomGroup _uomGroup = new PosUomGroup();

        public PosUomGroup UomGroup
        {
            get { return _uomGroup; }
            set { _uomGroup = value; }
        }

        public PosUomGroupDetail _uomGroupDetail = new PosUomGroupDetail();

        public PosUomGroupDetail UomGroupDetail
        {
            get { return _uomGroupDetail; }
            set { _uomGroupDetail = value; }
        }
    }
}