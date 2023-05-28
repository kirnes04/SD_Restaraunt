using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SD_Restaurant.Models;

// Класс - модель сессии
[Table("session")]
public class Session
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("user_id")]
    public int User_id { get; set; }
    
    [Column("session_token")]
    public string Session_token { get; set; }
    
    [Column("expires_at")]
    public DateTime Expires_at { get; set; }
}