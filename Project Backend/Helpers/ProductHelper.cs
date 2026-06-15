using Project_Backend.Models;
namespace Project_Backend.Helpers

{
    public class ProductHelper
    {
        public static bool ApplyNonImageChanges(Product target, Product source)
        {
            target.productName = source.productName;
            if (target.productName == null || target.productName.Length == 0) { return false; }
            target.ProductStyle = source.ProductStyle;
            if (target.ProductStyle == null || target.ProductStyle.Length == 0) { return false; }
            target.Price = source.Price;
            target.Stock = source.Stock;
            target.Curruency = source.Curruency;
            if (target.Curruency == null || target.Curruency.Length == 0) { return false; }
            target.Disclaimber = source.Disclaimber;
            if (target.Disclaimber == null || target.Disclaimber.Length == 0) { return false; }
            target.description = source.description;
            if (target.description == null || target.description.Length == 0) { return false; }
            target.ProductCategory = source.ProductCategory;
            if (target.ProductCategory == null || target.ProductCategory.Length == 0) { return false; }
            target.ProductSlogan = source.ProductSlogan;
            if (target.ProductSlogan == null || target.ProductSlogan.Length == 0) { return false; }
            target.Specs = source.Specs.ToDictionary(
                outer => outer.Key,
                outer => outer.Value.ToDictionary(
                    inner => inner.Key,
                    inner => inner.Value
                )
            );

            if (target.Specs == null || target.Specs.Count < 2) { return false; }
            return true;
        }

        public static bool ApplyImageChanges(Product target, Dictionary<int, string> images)
        {
            target.Images = images?.ToDictionary() ?? new();
            if (target.Images == null || target.Images.Count < 1) { return false; }
            return true;
        }
    }
}
