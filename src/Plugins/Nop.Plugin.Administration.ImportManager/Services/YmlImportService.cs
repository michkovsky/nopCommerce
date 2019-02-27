using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Media;
using Nop.Plugin.Administration.ImportManager.Domain;
using Nop.Services.Catalog;
using Nop.Services.Logging;
using Nop.Services.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Nop.Plugin.Administration.ImportManager.Services
{
    public class YmlImportService:IYmlImportService
    {
        readonly ILogger _logger;
        readonly IPictureService _pictureService;
        readonly IProductService _productService;
        public YmlImportService(ILogger logger, IPictureService pictureService, IProductService productService)
        {
            _logger = logger;
            _pictureService = pictureService;
            _productService = productService;
            
        }
        public YmlDocument Load(string path)
        {
            
            var serializer = new XmlSerializer(typeof(YmlDocument));
            using (var reader = XmlReader.Create(path,new XmlReaderSettings { DtdProcessing=DtdProcessing.Parse}))
            {
                YmlDocument ret = serializer.Deserialize(reader) as YmlDocument;
                return ret;
            }
        }
        private static readonly object  syncroot=new object();
        public void Convert(YmlDocument doc)
        {
            doc.shop.offers.ToList().ForEach(offer =>
            {
                var product = new Product
                {
                    ProductType = ProductType.SimpleProduct,
                    Name = offer.name,
                    ShortDescription = offer.description,
                    Price=offer.price??0,
                    ManufacturerPartNumber = offer.vendorCode,

                };
                    _productService.InsertProduct(product);
                var pics = offer.Pictures.Select(path =>
                {
                    byte[] data = null;
                    using (var client = new WebClient())
                    {
                        try
                        {
                            data = client.DownloadData(path);
                        }
                        catch (Exception ex)
                        {
                            _logger.Warning($"Cann't load file \"{path}\" due {ex.Message} ");
                        }
                    }
                    // TODO: fix mime and seo generation
                    return new { data = data, mime = "image/jpeg", seo = "seo-picture" };
                })
                .Where(parsed => parsed.data != null)
                .Select(parsed => _pictureService.InsertPicture(parsed.data, parsed.mime, parsed.seo))
                .ToLookup(p => p.Id);
                var prod_pics = pics.Select(p => new ProductPicture { PictureId = p.Key, ProductId = product.Id });
                prod_pics.ToList().ForEach(_productService.InsertProductPicture);
                
                
            });
        }
    }
}
