using System.Collections.Generic;
using System.Threading.Tasks;
using Library.Entities;
using Library.Models.DTO;

namespace Library.Repositories.Interfaces
{
    public interface IBookBorrowRepository : IBaseRepository
    {
        Task<BookBorrow> GetBookBorrow(int bookBorrow);
        Task<ICollection<BookBorrow>> GetBookBorrows();
        Task<BookBorrow> AddBookBorrow(BookBorrowDto borrow);
        Task<bool> ChangeBookBorrow(UpdateBookBorrowDto borrow);
    }
}
