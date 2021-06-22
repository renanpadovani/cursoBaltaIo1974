using System;
using System.Data;
using Microsoft.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using Dapper;
using BaltaStore.Domain.StoreContext.Repositories;
using BaltaStore.Domain.StoreContext.Entities;
using BaltaStore.Infra.StoreContext.DataContexts;
using BaltaStore.Domain.StoreContext.Queries;
using System.Collections.Generic;

namespace BaltaStore.Infra.StoreContext.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly BaltaDataContext _context;

        public CustomerRepository(BaltaDataContext context)
        {
            _context = context;
        }

        public bool CheckDocument(string document)
        {
            return _context
                .Connection
                .Query<bool>(
                "spCheckDocument", 
                    new { Document = document }, 
                    commandType: CommandType.StoredProcedure)
                .FirstOrDefault();
        }

        public bool CheckEmail(string email)
        {
            return _context
                .Connection
                .Query<bool>(
                "spCheckEmail",
                    new { Email = email },
                    commandType: CommandType.StoredProcedure)
                .FirstOrDefault();
        }

        public IEnumerable<ListCustomerQueryResult> Get()
        {
            return _context
                .Connection
                .Query<ListCustomerQueryResult>(
                "Select Id, Name = FirstName + ' ' + LastName, Document, Email From Customer",
                    commandType: CommandType.StoredProcedure);
        }

        public GetCustomerQueryResult GetById(Guid id)
        {
            return _context
                .Connection
                .Query<GetCustomerQueryResult>(
                "Select Id, Name = FirstName + ' ' + LastName, Document, Email From Customer Where Id=@id",
                    new { id = id },
                    commandType: CommandType.StoredProcedure)
                .FirstOrDefault();
        }

        public CustomerOrdersCountResult GetCustomerOrdersCount(string document)
        {
            return _context
                .Connection
                .Query<CustomerOrdersCountResult>(
                "spGetCustomerOrdersCount",
                    new { Document = document },
                    commandType: CommandType.StoredProcedure)
                .FirstOrDefault();
        }

        public IEnumerable<ListCustomerOrdersQueryResult> GetOrders(Guid id)
        {
            return _context
                .Connection
                .Query<ListCustomerOrdersQueryResult>(
                "Select Id, Name = FirstName + ' ' + LastName, Document, Email, Number, Total From Customer Inner Join Order On CustomerId = Id Where Id=@id",
                new { id = id },
                commandType: CommandType.StoredProcedure);
        }

        public void Save(Customer customer)
        {
            _context.Connection.Execute("spCreateCustomer",
            new
            {
                Id = customer.Id,
                FirstName = customer.Name.FirstName,
                LastName = customer.Name.LastName,
                Document = customer.Document.Number,
                Email = customer.Email.Address,
                Phone = customer.Phone
            }, commandType: CommandType.StoredProcedure);

            foreach (var address in customer.Addresses)
            {
                _context.Connection.Execute("spCreateAddress",
                new
                {
                    Id = address.Id,
                    CustomerId = customer.Id,
                    Number = address.Number,
                    Complement = address.Complement,
                    District = address.District,
                    City = address.City,
                    State = address.State,
                    Country = address.Country,
                    ZipCode = address.ZipCode,
                    Type = address.Type,
                }, commandType: CommandType.StoredProcedure);
            }

        }
    }
}
