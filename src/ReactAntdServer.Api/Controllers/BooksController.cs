using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ReactAntdServer.Api.Attributes;
using ReactAntdServer.Api.Enums;
using ReactAntdServer.Model;
using ReactAntdServer.Service.Impl;

namespace ReactAntdServer.Api.Controllers
{
    /// <summary>
    /// 书籍信息
    /// </summary>
    //[Route("api/[controller]")]
    [CustomRoute]
    [ApiController]
    [EnableCors("AllowSpecificOrigin")]
    public class BooksController: ControllerBase
    {
        private readonly BookService _bookService;
        public BooksController(BookService bookService)
        {
            _bookService = bookService;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<List<Book>> Get() =>
            _bookService.Get();

        /// <summary>
        /// 获取列表v2
        /// </summary>
        /// <returns></returns>
        [CustomRoute(ApiVersions.v2,   "BookTest")]
        [HttpGet]
        public async Task<object> V2_BookTest()
        {
            return Ok(new { status = 220, data = "This is version 2" });
        }

        /// <summary>
        /// format 特征可以设置返回特定的格式类型 .{format?}
        /// 统一设置返回类型后此设置无效 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:length(24)}", Name ="GetBook")]
        public ActionResult<Book> Get(string id)
        {
            var book = _bookService.Get(id);
            if (book == null)
            {
                return NotFound();
            } 
            return  book;
        }

        [HttpPost]
        public ActionResult<Book> Create(Book book)
        {
            _bookService.Create(book);
            return CreatedAtRoute("GetBook", new { id = book.Id.ToString() }, book);
        }

        [HttpPut("id:length(24)")]
        public IActionResult Update(string id, Book bookIn)
        {
            var book = _bookService.Get(id);
            if (book == null)
            {
                return NotFound();
            }
            _bookService.Update(id, bookIn);
            return NoContent();
        }

        [HttpDelete("id:length(24)")]
        public IActionResult Delete(string id)
        {
            var book = _bookService.Get(id);
            if (book == null)
            {
                return NotFound();
            }
            _bookService.Remove(id);
            return NoContent();
        } 

    }
} 