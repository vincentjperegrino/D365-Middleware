using KTI.Moo.Extensions.Shopify.Service;
using Microsoft.Extensions.Azure;
using ShopifySharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace KTI.Moo.Extensions.Shopify.Domain;

public class Product : KTI.Moo.Extensions.Core.Domain.IProduct<Model.Product>
{

    private readonly ProductService _service;
    private readonly ProductImageService _serviceProductImage;

    public Product(Config config)
    {
        _service = new(config.defaultURL, config.admintoken);
        _serviceProductImage = new(config.defaultURL, config.admintoken);
    }

    public Model.Product Add(Model.Product product)
    {
        try
        {
            var Product = new Model.DTO.Product(product);
            var result = _service.CreateAsync(Product).GetAwaiter().GetResult();
            var ProductModel = new Model.Product(result);
            return ProductModel;
        }
        catch (Exception ex)
        {
            throw new Exception($"Extension Shopify Add. {ex.Message}");
        }
    }

    public async Task<Model.Product> AddAsync(Model.Product product)
    {
        try
        {
            var Product = new Model.DTO.Product(product);
            var result = await _service.CreateAsync(Product);
            var ProductModel = new Model.Product(result);
            return ProductModel;
        }
        catch (Exception ex)
        {
            throw new Exception($"Extension Shopify Add. {ex.Message}");
        }
    }




    public bool Delete(Model.Product product)
    {
        try
        {
            _service.DeleteAsync(product.id).GetAwaiter().GetResult();
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception($"Extension Shopify Delete. {ex.Message}");
        }
    }

    public Model.Product Get(long productId)
    {
        try
        {
            var ProductDTO = _service.GetAsync(productId).GetAwaiter().GetResult();

            if (ProductDTO is null)
            {
                return new Model.Product();
            }

            var Product = new Model.Product(ProductDTO);

            return Product;
        }
        catch (Exception ex)
        {
            throw new Exception($"Extension Shopify Get by productid. {ex.Message}");
        }
    }

    public Model.Product Get(string productSku)
    {
        try
        {
            var DTOProductList = _service.ListAsync().GetAwaiter().GetResult();

            if (DTOProductList is null)
            {
                return new Model.Product();
            }


            var ProductSearchedBySKU = DTOProductList.Items.Where(product => product.Variants.Any(variant => variant.SKU == productSku))
                                                           .Select(product => new Model.Product(product)).FirstOrDefault();

            if (ProductSearchedBySKU is null)
            {
                return new Model.Product();
            }

            return ProductSearchedBySKU;
        }
        catch (Exception ex)
        {
            throw new Exception($"Extension Shopify Get by SKU. {ex.Message}");
        }
    }

    /// <summary>
    /// If product has shopify id it will be check if existing then update.
    /// </summary>
    /// <param name="product">Must have an existing id to update</param>
    /// <returns></returns>
    public Model.Product Upsert(Model.Product product)
    {
        try
        {
            var ProductDTO = new Model.DTO.Product(product);

            if (product.id > 0)
            {
                var ExistingProduct = Get(product.id);

                if (ExistingProduct.id == product.id)
                {
                    var result = _service.UpdateAsync(product.id, ProductDTO).GetAwaiter().GetResult();
                    var ModelProduct = new Model.Product(ProductDTO);
                    return ModelProduct;
                }

            }

            var CreatedProduct = _service.CreateAsync(ProductDTO).GetAwaiter().GetResult();

            var CreatedModelProduct = new Model.Product(CreatedProduct);

            return CreatedModelProduct;
        }
        catch (Exception ex)
        {
            throw new Exception($"Extension Shopify Upsert. {ex.Message}");
        }

    }

    public bool Update(Model.Product product)
    {
        try
        {
            var Product = new Model.DTO.Product(product);
            var result = _service.UpdateAsync(product.id, Product).GetAwaiter().GetResult();

            return result.Id == product.id;
        }
        catch (Exception ex)
        {
            throw new Exception($"Extension Shopify Update. {ex.Message}");
        }
    }

    public bool AddImage(long productId, string externalImageUrl, bool primary = false)
    {
        try
        {
            var productimage = new Model.DTO.ProductImage()
            {
                Position = primary ? 1 : null,
                Src = externalImageUrl,
            };

            var image = _serviceProductImage.CreateAsync(productId, productimage).GetAwaiter().GetResult();

            if (image.ProductId == productId)
            {
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            throw new Exception($"Extension Shopify AddImage. {ex.Message}");
        }
    }

    public bool RemoveImage(long productId, int index)
    {
        try
        {
            var ImageList = _serviceProductImage.ListAsync(productId).GetAwaiter().GetResult();

            var ImageIdForRemoval = ImageList.Items.Where(image => image.Position == index).Select(image => image.Id).FirstOrDefault();

            if (ImageIdForRemoval is null)
            {
                return false;
            }

            _serviceProductImage.DeleteAsync(productId, (long)ImageIdForRemoval).GetAwaiter().GetResult();

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception($"Extension Shopify RemoveImage. {ex.Message}");
        }
    }

    public bool SetPrimaryImage(long productId, int index)
    {
        try
        {
            var ImageList = _serviceProductImage.ListAsync(productId).GetAwaiter().GetResult();

            if (ImageList is null || ImageList.Items is null || !ImageList.Items.Any())
            {
                return false;
            }

            var Image = ImageList.Items.Where(image => image.Position == index).FirstOrDefault();

            if (Image is null || Image.Id is null)
            {
                return false;
            }

            Image.Position = 1;

            var result = _serviceProductImage.UpdateAsync(productId, (long)Image.Id, Image).GetAwaiter().GetResult();

            return result.Id == Image.Id;
        }
        catch (Exception ex)
        {
            throw new Exception($"Extension Shopify SetPrimaryImage. {ex.Message}");
        }
    }


}
