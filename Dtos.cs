namespace users
{
    public class Dtos
    {

        //minden adat lekérés
        public record UserDto(Guid Id, string Name, string Email, int Age, DateTimeOffset Created);

        //felh. letrehozas
        public record CreateUserDto(string Name, string Email, int Age);

        //felh. adat módosítás
        public record UpdateUserDto(string Name, string Email, int Age);

    }
}
