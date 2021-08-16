using FluentValidation;
using RoleBased.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RoleBased.API.Validator
{
     public class CustomerValidator : AbstractValidator<RegisterModel>
    {
        public CustomerValidator()
        {
            RuleFor(c => c.FirstName).NotEmpty().WithMessage("Lütfen adınızı boş geçmeyiniz.");
            RuleFor(c => c.LastName).NotEmpty().WithMessage("Lütfen soyadı boş geçmeyiniz.");

            RuleFor(s => s.Email).NotEmpty().WithMessage("Email address is required")
                         .EmailAddress().WithMessage("A valid email is required");

            RuleFor(c => c.PhoneNumber).NotEmpty().WithMessage("Lütfen Telefon numarasını boş geçmeyiniz.");

            RuleFor(c => c.UserName).NotEmpty().WithMessage("Lütfen kullanıcı adını boş geçmeyiniz.");
            RuleFor(x => x.UserName).MinimumLength(3).WithMessage("Lütfen kullanıcı adını 3 karakterden kısa girmeyiniz."); ;
            RuleFor(x => x.UserName).MaximumLength(20).WithMessage("Lütfen kullanıcı adını 20 karakterden kısa girmeyiniz."); ;

            RuleFor(c => c.Password).NotEmpty().WithMessage("Lütfen şifreyi boş geçmeyiniz boş geçmeyiniz.");
            RuleFor(x => x.Password)
            .Length(5, 15)
            .Must(x => HasValidPassword(x)).WithMessage("Lütfen şifreyi valid bir şifre giriniz.");

            RuleFor(c => c.Role).NotEmpty().Must(c => c.Equals("Yönetici") || c.Equals("Kullanıcı")).WithMessage("Lütfen 'Yönetici' veye 'Kullanıcı'rolleriniden birini seçiniz "); ;

        }


        private bool HasValidPassword(string pw)
        {
            var lowercase = new Regex("[a-z]+");
            var uppercase = new Regex("[A-Z]+");
            var digit = new Regex("(\\d)+");
            var symbol = new Regex("(\\W)+");

            return (lowercase.IsMatch(pw) && uppercase.IsMatch(pw) && digit.IsMatch(pw) && symbol.IsMatch(pw));
        }

    }
}
