using LINQtoSQL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQtoSQL.Data
{
    public class DataSeeder
    {
        public List<User> GenerateUserDatas()
        {
            var Orders = GenerateOrderDatas();
            return new List<User>
            {
                new User
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    
                    DateOfBirth = new DateTime(1990, 5, 23),
                    Orders = new List<Orders>() {Orders[0]},
                    CreatedAt = DateTime.Now.AddDays(-1),
                },
                new User
                {
                    Id = 2,
                    FirstName = "Jane",
                    LastName = "Smith",
                    DateOfBirth = new DateTime(1985, 7, 15),
                    Orders = new List<Orders>(){Orders[1]},
                    CreatedAt = DateTime.Now.AddDays(-2),

                },
                new User
                {
                    Id = 3,
                    FirstName = "Alice",
                    LastName = "Johnson",
                    DateOfBirth = new DateTime(1992, 10, 30),
                    Orders = new List<Orders>(){Orders[2]},
                    CreatedAt = DateTime.Now.AddDays(-3),
                },
                new User
                {
                    Id = 4,
                    FirstName = "Bob",
                    LastName = "Williams",
                    DateOfBirth = new DateTime(1980, 12, 5),
                    Orders = new List<Orders>(){Orders[3]},
                    CreatedAt = DateTime.Now.AddDays(-4),
                },
                new User
                {
                    Id = 5,
                    FirstName = "Charlie",
                    LastName = "Brown",
                    DateOfBirth = new DateTime(1993, 3, 18),
                    Orders = new List<Orders>(){Orders[4]},
                    CreatedAt = DateTime.Now.AddDays(-5),
                },
            };
        }
        public List<Orders> GenerateOrderDatas()
        {
            return new List<Orders>
        {
            new Orders
            {
                OrderDateTime = DateTime.Now.AddDays(-1),
                OrderAmount = 150.75m,
                ProductName = "phone",
                ProductCount = 1,
                Status = "Pending",
                UserId = 1
            },
            new Orders
            {
                OrderDateTime = DateTime.Now.AddDays(-2),
                OrderAmount = 200.50m,
                ProductName = "Telephone",
                ProductCount = 2,
                Status = "Completed",
                UserId = 2
            },
            new Orders
            {
                OrderDateTime = DateTime.Now.AddDays(-3),
                OrderAmount = 320.00m,
                ProductName = "Car",
                ProductCount = 3,
                Status = "Paiment",
                UserId = 3
            },
            new Orders
            {
                OrderDateTime = DateTime.Now.AddDays(-4),
                OrderAmount = 110.20m,
                ProductName="book",
                ProductCount = 4,
                Status = "faild",
                UserId = 4
            },
            new Orders
            {
                OrderDateTime = DateTime.Now.AddDays(-5),
                OrderAmount = 245.10m,
                ProductName = "Car",
                ProductCount = 5,                
                Status = "Paiment",
                UserId = 5
            },
            new Orders
            {
                OrderDateTime = DateTime.Now.AddDays(-6),
                OrderAmount = 330.50m,
                ProductName = "Telephone",
                ProductCount = 6,
                Status = "Pending",
                UserId = 1
            },
            new Orders
            {
                OrderDateTime = DateTime.Now.AddDays(-7),
                OrderAmount = 125.75m,
                ProductName="book",
                ProductCount = 7,
                Status = "field",
                UserId = 2
            },
            new Orders
            {
                OrderDateTime = DateTime.Now.AddDays(-8),
                OrderAmount = 490.00m,
                ProductName = "phone",
                ProductCount = 8,
                Status = "completed",
                UserId = 3
            }
        };
        }
    }   
}
