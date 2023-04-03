using Microsoft.AspNetCore.Identity;

namespace Project.Identity.Extensions
{
    public  class CustomIdentityErrorDescriber :IdentityErrorDescriber
    {
        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError()
            {
                Code = "PasswordTooShort",
                Description=$"Parola {length} Karakterden Kısa Olamaz."
            };
        }
        public override IdentityError PasswordRequiresLower()
        {
            return new IdentityError()
            {
                Code = "PasswordRequiresLower",
                Description = $"Parolada En Az 1 Küçük Harf Olmalıdır."
            };
        }
        public override IdentityError PasswordRequiresUpper()
        {
            return new IdentityError()
            {
                Code= "PasswordRequiresUpper",
                Description= "Parolada En Az 1 Büyük Harf Olmalıdır."
            };
        }
        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError()
            {
                Code = "DuplicateUserName",
                Description = $"{userName} Kullanıcı Adı Başkası Tarafından Kullanılmakta."
            };
        }
        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new IdentityError()
            {
                Code = "PasswordRequiresNonAlphanumeric",
                Description = "Parola Özel Karakter İçermelidir (!,',+,_,(,),/,?,.)"
            };
        }
    }
}
