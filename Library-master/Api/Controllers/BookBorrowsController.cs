using System.Threading.Tasks;
using Library.Models.DTO;
using Library.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    [Route("api/book-borrows")]
    //[Authorize(Roles="Reader")]
    public class BookBorrowsController : Controller
    {
        private readonly IBookBorrowRepository _bookBorrowRepository;

        public BookBorrowsController(IBookBorrowRepository bookBorrowRepository)
        {
            _bookBorrowRepository = bookBorrowRepository;
        }

        [HttpGet("{idBookBorrow}")]
        public async Task<IActionResult> GetBookBorrow([FromRoute] int idBookBorrow)
        {
            var res = await _bookBorrowRepository.GetBookBorrow(idBookBorrow);
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetBookBorrows()
        {
            var res = await _bookBorrowRepository.GetBookBorrows();
            return Ok(res);
        }

        [HttpPost(Name = nameof(AddBookBorrow))]
        public async Task<IActionResult> AddBookBorrow([FromBody] BookBorrowDto borrow)
        {
            var res = await _bookBorrowRepository.AddBookBorrow(borrow);
            return CreatedAtRoute(nameof(AddBookBorrow), res);
        }

        [HttpPut("{idBookBorrow}")]
        public async Task<IActionResult> UpdateBookBorrow([FromBody] UpdateBookBorrowDto borrow)
        {
            await _bookBorrowRepository.ChangeBookBorrow(borrow);
            return NoContent();
        }

        
    }
}