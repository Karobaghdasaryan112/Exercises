using LINQtoSQL.DTOs;
using LINQtoSQL.Entities;

namespace LINQtoSQL.Service
{
    public static class Extentions
    {
        //Return IQuerable<User>        
        //The query is executed in the database without loading all the data into memory
        //return as IQuerable
        //Where()
        //Select()
        //OrderBy(), OrderByDescending()
        //GroupBy()
        //Skip(), Take()
        //Join()
        //Distinct()

        //return IEnumerbale<User>:
        //All data is loaded into memory and then filtered.

        //____________________________________________________________________________________________________________
        //Extention Methods working with User Entities
        //Filteration
        //Find all users who have at least one order greater than a given amount.

        public static IEnumerable<User> UsersWithContitionAmount(this IQueryable<User> query, decimal MinAmount)
        {
            return query.Where(user => user.Orders.Any(order => order.OrderAmount >= MinAmount));
        }


        public static IQueryable<User> UsersWithContitionAmountLikeSQlQuary(this IQueryable<User> query, decimal MinAmount, decimal MaxAmount)
        {
            var result = from user in query
                         join order in query.SelectMany(u => u.Orders)
                         on user.Id equals order.UserId
                         where order.OrderAmount >= MinAmount && order.OrderAmount <= MaxAmount
                         select user;

            return result;
        }

        public static IQueryable<User> UsersWithContitionAmountWithLINQ(
        this IQueryable<User> users,
        IQueryable<Orders> orders,
        decimal MinAmount,
        decimal MaxAmount)
        {
            var JoinResult = users.Join(
                orders,
                user => user.Id,
                order => order.UserId,
                (user, order) => new { user, order }
            ).Where(res => res.order.OrderAmount > MinAmount && res.order.OrderAmount < MaxAmount)
            .Select(res => res.user).AsQueryable();

            return JoinResult;
        }
        //____________________________________________________________________________________________________________




        //Select all users registered in the last 30 days.

        public static IEnumerable<User> UsersWithRegistrationDataCondition(this IQueryable<User> users, int Days)
        {
            return users.Where(user => user.CreatedAt.Value.Day <= Days);
        }


        public static IQueryable<User> UsersWithRegistrationDataCondition(this IQueryable<User> users, int MinDays, int MaxDays)
        {
            var result = from user in users
                         where user.CreatedAt.Value.Day >= MinDays && user.CreatedAt.Value.Day <= MaxDays
                         select user;
            return result;
        }

        public static IQueryable<User> UsersWithRegistrationDataConditionLINQ(this IQueryable<User> users, int MinDays, int MaxDays)
        {
            var result = users.
                Where(user => DateTime.Now.Day - user.CreatedAt.Value.Day >= MinDays && DateTime.Now.Day - user.CreatedAt.Value.Day <= MaxDays);

            return result;
        }
        //____________________________________________________________________________________________________________




        //Find all users who have placed an order with the specified product.
        public static IQueryable<User> UserWithSpecifiedProductLINQ(this IQueryable<User> users, IQueryable<Orders> orders, string ProductName)
        {
            var JoinResult =
                 users.Join(
                 orders,
                 User => User.Id,
                 Order => Order.UserId,
                 (user, order) => new { user, order.ProductName }
                 ).Where(res => res.ProductName == ProductName).
                 Select(res => res.user);

            return JoinResult;
        }

        public static IEnumerable<User> UserWithSpecifiedProductEnumerable(this IEnumerable<User> users, string productName)
        {
            var usersWithSpecificProduct = users
                .Where(user => user.Orders.Any(order => order.ProductName == productName));

            return usersWithSpecificProduct;
        }

        public static IQueryable<User> UserWithSpecifiedProductSQL(this IQueryable<User> users, string ProductName)
        {
            var result = from user in users
                         join order in users.SelectMany(user => user.Orders)
                         on user.Id equals order.UserId
                         where order.ProductName == ProductName
                         select user;

            return result;
        }


        //____________________________________________________________________________________________________________
        //____________________________________________________________________________________________________________
        //Sort
        //Sort users first by registration date, then by last name in alphabetical order.

        public static IQueryable<User> OrderByCreatedDateAndLastNameLINQ(this IQueryable<User> users)
        {
            var result = users.
                OrderBy(u => u.DateOfBirth).
                ThenBy(u => u.FirstName);
            return result;
        }


        public static IQueryable<User> OrderByCreatedDateAndLastNameSQL(this IQueryable<User> users)
        {
            var Result = from user in users
                         orderby user.DateOfBirth, user.FirstName
                         select user;
            return Result;
        }


        //____________________________________________________________________________________________________________
        //Sort users by number of orders made.
        public static IQueryable<User> OrderByNumberOfOrderMadeLINQ(this IQueryable<User> users)
        {
            var result = users.OrderByDescending(user => user.Orders.Count());

            return result;
        }


        public static IQueryable<User> OrderByNumberOfOrderMadeSQL(this IQueryable<User> users)
        {
            var result = from user in users
                         orderby user.Orders.Count() descending
                         select user;

            return result;
        }






        //____________________________________________________________________________________________________________
        //____________________________________________________________________________________________________________
        //Agregate Functions
        //Find the average age of all users who have at least one order.
        public static double AvverageAgeOfUsersWithHavingOrderLINQ(this IQueryable<User> users)
        {
            var result = users.
                Where(user => user.Orders.Any()).
                Average(user => user.DateOfBirth.AddYears((DateTime.Today.Year - user.DateOfBirth.Year)) > DateTime.Today ?
                DateTime.Today.Year - user.DateOfBirth.Year - 1 :
                 DateTime.Today.Year - user.DateOfBirth.Year);
            return result;
        }


        public static double AvverageAgeOfUsersWithHavingOrderSQL(this IQueryable<User> users)
        {
            var query = from user in users
                        where user.Orders.Any()
                        select new
                        {
                            user.DateOfBirth,
                        };

            var result = query.Average(UserDate => UserDate.DateOfBirth.AddYears(DateTime.Today.Year - UserDate.DateOfBirth.Year) > DateTime.Today ?
                DateTime.Today.Year - UserDate.DateOfBirth.Year - 1 :
                DateTime.Today.Year - UserDate.DateOfBirth.Year
                );
            return result;
        }

        //____________________________________________________________________________________________________________
        //Group users by registration year and count the number of users in each year.
        public static IQueryable<UserRegistrationGroup> GroupUsersByRegisdtrationAndCountLINQ(this IQueryable<User> users)
        {
            var result = users.
                GroupBy(user => user.CreatedAt).
                    Select(group => new UserRegistrationGroup { CreatedAt = group.Key, Count = group.Count() });

            return result;
        }

        public static IQueryable<UserRegistrationGroup> GroupByRegistrationAndCountSQL(this IQueryable<User> users)
        {
            var result = from user in users
                         group user by user.CreatedAt into UserGruop
                         select new UserRegistrationGroup
                         {
                             CreatedAt = UserGruop.Key,
                             Count = UserGruop.Count()
                         };

            return result;
        }


        //____________________________________________________________________________________________________________
        //Group users by age and count the number of users in each age.

        public static IQueryable<UsersByAgeGroup> UsersByAgeGroupLINQ(this IQueryable<User> users)
        {

            var result = users.GroupBy(user => CalculateAge(user.DateOfBirth))
                .Select(group => new UsersByAgeGroup
                {
                    Age = group.Key,
                    Count = group.Count()
                });
            return result;
        }

        private static IQueryable<UsersByAgeGroup> UsersByAgeGroupSQL(this IQueryable<User> users)
        {
            var result = from user in users
                         group users by CalculateAge(user.DateOfBirth) into UserGruop
                         select new UsersByAgeGroup
                         {
                             Age = UserGruop.Key,
                             Count = UserGruop.Count()
                         };
            return result;
        }

        private static int CalculateAge(DateTime DateOfBirth)
        {
            var Age = DateTime.Now.Year - DateOfBirth.Year;
            if (DateOfBirth.Date > DateTime.Now.AddYears(-Age))
                Age--;
            return Age;
        }















        //Extention Methods working with Order Entities
        //Sort
        //Sort orders first by creation date, then by amount in descending order.

        //____________________________________________________________________________________________________________
        public static IQueryable<Orders> OrderByCreationDateAndAmountLINQ(this IQueryable<Orders> orders)
        {
            var result = orders.
                OrderBy(order => order.CreatedAt).
                ThenByDescending(order => order.OrderAmount);

            return result;
        }

        public static IQueryable<Orders> OrderByCreationDateAndAmountSQL(this IQueryable<Orders> orders)
        {
            var result = from order in orders
                         orderby order.CreatedAt, order.OrderAmount descending
                         select order;

            return result;
        }
        //____________________________________________________________________________________________________________


        //Sort user orders by number of items, starting with the largest.
        public static IQueryable<Orders> OrderByProductCountLINQ(this IQueryable<Orders> orders)
        {
            var result = orders.OrderByDescending(order => order.ProductCount);
            return result;
        }

        public static IQueryable<Orders> OrderByProductCountSQL(this IQueryable<Orders> orders)
        {
            var result = from order in orders
                         orderby order.ProductCount descending
                         select order;

            return result;
        }


        //____________________________________________________________________________________________________________
        //____________________________________________________________________________________________________________
        //Calculate the total number of orders made by users over the last month.
        public static int ToalNumberOfOrdersInLast30DaysLINQ(this IQueryable<Orders> orders)
        {
            var result = orders.Where(order => order.OrderDateTime > DateTime.Now.AddDays(-30)).Count();
            return result;
        }


        public static int TotalNumberOfOrdersInLast30DaysSQL(this IQueryable<Orders> orders)
        {
            var result = (from order in orders
                          where order.OrderDateTime > DateTime.Now.AddDays(-30)
                          select order).Count();

            return result;
        }

        //Find the maximum order amount made by a user registered last year.

        public static decimal MaxAmountOfOrderInLastYearLINQ(this IQueryable<Orders> orders)
        {
            var result = orders.
                Where(order => DateTime.Now.AddYears(-1) <= order.OrderDateTime && order.OrderDateTime < DateTime.Now).
                Max(order => order.OrderAmount);

            return result;
        }

        public static decimal MaxAmountOfOrderInlastYearSQL(this IQueryable<Orders> orders)
        {
            var result = (from order in orders
                          where DateTime.Now.AddYears(-2) <= order.OrderDateTime && order.OrderDateTime <= DateTime.Now.AddYears(-1)
                          select order).Max(order => order.OrderAmount);
            return result;
        }




        //____________________________________________________________________________________________________________
        //____________________________________________________________________________________________________________
        //GroupBy
        //Group orders by status and count the number of orders in each status.

        public static IQueryable<OrderStatusGroup> GroupByOrderStatusAndCountLINQ(this IQueryable<Orders> orders)
        {
            var result = orders
                .GroupBy(order => order.Status)
                .Select(gruop => new OrderStatusGroup { Count = gruop.Count(), status = gruop.Key });
            return result;
        }

        public static IQueryable<object> GruopByOrderStatusAndCountSQL(this IQueryable<Orders> orders)
        {
            var result = from order in orders
                         group order by order.Status into orderGruop
                         select new OrderStatusGroup
                         {
                             status = orderGruop.Key,
                             Count = orderGruop.Count()
                         };

            return result;
        }

        //____________________________________________________________________________________________________________
        //Group orders by user and calculate the total order amount for each user.

        public static IQueryable<OrdersByUserGroup> GroupOrdersByUserLINQ(this IQueryable<Orders> orders)
        {
            var result = orders.
                GroupBy(order => order.UserId).
                Select(group => new OrdersByUserGroup
                {
                    Count = group.Count(),
                    UserId = group.Key,
                    TotalAmount = group.Sum(order => order.OrderAmount)
                }
                );


            return result;
        }

        public static IQueryable<OrdersByUserGroup> GroupOrderByUserSQL(this IQueryable<Orders> orders)
        {
            IQueryable<OrdersByUserGroup> result = from order in orders
                                                   group order by order.UserId into OrderGroup
                                                   select new OrdersByUserGroup
                                                   {
                                                       UserId = OrderGroup.Key,
                                                       Count = OrderGroup.Count(),
                                                       TotalAmount = OrderGroup.Sum(order => order.OrderAmount)
                                                   };
            return result;

        }


        //____________________________________________________________________________________________________________
        //Group orders by month and count how many orders were made in each month.

        public static IQueryable<OrderingByMonthGroup> GroupByMonthOrdringLINQ(this IQueryable<Orders> orders)
        {
            var result = orders.
                GroupBy(order => order.CreatedAt.Month).
              Select(group =>
              new OrderingByMonthGroup
              { 
                  Count = group.Count(),
                  OrderMonth = group.Key
              });

            return result;

        }

        public static IQueryable<OrderingByMonthGroup> GroupByMonthOrdringSQL(this IQueryable<Orders> orders)
        {
            var result = from order in orders
                         group orders by order.CreatedAt.Month into groupMonth
                         select new OrderingByMonthGroup
                         {
                             Count = groupMonth.Count(),
                             OrderMonth = groupMonth.Key
                         };

            return result;

        }

    }
}
