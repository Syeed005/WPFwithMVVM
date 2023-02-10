using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Serialization;
using ValitE3DProjectCreator.Models;

namespace ValitE3DProjectCreator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static Logger logger;
        public List<Product> Products { get; set; }
        //public List<Equipment> Equipment { get; set; }
        public List<Equipment> Equipment { get; set; }
        private static int count = 0;
        public static List<ProcessedProducts> processedProducts = new List<ProcessedProducts>();
        public MainWindow()
        {           
            NLoggerSetup();           
            LoadAllProducts();
            LoadAllEquipment();
            InitializeComponent();
            EquipmentComboInit();
        }

        private void EquipmentComboInit()
        {
            ComboBox equiCombo = this.FindName("cmb_equipment") as ComboBox;
            equiCombo.SetValue(ComboBox.ItemsSourceProperty, Equipment);
            equiCombo.SetValue(ComboBox.DisplayMemberPathProperty, "Name");
            equiCombo.SelectionChanged += new SelectionChangedEventHandler(this.CreateProduct);
        }

        private void CreateProduct(object sender, SelectionChangedEventArgs e)
        {
            stk_product.Children.Clear();
            count = 0;
            ComboBox comboBox = sender as ComboBox;
            Equipment equipment = (Equipment)comboBox.SelectedItem;
            btn_creareProject.IsEnabled = false;
            if (equipment.Products != null) {
                btn_creareProject.IsEnabled = true;
                foreach (Product item in equipment.Products) {
                    CreateField(item);
                }
            }            
        }

        private void CreateField(Product item)
        {
            count = stk_product.Children.OfType<Grid>().ToList().Count;
            //gererate support grid
            Grid supportGrid = new Grid();
            supportGrid.Name = "stGrid_" + (count);
            supportGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(9, GridUnitType.Star) });
            supportGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(.5, GridUnitType.Star) });
            supportGrid.Margin = new Thickness(0, 5 ,0, 5);

            //button and others placement
            Grid selectedProduct = new Grid();
            Grid.SetColumn(selectedProduct, 0);
            selectedProduct.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(3, GridUnitType.Star) });
            selectedProduct.ColumnDefinitions.Add(new ColumnDefinition());
            selectedProduct.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(2, GridUnitType.Star) });
            selectedProduct.ColumnDefinitions.Add(new ColumnDefinition());
            selectedProduct.ColumnDefinitions.Add(new ColumnDefinition());
            selectedProduct.ColumnDefinitions.Add(new ColumnDefinition());
            selectedProduct.ColumnDefinitions.Add(new ColumnDefinition());
            selectedProduct.ColumnDefinitions.Add(new ColumnDefinition());
            selectedProduct.ColumnDefinitions.Add(new ColumnDefinition());

            //code for combobox
            ComboBox cmbProduct = new ComboBox();
            cmbProduct.Name = "cmbProduct_" + (count);
            Grid.SetColumn(cmbProduct, 0);
            cmbProduct.Margin = new Thickness(0, 0, 2.5, 0);
            cmbProduct.SetValue(ComboBox.ItemsSourceProperty, Products);
            cmbProduct.SetValue(ComboBox.SelectedIndexProperty, item.RPId - 1);
            cmbProduct.SetValue(ComboBox.DisplayMemberPathProperty, "Description");
            //comboBox.SelectionChanged += new SelectionChangedEventHandler(this.UpdateProduct);
            selectedProduct.Children.Add(cmbProduct);

            //code for millid textbox
            TextBox txtMill = new TextBox();
            Grid.SetColumn(txtMill, 1);
            txtMill.Margin = new Thickness(0, 0, 2.5, 0);
            txtMill.Name = "txtMill_" + (count);
            txtMill.Text = "119160";           
            selectedProduct.Children.Add(txtMill);


            //code for euiqid textbox
            TextBox txtEquiId = new TextBox();
            Grid.SetColumn(txtEquiId, 2);
            txtEquiId.Margin = new Thickness(0, 0, 2.5, 0);
            txtEquiId.Name = "txtEquiId_" + (count);
            Binding b = new Binding();
            b.Source = $"{txtMill.Text}.XXX";
            b.Mode = BindingMode.OneWay;
            txtEquiId.SetBinding(TextBox.TextProperty, b);
            selectedProduct.Children.Add(txtEquiId);

            //code for countryid textbox
            TextBox txtCountryId= new TextBox();
            Grid.SetColumn(txtCountryId, 3);
            txtCountryId.Margin = new Thickness(0, 0, 2.5, 0);
            txtCountryId.Name = "txtCountryId_" + (count);
            txtCountryId.Text = "FI";
            selectedProduct.Children.Add(txtCountryId);

            //code for projectCode textbox
            TextBox txtProjectCode = new TextBox();
            Grid.SetColumn(txtProjectCode, 4);
            txtProjectCode.Margin = new Thickness(0, 0, 2.5, 0);
            txtProjectCode.Name = "txtProjectCode_" + (count);
            txtProjectCode.Text = "XXX";
            selectedProduct.Children.Add(txtProjectCode);

            //code for projectCode textbox
            TextBox txtProductCode = new TextBox();
            Grid.SetColumn(txtProductCode, 5);
            txtProductCode.Margin = new Thickness(0, 0, 2.5, 0);
            txtProductCode.Name = "txtProductCode_" + (count);
            txtProductCode.Text = item.Code;
            selectedProduct.Children.Add(txtProductCode);

            //code for projectCodeNo textbox
            TextBox txtProductCodeNo = new TextBox();
            Grid.SetColumn(txtProductCodeNo, 6);
            txtProductCodeNo.Margin = new Thickness(0, 0, 2.5, 0);
            txtProductCodeNo.Name = "txtProductCodeNo_" + (count);
            txtProductCodeNo.Text = "1";
            selectedProduct.Children.Add(txtProductCodeNo);

            //code for projectCodeSec textbox
            TextBox txtProductCodeSecond = new TextBox();
            Grid.SetColumn(txtProductCodeSecond, 7);
            txtProductCodeSecond.Margin = new Thickness(0, 0, 2.5, 0);
            txtProductCodeSecond.Name = "txtProductCodeSecode_" + (count);
            txtProductCodeSecond.Text = item.Code;
            selectedProduct.Children.Add(txtProductCodeSecond);

            //code for projectCodSecNo textbox
            TextBox txtProductCodeSecNo = new TextBox();
            Grid.SetColumn(txtProductCodeSecNo, 8);
            txtProductCodeSecNo.Margin = new Thickness(0, 0, 2.5, 0);
            txtProductCodeSecNo.Name = "txtProductCodeSecNo_" + (count);
            txtProductCodeSecNo.Text = "1";
            selectedProduct.Children.Add(txtProductCodeSecNo);


            supportGrid.Children.Add(selectedProduct);

            //Code for delete button
            Button deleteButton = new Button();
            Grid.SetColumn(deleteButton, 1);
            deleteButton.Content = "Delete";
            deleteButton.Name = "btnDelete_" + (count);
            //deleteButton.Click += new RoutedEventHandler(this.DeleteButton_Click);
            supportGrid.Children.Add(deleteButton);

            stk_product.Children.Add(supportGrid);
        }

        private void UpdateProduct(object sender, SelectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void LoadAllEquipment()
        {
            Equipment = new List<Equipment>();
            string equipmentFile= "Equipment.xml";
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

        private void NLoggerSetup()
        {
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

        private void LoadAllProducts()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Product>));
            try {
                using (var reader = new StreamReader("Products.xml")) {
                    Products = (List<Product>)xmlSerializer.Deserialize(reader);
                }
            } catch (Exception ex) {
                logger.Error(ex.Message);
            }            
        }

        public static T FindChild<T>(DependencyObject parent, string childName) where T : DependencyObject
        {
            // Confirm parent and childName are valid. 
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++) {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                T childType = child as T;
                if (childType == null) {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child, childName);

                    // If the child is found, break so we do not overwrite the found child. 
                    if (foundChild != null) break;
                } else if (!string.IsNullOrEmpty(childName)) {
                    var frameworkElement = child as FrameworkElement;
                    // If the child's name is set for search
                    if (frameworkElement != null && frameworkElement.Name == childName) {
                        // if the child's name is of the request name
                        foundChild = (T)child;
                        break;
                    }
                } else {
                    // child element found.
                    foundChild = (T)child;
                    break;
                }
            }
            return foundChild;
        }

        private void btn_creareProject_Click(object sender, RoutedEventArgs e) {
            processedProducts.Clear();
            Button bt = sender as Button;
            var grid = bt.Parent as Grid;
            var motherStack = grid.Parent as StackPanel;
            var mainStack = motherStack.Children[1];
            for (int i = count; i >= 0; i--) {
                ProcessedProducts procProducts = new ProcessedProducts();
                //procProducts.Name = ((Product)FindChild<ComboBox>(mainStack, $"cmbProduct_{i}").SelectedItem).Description;
                procProducts.MillId = FindChild<TextBox>(mainStack, $"txtMill_{i}").Text;
                procProducts.EquipmentId = FindChild<TextBox>(mainStack, $"txtEquiId_{i}").Text;
                procProducts.Country = FindChild<TextBox>(mainStack, $"txtCountryId_{i}").Text;
                procProducts.ProjectCode = FindChild<TextBox>(mainStack, $"txtProjectCode_{i}").Text;
                procProducts.RPCode = FindChild<TextBox>(mainStack, $"txtProductCode_{i}").Text;
                procProducts.RPCodeNo = FindChild<TextBox>(mainStack, $"txtProductCodeNo_{i}").Text;
                procProducts.MainRPCode = FindChild<TextBox>(mainStack, $"txtProductCodeSecode_{i}").Text;
                procProducts.MainRPCodeNo = FindChild<TextBox>(mainStack, $"txtProductCodeSecNo_{i}").Text;
                processedProducts.Add(procProducts);
            }
        }
    }
}
