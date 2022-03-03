using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ERP.Models.POS;

namespace ERP.Models.VModel
{
    public class VmTender
    {
        public PosTenderType _tenderType=new PosTenderType();
        public PosTender _tender=new PosTender();


        public PosTenderType PosTenderType
        {
            get { return _tenderType; }
            set { _tenderType = value; }
        }

        public PosTender PosTender
        {
            get { return _tender; }
            set { _tender = value; }
        }
    }
}