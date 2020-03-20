using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Library.Entities;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;

namespace XUnitTestProject1.Controllers
{
    public class BookBorrowsInitTests
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;


        public BookBorrowsInitTests()
        {
            _server = ServerFactory.GetServerInstance();
            _client = _server.CreateClient();


            using (var scope = _server.Host.Services.CreateScope())
            {
                var _db = scope.ServiceProvider.GetRequiredService<LibraryContext>();

                var date1 = new DateTime(2008, 3, 1, 7, 0, 0);
                var date2 = new DateTime(2009, 3, 1, 7, 0, 0);

                _db.BookBorrow.Add(new BookBorrow
                {
                    IdUser = 1,
                    IdBook = 1,
                    Comments = "działa1"

                });


                _db.SaveChanges();

            }
        }


        [Fact]
        public async Task GetBookBorrows_200Ok()
        {
            //Arrange i Act
            var httpResponse = await _client.GetAsync($"{_client.BaseAddress.AbsoluteUri}api/book-borrows");

            httpResponse.EnsureSuccessStatusCode();
            var content = await httpResponse.Content.ReadAsStringAsync();
            var bookBorrows = JsonConvert.DeserializeObject<IEnumerable<BookBorrow>>(content);


            Assert.True(bookBorrows.Count() == 1);
            Assert.True(bookBorrows.ElementAt(0).Comments == "działa1");
        }


        [Fact]
        public async Task GetBookBorrow_200Ok()
        {
            //Arrange i Act
            var httpResponse = await _client.GetAsync($"{_client.BaseAddress.AbsoluteUri}api/book-borrows/{1}");

            httpResponse.EnsureSuccessStatusCode();
            var content = await httpResponse.Content.ReadAsStringAsync();
            var bookBorrow = JsonConvert.DeserializeObject<BookBorrow>(content);

            Assert.True(bookBorrow.Comments == "działa1");
        }


        [Fact]
        public async Task AddBookBorrow_200Ok()
        {

            var newBookBorrow = new BookBorrow()
            {
                IdUser = 2,
                IdBook = 2,
                Comments = "działa2"
            };



            var serializedUser = JsonConvert.SerializeObject(newBookBorrow);

            var payload = new StringContent(serializedUser, Encoding.UTF8, "application/json");

            var postResponses = await _client.PostAsync($"{ _client.BaseAddress.AbsoluteUri}api/book-borrows", payload);

            postResponses.EnsureSuccessStatusCode();

            var content = await postResponses.Content.ReadAsStringAsync();

            Assert.Contains("działa2", content);

        }

        [Fact]
        public async Task EditBookBorrow_200Ok()
        {

            var editBookBorrow = new BookBorrow()
            {
                IdBookBorrow = 1,
                IdUser = 2,
                IdBook = 2,
                Comments = "działaZedytowane"
            };



            var serializedBookBorrow = JsonConvert.SerializeObject(editBookBorrow);

            var payload = new StringContent(serializedBookBorrow, Encoding.UTF8, "application/json");

            var postResponses = await _client.PutAsync($"{ _client.BaseAddress.AbsoluteUri}api/book-borrows/{1}", payload);

            postResponses.EnsureSuccessStatusCode();






            var httpResponse = await _client.GetAsync($"{_client.BaseAddress.AbsoluteUri}api/book-borrows/{1}");

            httpResponse.EnsureSuccessStatusCode();
            var content = await httpResponse.Content.ReadAsStringAsync();
            var bookBorrow = JsonConvert.DeserializeObject<BookBorrow>(content);

            Assert.True(bookBorrow.Comments == "działaZedytowane");

        }


    }
}
