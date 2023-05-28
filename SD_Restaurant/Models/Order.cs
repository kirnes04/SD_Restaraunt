using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SD_Restaurant.Models;

// Класс - модель заказа
[Table("order")]
public class Order
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("user_id")]
    public int User_id { get; set; }
    [Column("status")]
    public string Status { get; set; }
    [Column("special_requests")]
    public string Special_requests { get; set; }
    [Column("created_at")]
    public DateTime Created_at { get; set; }
    [Column("updated_at")]
    public DateTime Updated_at { get; set; }
}