using Library.Controllers;
using Library.Entities;
using Library.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Linq;

namespace XUnitTestProject1.Controllers
{
    public class BookBorrowsUnitTests
    {
        [Fact]
        public async Task GetBookBorrows_200k()
        {
            var m = new Mock<IBookBorrowRepository>();

            ICollection<BookBorrow> bookBorrows = new List<BookBorrow>
            {
                new BookBorrow{IdBookBorrow=1, IdUser=1, IdBook=1, Comments = "działa1"},
                new BookBorrow{IdBookBorrow=2, IdUser=2, IdBook=2, Comments = "działa2"},
                new BookBorrow{IdBookBorrow=3, IdUser=3, IdBook=3, Comments = "działa3"}
            };

            m.Setup(c => c.GetBookBorrows()).Returns(Task.FromResult(bookBorrows));
            var controller = new BookBorrowsController(m.Object);
            //Act
            var result = await controller.GetBookBorrows();
            //Assert
            Assert.True(result is OkObjectResult);
            var r = result as OkObjectResult;
            Assert.True((r.Value as ICollection<BookBorrow>).Count == 3);
            Assert.True((r.Value as ICollection<BookBorrow>).ElementAt(0).Comments == "działa1");
        }
    }
}
