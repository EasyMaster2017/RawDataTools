using Microsoft.Win32;
using OfficeOpenXml;
using RawDataTools.Model;
using RawDataTools.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace RawDataTools
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _importExcelPath = string.Empty;

        private MainWindowVM _mainWindowVM = new MainWindowVM();
        private List<string> _sheetNames = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = _mainWindowVM;
        }

        public void StartOnDoubleClickFile(string filePath)
        {
            _mainWindowVM.PointFilePath = filePath;
            _mainWindowVM.Load(filePath);

        }

        private void PasteData(int row = 0,int column = 0)
        {
            DataObject dataObject = Clipboard.GetDataObject() as DataObject;
            if (dataObject.GetDataPresent(DataFormats.Text)) 
            {
                string[] rowDatas = dataObject.GetDataPresent(DataFormats.Text).ToString().Split('\n');
            }
           
        }

        private void Menu_Open_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openDialog = new OpenFileDialog();
                openDialog.DefaultExt = "*.wyl";
                openDialog.Filter = "BIF.wyl|*.wyl|所有文件（*.*）|*.*";
                if (openDialog.ShowDialog() == true)
                {
                    _mainWindowVM.PointFilePath = openDialog.FileName;
                    _mainWindowVM.Load(openDialog.FileName);
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message + "\n" + exp.StackTrace);
                //throw;
            }         

        }

        private void Menu_Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.Empty == _mainWindowVM.PointFilePath || _mainWindowVM.PointFilePath == null) 
                {
                    Menu_SaveAs_Click(sender, e);
                }
                else
                {
                    _mainWindowVM.WriteData(_mainWindowVM.PointFilePath);
                    _mainWindowVM.Load(_mainWindowVM.PointFilePath);
                }                
            }
            catch (Exception exp)
            {

                MessageBox.Show(exp.Message + "\n" + exp.StackTrace);
            }
        }

        private void Menu_SaveAs_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.DefaultExt = "*.wyl";
                saveDialog.Filter = "BIF.wyl|*.wyl|所有文件（*.*）|*.*";
                saveDialog.OverwritePrompt = true;
                saveDialog.ValidateNames = true;

                if (saveDialog.ShowDialog() == true)
                {
                    _mainWindowVM.PointFilePath = saveDialog.FileName;
                    _mainWindowVM.WriteData(_mainWindowVM.PointFilePath);
                    _mainWindowVM.Load(_mainWindowVM.PointFilePath);
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message + "\n" + exp.StackTrace);
                //throw;
            }
        }

        private void Menu_Paste_Click(object sender, RoutedEventArgs e)
        {
            if (ShowDataGrid.SelectedCells.Count > 0)
            {
                var items = ShowDataGrid.SelectedCells.Select(s => s.Item).Distinct().ToArray();
                var lineNumbers = items.Select(s => ShowDataGrid.Items.IndexOf(s));
                int lineNumber = lineNumbers.FirstOrDefault();               

            }
        }

        /// <summary>
        /// 向前插入一行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_InsertForward_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ShowDataGrid.Items.Count <= 0) 
                {
                    _mainWindowVM.InsertPoint(0);
                }
                else if (ShowDataGrid.SelectedCells.Count > 0)
                {
                    var items = ShowDataGrid.SelectedCells.Select(s => s.Item).Distinct().ToArray();
                    var lineNumbers = items.Select(s => ShowDataGrid.Items.IndexOf(s));
                    int lineNumber = lineNumbers.FirstOrDefault();

                    _mainWindowVM.InsertPoint(lineNumber);
                }
            }
            catch (Exception exp)
            {

                MessageBox.Show(exp.Message);
            }
            
        }

        private void Menu_InsertBackward_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ShowDataGrid.Items.Count <= 0)
                {
                    _mainWindowVM.InsertPoint(0);
                }
                else if (ShowDataGrid.SelectedCells.Count > 0)
                {
                    var items = ShowDataGrid.SelectedCells.Select(s => s.Item).Distinct().ToArray();
                    var lineNumbers = items.Select(s => ShowDataGrid.Items.IndexOf(s));
                    int lineNumber = lineNumbers.FirstOrDefault();

                    _mainWindowVM.InsertPoint(lineNumber + 1);
                }
            }
            catch (Exception exp)
            {

                MessageBox.Show(exp.Message);
            }

        }

        private void Menu_Copy_Click(object sender, RoutedEventArgs e)
        {
            var items = ShowDataGrid.SelectedCells.Select(s => s.Item).Distinct().ToArray();
            var lineNumbers = items.Select(s => ShowDataGrid.Items.IndexOf(s));
            int lineNumber = lineNumbers.FirstOrDefault();
        }

        private void Menu_DeleteCurrent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ShowDataGrid.Items.Count <= 0)
                {
                    return;
                }

                if (ShowDataGrid.SelectedCells.Count > 0)
                {
                    var items = ShowDataGrid.SelectedCells.Select(s => s.Item).Distinct().ToArray();
                    var lineNumbers = items.Select(s => ShowDataGrid.Items.IndexOf(s));
                    int lineNumber = lineNumbers.FirstOrDefault();

                    _mainWindowVM.DeletePoint(lineNumber);
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void ShowDataGrid_LayoutUpdated(object sender, EventArgs e)
        {
            try
            {
                _mainWindowVM.UpdateRawData();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message + "\n" + exp.StackTrace);
            }
            finally
            {
                ShowDataGrid.LayoutUpdated -= ShowDataGrid_LayoutUpdated;
            }
        }

        private void ArrayBtn_Click(object sender, RoutedEventArgs e)
        {
            _mainWindowVM.UpdateArrayNo();
        }

        private void DistanceBtn_Click(object sender, RoutedEventArgs e)
        {
            _mainWindowVM.UpdateDistance(_mainWindowVM.MinDistance, _mainWindowVM.MaxDistance);
        }

        private void HeightBtn_Click(object sender, RoutedEventArgs e)
        {
            _mainWindowVM.UpdateHeight();
        }

        private void ShowDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            ShowDataGrid.LayoutUpdated += ShowDataGrid_LayoutUpdated;
        }

        private void Menu_ExportToExcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ShowDataGrid.Items.Count <= 0)
                {
                    return;
                }

                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.DefaultExt = "*.xlsx";
                saveDialog.FileName = _mainWindowVM.ExportDateTime.ToString("yyyy-MM-dd");
                saveDialog.Filter = string.Format("{0}.xlsx|*.xlsx|所有文件（*.*）|*.*", "Excel文件");
                saveDialog.OverwritePrompt = true;
                saveDialog.ValidateNames = true;

                if (saveDialog.ShowDialog() == false)
                {
                    return;
                }

                if (_mainWindowVM.ExportToExcle(saveDialog.FileName, _mainWindowVM.ExportLanguage))
                {
                    System.Diagnostics.Process.Start(saveDialog.FileName);
                }
            }
            catch (Exception exp)
            {

                MessageBox.Show(exp.Message);
            }
        }

        

        /// <summary>
        /// 从制定Excel中读取表名
        /// </summary>
        /// <param name="filePath"></param>
        private void GetSheetNameFromExcel(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            if (!fileInfo.Exists)
            {
                throw new Exception("文件不存在");
            }

            using (ExcelPackage xlsx = new ExcelPackage(fileInfo, true))
            {
                ExcelWorksheet currentWorksheet = xlsx.Workbook.Worksheets.FirstOrDefault();

                _sheetNames.Clear();
                foreach (var worksheet in xlsx.Workbook.Worksheets)
                {
                    _sheetNames.Add(worksheet.Name);
                }

                SheetNameCB.ItemsSource = _sheetNames;
            }
        }

        /// <summary>
		/// 根据数字，生成字母编号，比如 1 ＝ A ， 27 ＝ AA， 29 ＝ AC
		/// </summary>
		/// <param name="num">数字编号，从0开始计数</param>
		/// <returns>返回一个编号字符串</returns>
		public string GetColumnCode(int num)
        {
            int begin = 'A';
            int end = 'Z';
            int tmpNum = num;
            string valueStr = string.Empty;
            while (tmpNum + begin > end)
            {
                valueStr = ((char)(tmpNum % 26 + begin)).ToString() + valueStr;
                tmpNum = tmpNum / 26 - 1;
            }
            valueStr = ((char)(tmpNum % 26 + begin)).ToString() + valueStr;

            return valueStr;
        }


        /// <summary>
        /// 读取Excel数据
        /// </summary>
        /// <param name="filePath"></param>
        private void ImportFromExcel(string filePath, string sheetName)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            if (!fileInfo.Exists)
            {
                throw new Exception("文件不存在");
            }

            using (ExcelPackage xlsx = new ExcelPackage(fileInfo, true))
            {
                ExcelWorksheet currentWorksheet = xlsx.Workbook.Worksheets.FirstOrDefault();

                var workSheets = xlsx.Workbook.Worksheets.Where(item => item.Name == sheetName);
                if (workSheets.Count() <= 0)
                {
                    throw new Exception(string.Format("文件{0}中没有名为{1}的表", filePath, sheetName));
                }

                // 从指定表中开始读取所有数据到DataTable
                ExcelWorksheet worksheet = workSheets.FirstOrDefault();
                int rows = worksheet.Dimension.End.Row;
                int cols = worksheet.Dimension.End.Column;

                DataTable dt = new DataTable(worksheet.Name);
                // 先构建列
                for (int col = 0; col < cols; col++)
                {
                    dt.Columns.Add(GetColumnCode(col).ToString());
                }

                for (int i = 1; i <= rows; i++)
                {
                    DataRow dr = dt.Rows.Add();

                    for (int j = 1; j < cols; j++)
                    {
                        dr[j - 1] = worksheet.Cells[i, j].Value;
                    }
                }

                foreach (var bif in _mainWindowVM.BIFList)
                {
                    var results = dt.Select(string.Format("A='{0}'", bif.PointNo));
                    var result = results.FirstOrDefault();
                    if (result != null)
                    {
                        bif.Elevation = Convert.ToDouble(result.Field<object>("I"));
                    }

                }
            }
        }

        private void Menu_ImportFromExcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openDialog = new OpenFileDialog();
                openDialog.DefaultExt = "*.xlsx";
                openDialog.Filter = ".xlsx|*.xlsx|所有文件（*.*）|*.*";
                if (openDialog.ShowDialog() == true)
                {
                    _importExcelPath = openDialog.FileName;
                    GetSheetNameFromExcel(_importExcelPath);
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message + "\n" + exp.StackTrace);
                //throw;
            }
        }

        private void SheetNameCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_importExcelPath == string.Empty)
            {
                return;
            }

            string sheetName = SheetNameCB.SelectedItem as string;
            if (sheetName != null)
            {
                ImportFromExcel(_importExcelPath, sheetName);
            }
        }

        private void Menu_Quit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
