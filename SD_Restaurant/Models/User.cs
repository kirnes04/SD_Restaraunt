using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SD_Restaurant.Models;

// Класс - модель пользователя
[Table("user")]
public class User
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("username")]
    public string Username { get; set; }
    [Column("email")]
    public string Email { get; set; }
    [Column("password_hash")]
    public string Password_hash { get; set; }
    [Column("role")]
    public string Role { get; set; }
    [Column("created_at")]
    public DateTime Created_at { get; set; }
    [Column("updated_at")]
    public DateTime Updated_at { get; set; }
}