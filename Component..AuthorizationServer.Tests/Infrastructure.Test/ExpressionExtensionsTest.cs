using Component.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Component.Tests.Infrastructure.Test
{
    public class ExpressionExtensionsTest
    {
        [Fact]
        public void ReplaceParameterTest() {
            var ids = new List<int>() { 1,2};
            Expression<Func<AccountModel, bool>> predicate = (c) => c.CustomerName.Contains("Name") && c.Balance > 1.1 && ids.Contains(c.Id);
                //&& c.Addresses.Any(x => x.Id == 1);
            var predicateText = predicate.Body.ToString();
            var destPredicate = predicate.ReplaceParameter<AccountModel, Account>();
            var acounts = new List<Account>()
            {
                new Account(){
                    Id=1,
                    Balance=2,
                    CustomerName="CustomerName",
                    Addresses = new List<Address>()
                    {
                        new Address(){
                            Id=1
                        }
                    }
                },
                new Account(){
                    Id=1,
                    CustomerName="CustomerName",
                    Addresses = new List<Address>()
                    {
                        new Address(){
                            Id=3
                        }
                    }
                }

            };

            var result= acounts.Where(destPredicate.Compile()).ToList();
        }

    }

    public class Account
    {
        public int Id { get; set; }
        public double Balance { get; set; }
        public string CustomerName { get; set; }

        public List<Address> Addresses { get; set; }
    }

    public class AddressModel
    {
        public int Id { get; set; }
        public double Balance { get; set; }
        public string CustomerName { get; set; }
    }

    public class Address
    {
        public int Id { get; set; }
        public double Balance { get; set; }
        public string CustomerName { get; set; }
    }


    public class AccountModel
    {
        public int Id { get; set; }
        public double Balance { get; set; }
        public string CustomerName { get; set; }

        public List<Address> Addresses { get; set; }
    }
}
