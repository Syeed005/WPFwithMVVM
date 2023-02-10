using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ValitE3DProjectCreator.Models {
    public class ProcessedProducts :Screen {
        //field
        private Product _product;
        private string _millId;
        private string _equipmentId;
        private string _country = "";
        private string _projectCode = "";


        //propertise
        public Product Product {
            get { return _product; }
            set { 
                _product = value;
                NotifyOfPropertyChange(() => RPCode);
                NotifyOfPropertyChange(() => MainRPCode);
            }
        }

        public string MillId {
            get { return _millId; }
            set { 
                _millId = value;
                NotifyOfPropertyChange(() => MillId);
                SampleForEquipment();
            }
        }

        public string EquipmentId {
            get { return _equipmentId; }
            set { 
                _equipmentId = value;
                NotifyOfPropertyChange(() => EquipmentId);
            }
        }

        public string Country {
            get { return _country; }
            set { 
                _country = value.ToUpper();
                NotifyOfPropertyChange(() => Country);
            }
        }

        public string ProjectCode {
            get { return _projectCode; }
            set { 
                _projectCode = value.ToUpper();
                NotifyOfPropertyChange(() => ProjectCode);
            }
        }

        public string RPCode {
            get {
                if (Product != null) {
                    return Product.Code;
                } else {
                    return string.Empty;
                }                
            }
            set { 
                Product.Code = value.ToUpper();
            }           
        }

        public string RPCodeNo { get; set; }

        public string MainRPCode {
            get {
                if (Product != null) {
                    return Product.Code;
                } else {
                    return string.Empty;
                }
            }
            set { Product.Code = value.ToUpper(); }
        }

        public string MainRPCodeNo { get; set; }


        //methods
        private void SampleForEquipment() {
            EquipmentId = $"{MillId}.000";
        }

    }
}
