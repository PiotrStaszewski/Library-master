﻿using System;
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
    public class UsersInitTests
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;


        public UsersInitTests()
        {
            _server = ServerFactory.GetServerInstance();
            _client = _server.CreateClient();


            using (var scope = _server.Host.Services.CreateScope())
            {
                var _db = scope.ServiceProvider.GetRequiredService<LibraryContext>();

                _db.User.Add(new User
                {
                    Email = "jd@pja1.edu.pl",
                    Name = "Daniel",
                    Surname = "Jabłoński",
                    Login = "jd1",
                    Password = "ASNDKWQOJRJOP!JO@JOP1"
                });

                _db.User.Add(new User
                {
                    Email = "jd@pja2.edu.pl",
                    Name = "Marcin",
                    Surname = "Jabłoński",
                    Login = "jd2",
                    Password = "ASNDKWQOJRJOP!JO@JOP2"
                });

                _db.SaveChanges();

            }
        }


        [Fact]
        public async Task GetUsers_200Ok()
        {
            //Arrange i Act
            var httpResponse = await _client.GetAsync($"{_client.BaseAddress.AbsoluteUri}api/users");

            httpResponse.EnsureSuccessStatusCode();
            var content = await httpResponse.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<IEnumerable<User>>(content);

            Assert.True(users.Count() == 2);
            Assert.True(users.ElementAt(0).Login == "jd1");
        }

        [Fact]
        public async Task GetUser_200Ok()
        {
            //Arrange i Act
            var httpResponse = await _client.GetAsync($"{_client.BaseAddress.AbsoluteUri}api/users/{2}");

            httpResponse.EnsureSuccessStatusCode();
            var content = await httpResponse.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<User>(content);

            Assert.True(user.Name == "Marcin");
        }



        [Fact]
        public async Task AddUser_200Ok()
        {

               var newUser = new User()
                {
                   Email = "jd@pja3.edu.pl",
                   Name = "Daniel",
                   Surname = "Jabłoński",
                   Login = "jd3",
                   Password = "ASNDKWQOJRJOP!JO@JOP3"
               };



            var serializedUser = JsonConvert.SerializeObject(newUser);

            var payload = new StringContent(serializedUser, Encoding.UTF8, "application/json");

            var postResponses = await _client.PostAsync($"{ _client.BaseAddress.AbsoluteUri}api/users", payload);

            postResponses.EnsureSuccessStatusCode();

            var content = await postResponses.Content.ReadAsStringAsync();

            Assert.Contains("jd3", content);

        }


    }
}
