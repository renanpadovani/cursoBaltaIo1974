using BaltaStore.Domain.StoreContext.Commands.CustomerCommands.Outputs;
using BaltaStore.Domain.StoreContext.CustomerCommands.Inputs;
using BaltaStore.Domain.StoreContext.Entities;
using BaltaStore.Domain.StoreContext.Repositories;
using BaltaStore.Domain.StoreContext.Services;
using BaltaStore.Domain.StoreContext.ValueObjects;
using BaltaStore.Shared.Commands;
using FluentValidator;
using System;
using System.Collections.Generic;
using System.Text;

namespace BaltaStore.Domain.StoreContext.Handlers
{
    public class CustomerHandler : 
        Notifiable, 
        ICommandHandler<CreateCustomerCommand>,
        ICommandHandler<AddAddressCommand>
    {
        private readonly ICustomerRepository _repository;
        private readonly IEmailService _emailService;

        //Injeção de dependência do repositorio e do serviço (no construtor da classe)
        public CustomerHandler(ICustomerRepository repository, IEmailService emailService)
        {
            _repository = repository;
            _emailService = emailService;
        }


        public ICommandResult Handle(CreateCustomerCommand command)
        {

            if (_repository.CheckDocument(command.Document))
            {
                AddNotification("Document", "Este CPF já está em uso");
            }

            if (_repository.CheckEmail(command.Email))
            {
                AddNotification("E-mail", "Este e-mail está em uso");
            } 

            //Criar VOs
            var name = new Name(command.FirstName, command.LastName);
            var document = new Document(command.Document);
            var email = new Email(command.Email);

            //Criar Entidade
            var customer = new Customer(name, document, email, command.Phone);

            //Validar Entidades e VOs
            AddNotifications(name.Notifications);
            AddNotifications(document.Notifications);
            AddNotifications(email.Notifications);
            AddNotifications(customer.Notifications);

            if (Invalid)
                return new CommandResult(
                    false, 
                    "Ops, algo deu errado", 
                    Notifications
                ); 

            _repository.Save(customer);

            _emailService.Send("renan@flext.com.br", "flext@flext.com.br", "Bem vindo", "Blablabla");

            //Retornar resultado para tela
            return new CommandResult(true, "Cliente cadastrado com sucesso", new 
            {
                Id = customer.Id,
                Name = name.ToString(),
                Email = email.Address
            });
        }

        public ICommandResult Handle(AddAddressCommand command)
        {
            throw new NotImplementedException();
        }
    }


}
