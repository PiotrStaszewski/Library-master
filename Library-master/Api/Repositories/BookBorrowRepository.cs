﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Library.Entities;
using Library.Models.DTO;
using Library.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Library.Repositories
{
    public class BookBorrowRepository : BaseRepository, IBookBorrowRepository
    {
        public BookBorrowRepository(LibraryContext context) : base(context)
        {
        }


        public async Task<BookBorrow> AddBookBorrow(BookBorrowDto borrow)
        {
            var res = await _context.BookBorrow.AddAsync(new BookBorrow
            {
                IdUser = borrow.IdUser,
                IdBook =  borrow.IdBook,
                BorrowDate = DateTime.Now,
                Comments = borrow.Comments
            });

            await _context.SaveChangesAsync();

            return res.Entity;
        }

        public async Task<bool> ChangeBookBorrow(UpdateBookBorrowDto borrow)
        {
            var borrowFromDb =
                await _context.BookBorrow.SingleOrDefaultAsync(x => x.IdBookBorrow == borrow.IdBookBorrow);

            if (borrowFromDb == null)
                return false;

            borrowFromDb.IdBook = borrow.IdBook;
            borrowFromDb.IdUser = borrow.IdUser;
            borrowFromDb.Comments = borrow.Comments;
            borrowFromDb.BorrowDate = borrow.DateFrom;
            borrowFromDb.ReturnDate = borrow.DateTo;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<ICollection<BookBorrow>> GetBookBorrows()
        {
            var bookBorrowsFromDb = await _context.BookBorrow.ToListAsync();

            return bookBorrowsFromDb;
        }

        public async Task<BookBorrow> GetBookBorrow(int idBookBorrow)
        {
            var bookBorrowDb = await _context.BookBorrow.SingleAsync(x => x.IdBookBorrow == idBookBorrow);

            return bookBorrowDb;
        }
    }
}
