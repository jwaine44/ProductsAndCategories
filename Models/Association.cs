#pragma warning disable CS8618
namespace ProductsAndCategories.Models;
using System.ComponentModel.DataAnnotations;

public class Association
{
    [Key]
    public int AssociationId {get;set;}
    public int ProductId {get;set;}
    public int CategoryId {get;set;}
    public Product? Product {get;set;}
    public Category? Category {get;set;}
}