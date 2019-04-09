using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace RawDataTools.Model
{
    public class WYL:INotifyPropertyChanged
    {
        private ObservableCollection<BIFStruct> _bifListList = new ObservableCollection<BIFStruct>();
        public ObservableCollection<BIFStruct> BIFList
        {
            get
            {
                return _bifListList;
            }
            set
            {
                if(_bifListList!=value)
                {
                    _bifListList = value;
                    OnPropertyChanged("BIFList");
                }
            }
        }

        private DateTime _exportDateTime = DateTime.Now;
        public DateTime ExportDateTime
        {
            get
            {
                return _exportDateTime;
            }
            set
            {
                if (_exportDateTime != value)
                {
                    _exportDateTime = value;
                    OnPropertyChanged("ExportDateTime");
                }
            }
        }

        private double _minDistance = -2.0;
        /// <summary>
        /// 微调最小值
        /// </summary>
        public double MinDistance
        {
            get
            {
                return _minDistance;
            }
            set
            {
                if (_minDistance != value)
                {
                    _minDistance = value;
                    OnPropertyChanged("MinDistance");
                }
            }
        }

        private double _maxDistance = 2.0;
        /// <summary>
        /// 距离微调最小值
        /// </summary>
        public double MaxDistance
        {
            get
            {
                return _maxDistance;
            }
            set
            {
                if (_maxDistance != value)
                {
                    _maxDistance = value;
                    OnPropertyChanged("MaxDistance");
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

        /// <summary>
        /// 从filePath 加载数据
        /// </summary>
        /// <param name="filePath"></param>
        public void Load(string filePath)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filePath);
                XmlNode xmlRoot = xmlDoc.SelectSingleNode("wyl");
                XmlNode exportDateTimeNode = xmlRoot.SelectSingleNode("ExportTime");
                if (exportDateTimeNode != null)
                {
                    ExportDateTime = DateTime.Parse(exportDateTimeNode.InnerText);
                }

                XmlNodeList pointNodes = xmlRoot.SelectNodes("Point");

                BIFList.Clear();
                for (int i = 0; i < pointNodes.Count; i++)
                {
                    BIFList.Add(UnSerializePoint(pointNodes.Item(i)));
                }

            }
            catch (Exception e)
            {

                throw e;
                //MessageBox.Show(e.Message+"\n"+e.StackTrace);
            }

        }

        /// <summary>
        /// 保存数据到文件
        /// </summary>
        /// <param name="filePath"></param>
        public void WriteData(string filePath)
        {
            XmlDocument xmlDoc = new XmlDocument();

            XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmlDoc.AppendChild(xmlDecl);

            XmlNode xmlRoot = xmlDoc.CreateElement("","wyl","");
            xmlDoc.AppendChild(xmlRoot);

            XmlNode xmlExportDataTimeNode = xmlDoc.CreateElement("ExportTime");
            xmlExportDataTimeNode.InnerText = ExportDateTime.ToLongDateString();

            xmlRoot.AppendChild(xmlExportDataTimeNode);

            foreach (var bif in BIFList)
            {
                XmlNode xmlPointNode = xmlDoc.CreateElement("Point");

                XmlNode xmlArrayNo = xmlDoc.CreateElement("ArrayNo");
                xmlArrayNo.InnerText = bif.ArrayNo.ToString();
                xmlPointNode.AppendChild(xmlArrayNo);

                XmlNode xmlPointNo = xmlDoc.CreateElement("PointNo");
                xmlPointNo.InnerText = bif.PointNo;
                xmlPointNode.AppendChild(xmlPointNo);

                XmlNode xmlHeightNode = xmlDoc.CreateElement("Height");
                xmlHeightNode.InnerText = string.Format("{0:F6}", bif.Height);
                xmlPointNode.AppendChild(xmlHeightNode);

                XmlNode xmlDistanceNode = xmlDoc.CreateElement("Distance");
                xmlDistanceNode.InnerText = string.Format("{0:F6}", bif.Distance);
                xmlPointNode.AppendChild(xmlDistanceNode);

                XmlNode xmlStaffTypeNode = xmlDoc.CreateElement("StaffType");
                xmlStaffTypeNode.InnerText = Enum.GetName(typeof(StaffTypeEnum), bif.StaffType);
                xmlPointNode.AppendChild(xmlStaffTypeNode);

                XmlNode xmlReferNoNode = xmlDoc.CreateElement("ReferNo");
                xmlReferNoNode.InnerText = bif.ReferNo;
                xmlPointNode.AppendChild(xmlReferNoNode);

                XmlNode xmlMeasureTypeNode = xmlDoc.CreateElement("MeasureType");
                xmlMeasureTypeNode.InnerText = Enum.GetName(typeof(MeasureTypeEnum), bif.MeasureType);
                xmlPointNode.AppendChild(xmlMeasureTypeNode);

                XmlNode xmlIsReferNoNode = xmlDoc.CreateElement("IsReferNo");
                xmlIsReferNoNode.InnerText = bif.IsReferNo;
                xmlPointNode.AppendChild(xmlIsReferNoNode);

                XmlNode xmlElevationNode = xmlDoc.CreateElement("Elevation");
                xmlElevationNode.InnerText = string.Format("{0:F6}", bif.Elevation);
                xmlPointNode.AppendChild(xmlElevationNode);

                XmlNode xmlDesignElevationNode = xmlDoc.CreateElement("DesignElevation");
                xmlDesignElevationNode.InnerText = string.Format("{0:F6}", bif.DesignElevation);
                xmlPointNode.AppendChild(xmlDesignElevationNode);

                XmlNode xmlCutNode = xmlDoc.CreateElement("Cut");
                xmlCutNode.InnerText = string.Format("{0:F6}", bif.Cut);
                xmlPointNode.AppendChild(xmlCutNode);

                XmlNode xmlFillNode = xmlDoc.CreateElement("Fill");
                xmlFillNode.InnerText = string.Format("{0:F6}", bif.Fill);
                xmlPointNode.AppendChild(xmlFillNode);

                XmlNode xmlDeltaHeightNode = xmlDoc.CreateElement("DeltaHeight");
                xmlDeltaHeightNode.InnerText = string.Format("{0:F6}", bif.DeltaHeightDh);
                xmlPointNode.AppendChild(xmlDeltaHeightNode);

                xmlRoot.AppendChild(xmlPointNode);
            }

            xmlDoc.Save(filePath);

        }

        /// <summary>
        /// 根据XML 反序列化
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <returns></returns>
        private BIFStruct UnSerializePoint(XmlNode xmlNode)
        {
            BIFStruct bif = new BIFStruct();

            int arrayNo = 0;
            int.TryParse(xmlNode.SelectSingleNode("ArrayNo").InnerText, out arrayNo);

            string pointNo = xmlNode.SelectSingleNode("PointNo").InnerText;

            double height = 0.0;
            double.TryParse(xmlNode.SelectSingleNode("Height").InnerText, out height);

            double distance = 0.0;
            double.TryParse(xmlNode.SelectSingleNode("Distance").InnerText, out distance);

            StaffTypeEnum staffType = ("Upright" == xmlNode.SelectSingleNode("StaffType").InnerText ? StaffTypeEnum.Upright 
                : StaffTypeEnum.Down);

            string referNo = xmlNode.SelectSingleNode("ReferNo").InnerText;

            MeasureTypeEnum measureType = MeasureTypeEnum.I;
            switch (xmlNode.SelectSingleNode("MeasureType").InnerText)
            {
                case "B": measureType = MeasureTypeEnum.B; break;
                case "I": measureType = MeasureTypeEnum.I; break;
                case "F": measureType = MeasureTypeEnum.F; break;
                default: measureType = MeasureTypeEnum.I; break;
            }

            string isReferNo = xmlNode.SelectSingleNode("IsReferNo").InnerText;

            double elevation = 0.0;
            double.TryParse(xmlNode.SelectSingleNode("Elevation").InnerText, out elevation);

            double designElevation = 0.0;
            double.TryParse(xmlNode.SelectSingleNode("DesignElevation").InnerText, out designElevation);

            double cut = 0.0;
            double.TryParse(xmlNode.SelectSingleNode("Cut").InnerText, out cut);

            double fill = 0.0;
            double.TryParse(xmlNode.SelectSingleNode("Fill").InnerText, out fill);

            double deltaHeight = 0.0;
            double.TryParse(xmlNode.SelectSingleNode("DeltaHeight").InnerText, out deltaHeight);

            bif.ArrayNo = arrayNo;
            bif.PointNo = pointNo;
            bif.Height = height;
            bif.Distance = distance;
            bif.StaffType = staffType;
            bif.ReferNo = referNo;
            bif.MeasureType = measureType;
            bif.IsReferNo = isReferNo;
            bif.Elevation = elevation;
            bif.DesignElevation = designElevation;
            bif.Cut = cut;
            bif.Fill = fill;
            bif.DeltaHeightDh = deltaHeight;

            return bif;
        }

        /// <summary>
        /// 更新序号
        /// </summary>
        public void AutoGenerateArrayNo()
        {
            if (BIFList.Count <= 0) 
            {
                return;
            }

            int index = BIFList.First().ArrayNo;
            foreach (var bif in BIFList)
            {
                bif.ArrayNo = index;
                index += 1;
            }
        }

        /// <summary>
        /// 更新列表数据，遍历整个BIFList，统一计算
        /// </summary>
        /// <param name="beginIndex">更新的起始位置，默认为0</param>
        public void UpdateRawData(int beginIndex = 0)
        {
            if (BIFList.Count <= 0)
            {
                return;
            }
            if (BIFList.First().MeasureType != MeasureTypeEnum.B)
            {
                throw new Exception("起始测站点不为后视点，测量出错!");
            }


            for (int i = 1; i < BIFList.Count; i++)
            {
                switch (BIFList.ElementAt(i).MeasureType)
                {
                    case MeasureTypeEnum.I: 
                        // 寻找该测点前后最后一次B测， 作为后视
                        int j = i - 1;
                        while (BIFList.ElementAt(j).MeasureType != MeasureTypeEnum.B)
                        {
                            j--;
                        }
                        BIFList.ElementAt(i).DeltaHeightDh = Math.Round(BIFList.ElementAt(i).Elevation - BIFList.ElementAt(j).Elevation, 6);
                        BIFList.ElementAt(i).Height = BIFList.ElementAt(j).Height - BIFList.ElementAt(i).DeltaHeightDh;
                        BIFList.ElementAt(i).ReferNo = BIFList.ElementAt(j).PointNo;
                        break;
                    case MeasureTypeEnum.F:
                        // 寻找该测点前最后一次B测， 作为后视
                        int k = i - 1;
                        while (BIFList.ElementAt(k).MeasureType != MeasureTypeEnum.B)
                        {
                            k--;
                        }
                        BIFList.ElementAt(i).DeltaHeightDh = Math.Round(BIFList.ElementAt(i).Elevation - BIFList.ElementAt(k).Elevation, 6);
                        BIFList.ElementAt(i).Height = BIFList.ElementAt(k).Height - BIFList.ElementAt(i).DeltaHeightDh;
                        BIFList.ElementAt(i).ReferNo = "-";
                        break;
                    case MeasureTypeEnum.B:
                        //F和B应该是成对出现的，
                        if (BIFList.ElementAt(i - 1).MeasureType != MeasureTypeEnum.F)
                        {
                            break;
                        }

                        BIFList.ElementAt(i).Elevation = BIFList.ElementAt(i - 1).Elevation;
                        BIFList.ElementAt(i).DeltaHeightDh = 0;
                        BIFList.ElementAt(i).ReferNo = "-";
                        break;

                }
            }
        }

        /// <summary>
        /// 微调每次测量的数据
        /// </summary>
        /// <param name="minDistance"></param>
        /// <param name="maxDistance"></param>
        public void UpdateDistance(double minDistance = -1.0, double maxDistance = 2.0)
        {
            Random rd = new Random((new Guid()).GetHashCode());
            foreach (var point in BIFList)
            {
                System.Diagnostics.Debug.Print("当前微调值:{0}", rd.NextDouble() * (maxDistance - minDistance) + minDistance);
                point.Distance = point.Distance + (rd.NextDouble() * (maxDistance - minDistance) + minDistance);
            }
        }

        /// <summary>
        /// 调整B测的读数,采用随机数法，在0.3-1.7m范围内随机数
        /// </summary>
        public void UpdateHeight()
        {
            byte[] seedBytes = new byte[10];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(seedBytes);
            Random rd = new Random(BitConverter.ToInt32(seedBytes,0));
            foreach (var point in _bifListList)
            {
                if (point.MeasureType == MeasureTypeEnum.B)
                {
                    point.Height = rd.NextDouble() * (1.7 - 0.3) + 0.3;
                    System.Diagnostics.Debug.Print("{0}.Heigt:{1}", point.PointNo, point.Height);
                }
            }

            UpdateRawData();
        }
    }
}
