using System;
using System.Collections.Generic;

namespace Test.Model;

public partial class Product
{
    public string ProductId { get; set; } = null!;

    public int? TypeBootsId { get; set; }

    public string? Unit { get; set; }

    public int? Price { get; set; }

    public int? SupplierId { get; set; }

    public int? ManufacturerId { get; set; }

    public int? TypeProductId { get; set; }

    public int? Discount { get; set; }

    public int? Count { get; set; }

    public string? Title { get; set; }

    public byte[]? Photo { get; set; }

    public virtual Manufacturer? Manufacturer { get; set; }

    public virtual ICollection<OredersItem> OredersItems { get; set; } = new List<OredersItem>();

    public virtual Supplier? Supplier { get; set; }

    public virtual TypeBoot? TypeBoots { get; set; }

    public virtual TypeProduct? TypeProduct { get; set; }

    public string MaxTitle => TypeProduct.Title + "    |   " + TypeBoots.Title;
    public string CardTitle => "Описание товара: " + Title;
    public string CardManufacturer => "Производитель: " + Manufacturer.Title;
    public string CardSupplier => "Поставшик: " + Supplier.Title;
    public string CadrPrice => Discount > 0 ? Math.Round(Convert.ToDecimal((Price - (Price * (0.01 * Discount)))), 2).ToString() : "";
    public string CardUnit => "Единицы измерения: " + Unit;
    public string CardCount => "Количество на складе: " + Count;
    public object CardPhoto => Photo != null ? Photo : "/Data/picture.png";

}
