using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReactAntdServer.Api.Attributes;
using ReactAntdServer.Api.Utils;
using ReactAntdServer.Model;
using ReactAntdServer.Services; 

namespace ReactAntdServer.Api.Controllers
{
    //[Route("api/[controller]")]
    [CustomRoute]
    [ApiController]
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

        [HttpGet("{id:length(24)}",Name ="GetBook")]
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