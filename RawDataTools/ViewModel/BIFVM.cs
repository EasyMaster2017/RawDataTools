using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RawDataTools.Model;

namespace RawDataTools.ViewModel
{
    public class BIFVM : ViewModelBase
    {
        private BIFStruct _bif;

        public BIFVM()
        {
            _bif = new BIFStruct();
        }

        public BIFVM(BIFStruct bifStruct)
        {
            _bif = bifStruct;
        }

        public int ArrayNo
        {
            get
            {
                return _bif.ArrayNo;
            }
            set
            {
                if (_bif.ArrayNo != value)
                {
                    _bif.ArrayNo = value;
                    OnPropertyChanged(nameof(ArrayNo));
                }
            }
        }

        public string PointNo
        {
            get
            {
                return _bif.PointNo;
            }
            set
            {
                if (_bif.PointNo != value)
                {
                    _bif.PointNo = value;
                    OnPropertyChanged(nameof(PointNo));
                }
            }
        }



    }
}
