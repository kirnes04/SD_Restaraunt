using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SD_Restaurant.Models;

// Класс - модель блюда
[Table("dish")]
public class Dish
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("name")]
    public string Name { get; set; }
    [Column("description")]
    public string Description { get; set; }
    [Column("price")]
    public decimal Price { get; set; }
    [Column("quantity")]
    public int Quantity { get; set; }
    [Column("is_available")]
    public bool Is_available { get; set; }
    [Column("created_at")]
    public DateTime Created_at { get; set; }
    [Column("updated_at")]
    public DateTime Updated_at { get; set; }

    public override string ToString()
    {
        return $"Name = {Name}, Price = {Price}, {Quantity} items avaliable, Description: {Description}";
    }
}