using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Proje.Models.MVVM
{
    public class MainPageModel
    {
        public List<Product> SliderProducts { get; set; }
        public Product ProductOfDay { get; set; }
        public List<Product> NewProducts { get; set; }
        public List<Product> SpecialProducts { get; set; }
        public List<Product> DiscountedProducts { get; set; }
        public List<Product> HighlightedProducts { get; set; }
        public List<Product> TopsellerProducts { get; set; }
        public List<Product> StarProducts { get; set; }
        public List<Product> FeaturedProducts { get; set; }
        public List<Product> NotableProducts { get; set; }
        public Product ProductDetails { get; set; }
        public string CategoryName { get; set; }
        public string BrandName { get; set; }
        public List<Product> RelatedProducts { get; set; }
    }
}
