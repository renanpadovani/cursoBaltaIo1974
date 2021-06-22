using BaltaStore.Shared.Commands;
using FluentValidator;
using FluentValidator.Validation;

namespace BaltaStore.Domain.StoreContext.CustomerCommands.Inputs
{
    public class CreateCustomerCommand : Notifiable, ICommand
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Document { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public bool Valid()
        {
            AddNotifications( new ValidationContract()
                .HasMinLen(FirstName, 3, "FirstName", "O nome deve conter 3 caracteres")
                .HasMaxLen(FirstName, 40, "FirstName", "O nome deve conter no máximo 40 caracteres")
                .IsEmail(Email, "Email", "E-mail inválido")
                .HasLen(Document, 11, "Document", "CPF Inválido")
            );

            return IsValid;
        }
    }
}