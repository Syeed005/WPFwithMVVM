using Caliburn.Micro;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;
using ValitE3DProjectCreator.Models;

namespace ValitE3DProjectCreator.ViewModels {
    public class ShellViewModel : Screen {
        //field
        private static Logger logger;
        private BindableCollection<Equipment> _equipment = new BindableCollection<Equipment>();
        private Equipment _selectedEuipment = null;
        private BindableCollection<ProcessedProducts> _reportableProducts = new BindableCollection<ProcessedProducts>();




        //propertise
        public BindableCollection<Product> Products { get; set; }
        
        
        public BindableCollection<ProcessedProducts> ReportableProducts {
            get { return _reportableProducts; }
            set {
                _reportableProducts = value;
                NotifyOfPropertyChange(() => ReportableProducts);
            }
        }

        public BindableCollection<Equipment> Equipment {
            get { return _equipment; }
            set { 
                _equipment = value;
                NotifyOfPropertyChange(() => Equipment);
            }
        }
        
        public Equipment SelectedEuipment {
            get { return _selectedEuipment; }
            set {
                _selectedEuipment = value;
                NotifyOfPropertyChange(() => SelectedEuipment);
                CreateReportableList();
            }
        }

        //constructor
        public ShellViewModel() {
            NLoggerSetup();
            LoadAllProducts();
            LoadAllEquipment();
        }

        //methods
        private void CreateReportableList() {
            ReportableProducts.Clear();
            ProcessedProducts processedProducts;
            if (SelectedEuipment !=null) {
                if (SelectedEuipment.Products != null) {
                    foreach (Product product in SelectedEuipment.Products) {
                        processedProducts = new ProcessedProducts();
                        processedProducts.Product = product;
                        ReportableProducts.Add(processedProducts);
                    }
                }                
            }
        }

        private void LoadAllEquipment() {
            string equipmentFile = "Equipment.xml";
            List<Product> products;
            try {
                XmlDocument doc = new XmlDocument();
                doc.Load(equipmentFile);
                XmlNodeList equipments = doc.DocumentElement.SelectNodes("//ComProduct");
                foreach (XmlNode item in equipments) {
                    Equipment output = new Equipment();
                    var childNotes = item.ChildNodes;
                    output.Id = Int32.Parse(childNotes.Item(0).InnerText);
                    output.Name = childNotes.Item(1).InnerText;
                    if (!string.IsNullOrEmpty(childNotes.Item(2).InnerText)) {
                        string[] prod = childNotes.Item(2).InnerText.Split(';');
                        products = new List<Product>();
                        foreach (var pr in prod) {
                            products.Add(Products.Where(x => x.RPId == Int32.Parse(pr)).Single());
                        }
                        output.Products = products;
                    }
                    Equipment.Add(output);
                }
            } catch (Exception ex) {
                logger.Error(ex.Message);
            }
        }

        private void NLoggerSetup() {
            var config = new NLog.Config.LoggingConfiguration();

            string nlogTarget = $"Log\\log_{DateTime.Now.Millisecond}.txt";

            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = nlogTarget };
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");

            config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

            NLog.LogManager.Configuration = config;
            logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Info("NloggerSetup done to " + nlogTarget);
        }

        private void LoadAllProducts() {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Product>));
            try {
                using (var reader = new StreamReader("Products.xml")) {
                    Products = new BindableCollection<Product>((List<Product>)xmlSerializer.Deserialize(reader));
                }
            } catch (Exception ex) {
                logger.Error(ex.Message);
            }
        }

        public void DeleteProduct(object selectedProcessedProducts) {
            if (ReportableProducts != null) {
                ReportableProducts.Remove((ProcessedProducts)selectedProcessedProducts);
            }    
        }

        public void AddProduct(ProcessedProducts processedProducts) {
            if (ReportableProducts != null) {
                processedProducts = new ProcessedProducts();
                ReportableProducts.Add(processedProducts);
            }
        }



        public void Apply() {
            foreach (var item in ReportableProducts) {
                if (item.Product == null ) {
                    MessageBox.Show("No Product is selected.");
                    return;
                }
                if (item.MillId.Length != 6 ) {
                    MessageBox.Show("Mill Id should be 6 digit number.");
                    return;
                }
                if (item.Country.Length != 2) {
                    MessageBox.Show("Country should be 2 letter.");
                    return;
                }
                if (item.ProjectCode.Length != 3) {
                    MessageBox.Show("Project code should be 3 letter.");
                    return;
                }
                if (item.RPCode.Length != 2) {
                    MessageBox.Show("Product code should be 2 letter.");
                    return;
                }
                if (item.MainRPCode.Length != 2) {
                    MessageBox.Show("Product code should be 2 letter.");
                    return;
                }
                if (!int.TryParse(item.RPCodeNo, out int result)) {
                    MessageBox.Show("Product code should be number");
                    return;
                }
                if (!int.TryParse(item.MainRPCodeNo, out int result2)) {
                    MessageBox.Show("Product code should be number");
                    return;
                }
            }
            MessageBox.Show("Do something...");
        }

        
    }
}
