namespace SD_Restaurant.Models;

// Класс - модель пользователя для регистрации
public class UserRegister
{
    public string Username { get; set; }
    
    public string Email { get; init; }

    public string Password { get; init; }
}