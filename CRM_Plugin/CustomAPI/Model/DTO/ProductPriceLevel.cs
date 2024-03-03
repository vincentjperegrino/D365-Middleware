using Microsoft.Xrm.Sdk;
using System;

namespace CRM_Plugin.CustomAPI.Model.DTO
{
    public class ProductPriceLevel : CRM_Plugin.Core.Model.ProductPriceLevelBase
    {
        public ProductPriceLevel(Entity productPriceLevel)
        {
            string ErrorEntity = "";

            try
            {

                ErrorEntity = CustomAPI.Helper.EntityHelper.ProductPriceLevel.amount;
                if (productPriceLevel.Contains(CustomAPI.Helper.EntityHelper.ProductPriceLevel.amount))
                {
                    if (!string.IsNullOrWhiteSpace(productPriceLevel[CustomAPI.Helper.EntityHelper.ProductPriceLevel.amount].ToString()))
                    {
                        decimal.TryParse(productPriceLevel[CustomAPI.Helper.EntityHelper.ProductPriceLevel.amount].ToString(), out _amount);
                    }
                }

                ErrorEntity = CustomAPI.Helper.EntityHelper.ProductPriceLevel.discounttypeid;
                if (productPriceLevel.Contains(CustomAPI.Helper.EntityHelper.ProductPriceLevel.discounttypeid))
                {
                    if (!string.IsNullOrWhiteSpace(productPriceLevel[CustomAPI.Helper.EntityHelper.ProductPriceLevel.discounttypeid].ToString()))
                    {
                        _discounttypeid = (string)productPriceLevel[CustomAPI.Helper.EntityHelper.ProductPriceLevel.discounttypeid];
                    }
                }

                ErrorEntity = CustomAPI.Helper.EntityHelper.ProductPriceLevel.createdon;
                if (productPriceLevel.Contains(CustomAPI.Helper.EntityHelper.ProductPriceLevel.createdon))
                {
                    if (!string.IsNullOrWhiteSpace(productPriceLevel[CustomAPI.Helper.EntityHelper.ProductPriceLevel.createdon].ToString()))
                    {
                        _createdon = (DateTime)productPriceLevel[CustomAPI.Helper.EntityHelper.ProductPriceLevel.createdon];
                    }
                }

                ErrorEntity = CustomAPI.Helper.EntityHelper.ProductPriceLevel.percentage;
                if (productPriceLevel.Contains(CustomAPI.Helper.EntityHelper.ProductPriceLevel.percentage))
                {
                    if (!string.IsNullOrWhiteSpace(productPriceLevel[CustomAPI.Helper.EntityHelper.ProductPriceLevel.percentage].ToString()))
                    {
                        decimal.TryParse(productPriceLevel[CustomAPI.Helper.EntityHelper.ProductPriceLevel.percentage].ToString(), out _percentage);
                    }
                }

                ErrorEntity = CustomAPI.Helper.EntityHelper.ProductPriceLevel.pricelevelid;
                if (productPriceLevel.Contains(CustomAPI.Helper.EntityHelper.ProductPriceLevel.pricelevelid))
                {
                    if (!string.IsNullOrWhiteSpace(productPriceLevel[CustomAPI.Helper.EntityHelper.ProductPriceLevel.pricelevelid].ToString()))
                    {
                        _pricelevelid = (string)productPriceLevel[CustomAPI.Helper.EntityHelper.ProductPriceLevel.pricelevelid];
                    }
                }

                ErrorEntity = CustomAPI.Helper.EntityHelper.ProductPriceLevel.pricingmethodcode;
                if (productPriceLevel.Contains(CustomAPI.Helper.EntityHelper.ProductPriceLevel.pricingmethodcode))
                {
                    if (!string.IsNullOrWhiteSpace(productPriceLevel[CustomAPI.Helper.EntityHelper.ProductPriceLevel.pricingmethodcode].ToString()))
                    {
                        Int32.TryParse(productPriceLevel[CustomAPI.Helper.EntityHelper.ProductPriceLevel.pricingmethodcode].ToString(), out _pricingmethodcode);
                    }
                }

                ErrorEntity = CustomAPI.Helper.EntityHelper.ProductPriceLevel.productid;
                if (productPriceLevel.Contains(CustomAPI.Helper.EntityHelper.ProductPriceLevel.productid))
                {
                    if (!string.IsNullOrWhiteSpace(productPriceLevel[CustomAPI.Helper.EntityHelper.ProductPriceLevel.productid].ToString()))
                    {
                        _productid = (string)productPriceLevel[CustomAPI.Helper.EntityHelper.ProductPriceLevel.productid];
                    }
                }

                ErrorEntity = CustomAPI.Helper.EntityHelper.ProductPriceLevel.productpricelevelid;
                if (productPriceLevel.Contains(CustomAPI.Helper.EntityHelper.ProductPriceLevel.productpricelevelid))
                {
                    if (!string.IsNullOrWhiteSpace(productPriceLevel[CustomAPI.Helper.EntityHelper.ProductPriceLevel.productpricelevelid].ToString()))
                    {
                        _productpricelevelid = (string)productPriceLevel[CustomAPI.Helper.EntityHelper.ProductPriceLevel.productpricelevelid];
                    }
                }

                ErrorEntity = CustomAPI.Helper.EntityHelper.ProductPriceLevel.quantitysellingcode;
                if (productPriceLevel.Contains(CustomAPI.Helper.EntityHelper.ProductPriceLevel.quantitysellingcode))
                {
                    if (!string.IsNullOrWhiteSpace(productPriceLevel[CustomAPI.Helper.EntityHelper.ProductPriceLevel.quantitysellingcode].ToString()))
                    {
                        Int32.TryParse(productPriceLevel[CustomAPI.Helper.EntityHelper.ProductPriceLevel.quantitysellingcode].ToString(), out _quantitysellingcode);
                    }
                }

                ErrorEntity = CustomAPI.Helper.EntityHelper.ProductPriceLevel.roundingoptionamount;
                if (productPriceLevel.Contains(CustomAPI.Helper.EntityHelper.ProductPriceLevel.roundingoptionamount))
                {
                    if (!string.IsNullOrWhiteSpace(productPriceLevel[CustomAPI.Helper.EntityHelper.ProductPriceLevel.roundingoptionamount].ToString()))
                    {
                        decimal.TryParse(productPriceLevel[CustomAPI.Helper.EntityHelper.ProductPriceLevel.roundingoptionamount].ToString(), out _roundingoptionamount);
                    }
                }

                ErrorEntity = CustomAPI.Helper.EntityHelper.ProductPriceLevel.roundingoptioncode;
                if (productPriceLevel.Contains(CustomAPI.Helper.EntityHelper.ProductPriceLevel.roundingoptioncode))
                {
                    if (!string.IsNullOrWhiteSpace(productPriceLevel[CustomAPI.Helper.EntityHelper.ProductPriceLevel.roundingoptioncode].ToString()))
                    {
                        int.TryParse(productPriceLevel[CustomAPI.Helper.EntityHelper.ProductPriceLevel.roundingoptioncode].ToString(), out _roundingoptioncode);
                    }
                }

                ErrorEntity = CustomAPI.Helper.EntityHelper.ProductPriceLevel.roundingpolicycode;
                if (productPriceLevel.Contains(CustomAPI.Helper.EntityHelper.ProductPriceLevel.roundingpolicycode))
                {
                    if (!string.IsNullOrWhiteSpace(productPriceLevel[CustomAPI.Helper.EntityHelper.ProductPriceLevel.roundingpolicycode].ToString()))
                    {
                        int.TryParse(productPriceLevel[CustomAPI.Helper.EntityHelper.ProductPriceLevel.roundingpolicycode].ToString(), out _roundingpolicycode);
                    }
                }

                ErrorEntity = CustomAPI.Helper.EntityHelper.ProductPriceLevel.transactioncurrencyid;
                if (productPriceLevel.Contains(CustomAPI.Helper.EntityHelper.ProductPriceLevel.transactioncurrencyid))
                {
                    if (!string.IsNullOrWhiteSpace(productPriceLevel[CustomAPI.Helper.EntityHelper.ProductPriceLevel.transactioncurrencyid].ToString()))
                    {
                        _transactioncurrencyid = (string)productPriceLevel[CustomAPI.Helper.EntityHelper.ProductPriceLevel.transactioncurrencyid];
                    }
                }

                ErrorEntity = CustomAPI.Helper.EntityHelper.ProductPriceLevel.uomid;
                if (productPriceLevel.Contains(CustomAPI.Helper.EntityHelper.ProductPriceLevel.uomid))
                {
                    if (!string.IsNullOrWhiteSpace(productPriceLevel[CustomAPI.Helper.EntityHelper.ProductPriceLevel.uomid].ToString()))
                    {
                        _uomid = (string)productPriceLevel[CustomAPI.Helper.EntityHelper.ProductPriceLevel.uomid];
                    }
                }

                ErrorEntity = CustomAPI.Helper.EntityHelper.ProductPriceLevel.uomscheduleid;
                if (productPriceLevel.Contains(CustomAPI.Helper.EntityHelper.ProductPriceLevel.uomscheduleid))
                {
                    if (!string.IsNullOrWhiteSpace(productPriceLevel[CustomAPI.Helper.EntityHelper.ProductPriceLevel.uomscheduleid].ToString()))
                    {
                        _uomscheduleid = (string)productPriceLevel[CustomAPI.Helper.EntityHelper.ProductPriceLevel.uomscheduleid];
                    }
                }

                ErrorEntity = CustomAPI.Helper.EntityHelper.ProductPriceLevel.moosourcesystem;
                if (productPriceLevel.Contains(CustomAPI.Helper.EntityHelper.ProductPriceLevel.moosourcesystem))
                {
                    if (!string.IsNullOrWhiteSpace(productPriceLevel[CustomAPI.Helper.EntityHelper.ProductPriceLevel.moosourcesystem].ToString()))
                    {
                        _moosourcesystem = (string)productPriceLevel[CustomAPI.Helper.EntityHelper.ProductPriceLevel.moosourcesystem];
                    }
                }

                ErrorEntity = CustomAPI.Helper.EntityHelper.ProductPriceLevel.mooexternalid;
                if (productPriceLevel.Contains(CustomAPI.Helper.EntityHelper.ProductPriceLevel.mooexternalid))
                {
                    if (!string.IsNullOrWhiteSpace(productPriceLevel[CustomAPI.Helper.EntityHelper.ProductPriceLevel.mooexternalid].ToString()))
                    {
                        _mooexternalid = (string)productPriceLevel[CustomAPI.Helper.EntityHelper.ProductPriceLevel.mooexternalid];
                    }
                }

                ErrorEntity = CustomAPI.Helper.EntityHelper.ProductPriceLevel.companyid;
                if (productPriceLevel.Contains(CustomAPI.Helper.EntityHelper.ProductPriceLevel.companyid))
                {
                    if (!string.IsNullOrWhiteSpace(productPriceLevel[CustomAPI.Helper.EntityHelper.ProductPriceLevel.companyid].ToString()))
                    {
                        int.TryParse(productPriceLevel[CustomAPI.Helper.EntityHelper.ProductPriceLevel.companyid].ToString(), out _companyid);
                    }
                }
            }
            catch
            {
                throw new Exception("Product Price Level: Invalid " + ErrorEntity);
            }
        }

        #region Properties
        private decimal _amount = 0;
        new public decimal amount
        {
            get => _amount;
            set => _amount = value;
        }
        private string _discounttypeid = "";
        new public string discounttypeid
        {
            get => _discounttypeid;
            set => _discounttypeid = value;
        }
        private DateTime _createdon = DateTime.MinValue;
        new public DateTime createdon
        {
            get => _createdon;
            set => _createdon = value;
        }
        private decimal _percentage = 0;
        new public decimal percentage
        {
            get => _percentage;
            set => _percentage = value;
        }
        private string _pricelevelid = "";
        new public string pricelevelid
        {
            get => _pricelevelid;
            set => _pricelevelid = value;
        }
        private int _pricingmethodcode = 0;
        new public int pricingmethodcode
        {
            get => _pricingmethodcode;
            set => _pricingmethodcode = value;
        }
        private string _productid = "";
        new public string productid
        {
            get => _productid;
            set => _productid = value;
        }
        private string _productpricelevelid = "";
        new public string productpricelevelid
        {
            get => _productpricelevelid;
            set => _productpricelevelid = value;
        }
        private int _quantitysellingcode = 0;
        new public int quantitysellingcode
        {
            get => _quantitysellingcode;
            set => _quantitysellingcode = value;
        }
        private decimal _roundingoptionamount = 0;
        new public decimal roundingoptionamount
        {
            get => _roundingoptionamount;
            set => _roundingoptionamount = value;
        }
        private int _roundingoptioncode = 0;
        new public int roundingoptioncode
        {
            get => _roundingoptioncode;
            set => _roundingoptioncode = value;
        }
        private int _roundingpolicycode = 0;
        new public int roundingpolicycode
        {
            get => _roundingpolicycode;
            set => _roundingpolicycode = value;
        }
        private string _transactioncurrencyid = "";
        new public string transactioncurrencyid
        {
            get => _transactioncurrencyid;
            set => _transactioncurrencyid = value;
        }
        private string _uomid = "";
        new public string uomid
        {
            get => _uomid;
            set => _uomid = value;
        }
        private string _uomscheduleid = "";
        new public string uomscheduleid
        {
            get => _uomscheduleid;
            set => _uomscheduleid = value;
        }
        private string _moosourcesystem = "";
        new public string moosourcesystem
        {
            get => _moosourcesystem;
            set => _moosourcesystem = value;
        }
        private string _mooexternalid = "";
        new public string mooexternalid
        {
            get => _mooexternalid;
            set => _mooexternalid = value;
        }
        private int _companyid = 0;
        new public int companyid
        {
            get => _companyid;
            set => _companyid = value;
        }
        #endregion
    }
}
