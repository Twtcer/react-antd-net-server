using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReactAntdServer.Api.Attributes;
using ReactAntdServer.Api.Enums;
using ReactAntdServer.Model.Data;
using ReactAntdServer.Service.Impl;

namespace ReactAntdServer.Api.Controllers
{
    /// <summary>
    /// 产品
    /// </summary>
    [CustomRoute(ApiVersions.v1,"admin")]
    //[Produces("application/json")]
    [ApiController] 
    [Authorize]
    public class ProductsController: ControllerBase
    {
        private readonly ProductService _productService;
        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[Authorize("Permission")]
        public ActionResult<List<Product>> GetPageList() =>
            _productService.Get(); 
   

        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        public ActionResult<Product> Get(string id)
        {
            var book = _productService.Get(id);
            if (book == null)
            {
                return NotFound();
            }
            return book;
        }

        [HttpPost]
        public ActionResult<Product> Create(Product p)
        {
            _productService.Create(p);
            return CreatedAtRoute("GetProduct", new { id = p.Id.ToString() }, p);
        }

        [HttpPut("id:length(24)")]
        public IActionResult Update(string id, Product productIn)
        {
            var book = _productService.Get(id);
            if (book == null)
            {
                return NotFound();
            }
            _productService.Update(id, productIn);
            return NoContent();
        }

        [HttpDelete("id:length(24)")]
        public IActionResult Delete(string id)
        {
            var book = _productService.Get(id);
            if (book == null)
            {
                return NotFound();
            }
            _productService.Remove(id);
            return NoContent();
        }

    }
}
