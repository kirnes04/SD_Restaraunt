using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SD_Restaurant.Models;

[Table("order_dish")]
public class OrderDish
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("order_id")]
    public int Order_id { get; set; }
    [Column("dish_id")]
    public int Dish_id { get; set; }
    [Column("quantity")]
    public int Quantity { get; set; }
    [Column("price")]
    public decimal Price { get; set; }
}