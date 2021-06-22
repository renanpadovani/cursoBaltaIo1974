using System;
using FluentValidator;
using FluentValidator.Validation;

namespace BaltaStore.Domain.StoreContext.ValueObjects
{
    public class Document : Notifiable
    {
        public Document(string number)
        {
            Number = number;

            AddNotifications(new ValidationContract()
                .Requires()
                .IsTrue(Validate(Number),"Document","CPF Invalido")
            );
        }
        public string Number { get; private set; }

        public override string ToString()
        {
            return Number;
        }

        public bool Validate(string cpf)
        {
            if (cpf.Length == 11)
                return true;
            else
                return false;
        }
    }
}