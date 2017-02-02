using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace ProductFilter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        List<Worker> workerList = new List<Worker>();

        private string _inputPath;
        public string InputPath
        {
            get
            {
                return _inputPath;
            }
            set
            {
                if (InputPath != value)
                {
                    _inputPath = value;
                    OnPropertyChanged("InputPath");
                }
            }
        }

        private string _outputPath;
        public string OutputPath
        {
            get
            {
                return _outputPath;
            }
            set
            {
                if (OutputPath != value)
                {
                    _outputPath = value;
                    OnPropertyChanged("OutputPath");
                }
            }
        }

        private string _labelBaseStatus;
        public string LabelBaseStatus
        {
            get
            {
                return _labelBaseStatus;
            }
            set
            {
                if (LabelBaseStatus != value)
                {
                    _labelBaseStatus = value;
                    OnPropertyChanged("LabelBaseStatus");
                }
            }
        }

        private string _labelOperationStatus;
        public string LabelOperationStatus
        {
            get
            {
                return _labelOperationStatus;
            }
            set
            {
                if (LabelOperationStatus != value)
                {
                    _labelOperationStatus = value;
                    OnPropertyChanged("LabelOperationStatus");
                }
            }
        }

        private ICommand _loadDatabase;
        public ICommand LoadDatabase
        {
            get
            {
                return _loadDatabase;
            }
            set
            {
                if (LoadDatabase != value)
                {
                    _loadDatabase = value;
                    OnPropertyChanged("LoadDatabase");
                }
            }
        }

        private ICommand _getInputPath;
        public ICommand GetInputPath
        {
            get
            {
                return _getInputPath;
            }
            set
            {
                if (GetInputPath != value)
                {
                    _getInputPath = value;
                    OnPropertyChanged("GetInputPath");
                }
            }
        }

        private ICommand _setOutputPath;
        public ICommand SetOutputPath
        {
            get
            {
                return _setOutputPath;
            }
            set
            {
                if (SetOutputPath != value)
                {
                    _setOutputPath = value;
                    OnPropertyChanged("SetOutputPath");
                }
            }
        }

        private ICommand _filtrBase;
        public ICommand FiltrBase
        {
            get
            {
                return _filtrBase;
            }
            set
            {
                if (FiltrBase != value)
                {
                    _filtrBase = value;
                    OnPropertyChanged("FiltrBase");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindow()
        {
            InitializeComponent();
            inputTextBox.IsEnabled = false;
            textProdukt.IsEnabled = false;
            textOddzial.IsEnabled = false;
            textRok.IsEnabled = false;
            outputTextBox.IsEnabled = false;

            this._loadDatabase = new ICommandLoadDatabase(this);
            this._getInputPath = new ICommandGetInputPath(this);
            this._setOutputPath = new ICommandSetOutputPath(this);
            this._filtrBase = new ICommandFiltrBase(this);
            this.DataContext = this;

            LabelBaseStatus = "Status: Oczekiwanie na wczytanie bazy danych.";
        }

    
        public void LoadDatabaseButtonClick()
        {
            try
            {
                workerList.Clear();
                LabelBaseStatus = "Status: Trwa wczytywanie danych. To może chwilę potrwać...";
                string[] lines = System.IO.File.ReadAllLines(InputPath, Encoding.GetEncoding(codepage:1252));
                int tempId = 1;

                char[] delimiterChars = { ';' };

                for (int i=0; i<lines.Count(); i++)
                {
                    string[] delimitedLine = lines[i].Split(delimiterChars);
                    getDelimitedDataAndAddPersonToList(delimitedLine, tempId);
                }
                textProdukt.IsEnabled = true;
                textOddzial.IsEnabled = true;
                textRok.IsEnabled = true;
                textRok.Text = "2017";
                LabelBaseStatus = "Status: Baza została wczytana.";
                LabelOperationStatus = "Status: Oczekiwanie na zapytanie";
            }
            catch (Exception ex)
            {
                LabelBaseStatus = "Status: Nie ma takiego pliku!";
            }
        }

        public void getDelimitedDataAndAddPersonToList(string[] delimitedLine, int tempId)
        {
            int tempPersonId = 0;
            string tempPersonData = "";
            string tempDepartment = "";
            string tempItem = "";
            string tempItemProperty = "";
            string tempItemCount = "";
            string tempYear = "";
            try { tempId += 1;} catch {}
            try { tempPersonId = Convert.ToInt32(delimitedLine[0]); } catch {}
            try { tempPersonData = delimitedLine[1]; } catch {}
            try { tempDepartment = delimitedLine[6]; } catch {}
            try { tempItem = delimitedLine[2]; } catch {}
            try { tempItemProperty = delimitedLine[5]; } catch {}
            try { tempItemCount = delimitedLine[4]; } catch {}
            try { tempYear = delimitedLine[3]; } catch {}
            workerList.Add(new Worker()
            {
                Id = tempId,
                PersonId = tempPersonId,
                PersonData = tempPersonData,
                Department = tempDepartment,
                Item = tempItem,
                ItemProperty = tempItemProperty,
                ItemCount = tempItemCount,
                Year = tempYear
            });
        }

        public void GetInputPathButtonClick()
        {
            var fileDialog = new System.Windows.Forms.OpenFileDialog();
            var result = fileDialog.ShowDialog();
            switch (result)
            {
                case System.Windows.Forms.DialogResult.OK:
                    var file = fileDialog.FileName;
                    InputPath = file.ToString();
                    inputTextBox.Text = file;
                    inputTextBox.ToolTip = file;
                    break;
                case System.Windows.Forms.DialogResult.Cancel:
                default:
                    inputTextBox.Text = null;
                    inputTextBox.ToolTip = null;
                    break;
            }
        }

        public void SetOutputPathButtonClick()
        {
            var fileDialog = new System.Windows.Forms.SaveFileDialog();

            fileDialog.FileName = "document.txt";
            fileDialog.DefaultExt = ".text";
            fileDialog.Filter = "Text documents (.txt) | *.txt";

            var result = fileDialog.ShowDialog();
            switch (result)
            {
                case System.Windows.Forms.DialogResult.OK:
                    OutputPath = fileDialog.FileName;
                    outputTextBox.Text = OutputPath;
                    break;
                case System.Windows.Forms.DialogResult.Cancel:
                default:
                    OutputPath = InputPath + "(request)";
                    outputTextBox.Text = OutputPath;
                    break;
            }
        }

        public void FiltrBaseButtonClick()
        {
            string whichYear = textRok.Text;
            string whichItem = textProdukt.Text;
            string whichDepartment = textOddzial.Text;
            var uniqueWorkerList = workerList;

            if (whichDepartment == "" && whichYear != "" && whichItem != "") // for given item and year
            {
                uniqueWorkerList = workerList
                .Where(list => list.Year.Contains(whichYear))
                .Where(list => list.Item.Equals(whichItem, StringComparison.OrdinalIgnoreCase))
                .ToList();
                uniqueWorkerList = uniqueWorkerList.OrderBy(x => x.PersonId).ToList();
                for (int i = 0; i < uniqueWorkerList.Count - 1; i++)
                {
                    if (uniqueWorkerList[i + 1].PersonId == uniqueWorkerList[i].PersonId)
                    {
                        uniqueWorkerList.RemoveAt(i + 1);
                        i -= 1;
                    }
                }
                uniqueWorkerList = uniqueWorkerList.OrderBy(x => x.Department).ToList();
            }

            if (whichDepartment == "" && whichYear != "" && whichItem == "") // for given year
            {
                uniqueWorkerList = workerList
                .Where(list => list.Year.Contains(whichYear))
                .ToList();
                uniqueWorkerList = uniqueWorkerList.OrderBy(x => x.Department).ToList();
            }

            if (whichDepartment != "" && whichYear != "" && whichItem == "") // for given year and department
            {
                uniqueWorkerList = workerList
                .Where(list => list.Year.Contains(whichYear))
                .Where(list => list.Department.Equals(whichDepartment, StringComparison.OrdinalIgnoreCase))
                .ToList();
                uniqueWorkerList = uniqueWorkerList.OrderBy(x => x.Department).ToList();
            }

            if (whichDepartment != "" && whichYear != "" && whichItem != "") // for given all fields
            {
                uniqueWorkerList = uniqueWorkerList
                .Where(list => list.Year.Contains(whichYear))
                .Where(list => list.Item.Equals(whichItem, StringComparison.OrdinalIgnoreCase))
                .Where(list => list.Department.Equals(whichDepartment, StringComparison.OrdinalIgnoreCase))
                .ToList();
                uniqueWorkerList = uniqueWorkerList.OrderBy(x => x.PersonId).ToList();
                for (int i = 0; i < uniqueWorkerList.Count - 1; i++)
                {
                    if (uniqueWorkerList[i + 1].PersonId == uniqueWorkerList[i].PersonId)
                    {
                        uniqueWorkerList.RemoveAt(i + 1);
                        i -= 1;
                    }
                }
                uniqueWorkerList = uniqueWorkerList.OrderBy(x => x.PersonData).ToList();
            }

            while (File.Exists(OutputPath))
            {
                OutputPath += "(copy).txt";
            }
            if (OutputPath == "")
            {
                LabelOperationStatus = String.Format("Status: Nie podano ścieżki zapisu.");
                return;
            }
            List<string> linesList = new List<string>();
            for (int i = 0; i < uniqueWorkerList.Count(); i++)
            {
                uniqueWorkerList[i].Id = i + 1;
                string line = String.Format("{0};{1};{2};{3};{4};{5};{6};{7}",
                    uniqueWorkerList[i].Id, uniqueWorkerList[i].PersonId, uniqueWorkerList[i].PersonData,
                    uniqueWorkerList[i].Item, uniqueWorkerList[i].ItemProperty, uniqueWorkerList[i].ItemCount,
                    uniqueWorkerList[i].Year, uniqueWorkerList[i].Department);
                linesList.Add(line);
            }
            File.WriteAllLines(OutputPath, linesList, Encoding.GetEncoding(codepage: 1252));
            LabelOperationStatus = String.Format("Status: Wygenerowano plik: {0}", OutputPath);
            uniqueWorkerList.Clear();
        }
    }

    public class ICommandLoadDatabase : ICommand
    {
        MainWindow _oknoGlowne = null;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this._oknoGlowne.LoadDatabaseButtonClick();
        }

        public ICommandLoadDatabase(MainWindow oknoGlowne)
        {
            this._oknoGlowne = oknoGlowne;
        }
    }

    public class ICommandGetInputPath : ICommand
    {
        MainWindow _oknoGlowne = null;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this._oknoGlowne.GetInputPathButtonClick();
        }

        public ICommandGetInputPath(MainWindow oknoGlowne)
        {
            this._oknoGlowne = oknoGlowne;
        }
    }

    public class ICommandSetOutputPath : ICommand
    {
        MainWindow _oknoGlowne = null;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this._oknoGlowne.SetOutputPathButtonClick();
        }

        public ICommandSetOutputPath(MainWindow oknoGlowne)
        {
            this._oknoGlowne = oknoGlowne;
        }
    }

    public class ICommandFiltrBase : ICommand
    {
        MainWindow _oknoGlowne = null;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this._oknoGlowne.FiltrBaseButtonClick();
        }

        public ICommandFiltrBase(MainWindow oknoGlowne)
        {
            this._oknoGlowne = oknoGlowne;
        }
    }
}
