using KTI.Moo.Extensions.Shopify.Service;
using Microsoft.Extensions.Azure;
using ShopifySharp;
using ShopifySharp.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace KTI.Moo.Extensions.Shopify.Domain
{

    public class Carrier : KTI.Moo.Extensions.Core.Domain.ICarrier<Model.Carrier>
    {


        private readonly CarrierService _service;



        public Carrier(Config config)
        {
            _service = new(config.defaultURL, config.admintoken);

        }

        public Model.Carrier Add(Model.Carrier carrierDetails)
        {
            try
            {
                var Carrier = new Model.DTO.Carrier(carrierDetails);
                var result = _service.CreateAsync(Carrier).GetAwaiter().GetResult();
                var CarrierModel = new Model.Carrier(result);
                return CarrierModel;
            }
            catch (Exception ex)
            {
                throw new Exception($"Extension Shopify Add. {ex.Message}");
            }
        }

        public bool Delete(Model.Carrier carrier)
        {
            try
            {
                _service.DeleteAsync(carrier.id).GetAwaiter().GetResult();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Extension Shopify Delete. {ex.Message}");
            }
        }


        public bool Delete(int carrierID)
        {
            try
            {
                _service.DeleteAsync(carrierID).GetAwaiter().GetResult();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Extension Shopify Delete. {ex.Message}");
            }
        }



        public Model.Carrier Get(int carrierID)
        {
            try
            {
                var CarrierDTO = _service.GetAsync(carrierID).GetAwaiter().GetResult();

                if (CarrierDTO is null)
                {
                    return new Model.Carrier();
                }

                var Carrier = new Model.Carrier(CarrierDTO);

                return Carrier;
            }
            catch (Exception ex)
            {
                throw new Exception($"Extension Shopify Get by carrierID. {ex.Message}");
            }
        }

        public Model.Carrier Get(long carrierID)
        {
            try
            {
                var CarrierDTO = _service.GetAsync(carrierID).GetAwaiter().GetResult();

                if (CarrierDTO is null)
                {
                    return new Model.Carrier();
                }

                var Carrier = new Model.Carrier(CarrierDTO);

                return Carrier;
            }
            catch (Exception ex)
            {
                throw new Exception($"Extension Shopify Get by carrierID. {ex.Message}");
            }
        }

        public Model.Carrier Get(string carrierID)
        {
            try
            {
                var DTOCarrierList = _service.ListAsync().GetAwaiter().GetResult();

                if (DTOCarrierList is null)
                {
                    return new Model.Carrier();
                }

                var CarrierSearchedByID = DTOCarrierList.FirstOrDefault(carrier => carrier.Id.ToString() == carrierID);

                if (CarrierSearchedByID is null)
                {
                    return new Model.Carrier();
                }

                return new Model.Carrier(CarrierSearchedByID);
            }
            catch (Exception ex)
            {
                throw new Exception($"Extension Shopify Get by carrierID. {ex.Message}");
            }
        }



        public bool Update(Model.Carrier carrierDetails)
        {
            try
            {
                var Carrier = new Model.DTO.Carrier(carrierDetails);
                var result = _service.UpdateAsync(carrierDetails.id, Carrier).GetAwaiter().GetResult();

                return result.Id == carrierDetails.id;
            }
            catch (Exception ex)
            {
                throw new Exception($"Extension Shopify Update. {ex.Message}");
            }
        }

        public Model.Carrier Upsert(Model.Carrier carrierDetails)
        {
            try
            {
                var CarrierDTO = new Model.DTO.Carrier(carrierDetails);

                if (carrierDetails.id > 0)
                {
                    var ExistingCarrier = Get(carrierDetails.id);

                    if (ExistingCarrier.id == carrierDetails.id)
                    {
                        var result = _service.UpdateAsync(carrierDetails.id, CarrierDTO).GetAwaiter().GetResult();
                        var ModelCarrier = new Model.Carrier(CarrierDTO);
                        return ModelCarrier;
                    }

                }

                var CreateCarrier = _service.CreateAsync(CarrierDTO).GetAwaiter().GetResult();

                var CreateModelCarrier = new Model.Carrier(CreateCarrier);

                return CreateModelCarrier;
            }
            catch (Exception ex)
            {
                throw new Exception($"Extension Shopify Upsert. {ex.Message}");
            }
        }
    }
}
