using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RawDataTools.Model
{
    public enum StaffTypeEnum
    {
        Upright,
        Down
    }

    public enum MeasureTypeEnum
    {
        B,
        I,
        F
    }
    public class BIFStruct : INotifyPropertyChanged
    {
        private int _arrayNo;
        /// <summary>
        /// 点号
        /// </summary>
        public int ArrayNo
        {
            get
            {
                return _arrayNo;
            }
            set
            {
                if (_arrayNo != value)
                {
                    _arrayNo = value;
                    OnPropertyChanged("ArrayNo");
                }
            }
        }

        private string _pointNo;
        /// <summary>
        /// 测点名
        /// </summary>
        public string PointNo
        {
            get
            {
                return _pointNo;
            }
            set
            {
                if (_pointNo != value)
                {
                    _pointNo = value;
                    OnPropertyChanged("PointNo");
                }
            }
        }

        private double _height;
        /// <summary>
        /// 读数/高度
        /// </summary>        
        public double Height
        {
            get
            {
                return _height;
            }
            set
            {
                if (_height != value)
                {
                    _height = value;
                    OnPropertyChanged("Height");
                    if (_height >= 2.0 || _height <= 0.0)
                    {
                        HeightColor = "Red";
                    }
                    else
                    {
                        HeightColor = "Black";
                    }
                }
            }
        }

        private double _distance;
        /// <summary>
        /// 距离
        /// </summary>

        public double Distance
        {
            get
            {
                return _distance;
            }
            set
            {
                if(_distance!=value)
                {
                    _distance = value;
                    OnPropertyChanged("Distance");
                }
            }
        }

        /// <summary>
        /// 标尺类型 Upright、Down
        /// </summary>
        public StaffTypeEnum StaffType { get; set; }

        private string _referNo;
        /// <summary>
        /// 参考点名
        /// </summary>
        public string ReferNo
        {
            get
            {
                return _referNo;
            }
            set
            {
                if (_referNo != value)
                {
                    _referNo = value;
                    OnPropertyChanged("ReferNo");
                }
            }
        }
        /// <summary>
        /// 测量方式B、I、F
        /// </summary>
        public MeasureTypeEnum MeasureType { get; set; } = MeasureTypeEnum.I;
        /// <summary>
        /// 支点参考点名
        /// </summary>
        public string IsReferNo { get; set; } = "1";

        private double _elevation;
        /// <summary>
        /// 高程
        /// </summary>
        public double Elevation
        {
            get
            {
                return _elevation;
            }
            set
            {
                if (_elevation != value)
                {
                    _elevation = value;
                    OnPropertyChanged("Elevation");
                }
            }
        }
        /// <summary>
        /// 设计高程
        /// </summary>
        public double DesignElevation { get; set; }
        /// <summary>
        /// 挖
        /// </summary>
        public double Cut { get; set; }
        /// <summary>
        /// 填
        /// </summary>
        public double Fill { get; set; }

        private double _deltaHeightDh;
        /// <summary>
        /// 高差
        /// </summary>
        public double DeltaHeightDh
        {
            get
            {
                return _deltaHeightDh;
            }
            set
            {
                if (_deltaHeightDh != value)
                {
                    _deltaHeightDh = value;
                    OnPropertyChanged(nameof(DeltaHeightDh));
                }
            }
        }

        private string _heightColor = "while";
        public string HeightColor
        {
            get
            {
                return _heightColor;
            }
            set
            {
                if (_heightColor != value)
                {
                    _heightColor = value;
                    OnPropertyChanged(nameof(HeightColor));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
