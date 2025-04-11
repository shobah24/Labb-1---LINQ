using Labb_1___LINQ.Modals;
using Microsoft.EntityFrameworkCore;

namespace Labb_1___LINQ.Data
{
    public class ShowProductData
    {
        private readonly ECommerceContext _context;

        public ShowProductData(ECommerceContext context)
        {
            _context = context;   // skapas för att kunna hämta data från databasen i metoderna istället för att spaka instans av ECommerceContext i varje metod.
        }
        // Visa alla elektronik produkter hög till låg pris
        public void ShowProducts()
        {
            var products = _context.Products
                .Where(c => c.Category.Name == "Electronics")
                .OrderByDescending(p => p.Price)
                .ToList();
            foreach (var product in products)
            {
                Console.WriteLine($"Produkt: {product.Name}, pris: {product.Price:C}");
            }
        }

        // Visa alla leverantörer som har produkter med lagersaldo under 10 enheter
        public void ShowLowStockSuppliers()
        {
            var lowStockSuppliers = _context.Suppliers
                .Where(s => s.Products
                .Any(p => p.StockQuantity < 10))
                .Select(s => new
                {
                    SupName = s.Name,
                    SupEmail = s.Email,
                    count = s.Products.Count(p => p.StockQuantity < 10)      // ger oss antal produkter med lagersaldo under 10
                })
                .OrderByDescending(s => s.count)
                .ToList();
            foreach (var supplier in lowStockSuppliers)
            {
                Console.WriteLine($"Leverantör: {supplier.SupName}, Kontakt: {supplier.SupEmail}, Antal produkter med under 10 i lagersaldo: {supplier.count}");
            }
        }

        // Beräkna det totala ordervärdet för alla ordrar gjorda under den senaste månaden
        public void ShowTotalOrderValue()
        {
            var start = DateTime.Now.AddMonths(-1);
            var now = DateTime.Now;

            var orderValue = _context.Orders 
                .Where(o => o.OrderDate >= start && o.OrderDate <= now)
                .Sum(o => o.TotalAmount);
            //Console.WriteLine($"Från:{start:d} till:{now:d}"); // test för att se att datum räknas rätt
            Console.WriteLine($"Det totala ordervärdet under den senaste månaden: {orderValue:C}");

        }
        // Hitta de 3 mest sålda produkterna baserat på OrderDetail-data
        public void ShowTopSellingProducts()
        {
            var topSelling = _context.OrderDetails
                .GroupBy(o => o.Product)
                .Select(g => new
                {
                    Product = g.Key,
                    quantity = g.Sum(o => o.Quantity)
                })
                .OrderByDescending(o => o.quantity)
                .Take(3)   // används för att hämta de 3 mest sålda produkterna efter sorteringen i listan
                .ToList();
            foreach (var pro in topSelling)
            {
                Console.WriteLine($"Produkt: {pro.Product.Name}, antalet sålda enheter: {pro.quantity}");
            }
        }
        // Lista alla kategorier och antalet produkter i varje kategori
        public void ShowCategoriesAndProductCount()
        {
            var catAndPro = _context.Categories
                .Select(c => new
                {
                    CategoryName = c.Name,
                    ProductCount = c.Products.Count()
                })
                .ToList();

            foreach (var item in catAndPro)
            {
                Console.WriteLine($"Kategori: {item.CategoryName}, antalet produkter: {item.ProductCount}");
            }

        }
        // Hämta alla ordrar med tillhörande kunduppgifter och orderdetaljer där totalbeloppet överstiger 1000 kr
        public void ShowOrdersOver1000()
        {
            var orderInfo = _context.Orders
                .Where(o => o.TotalAmount > 1000)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .Include(o => o.Customer)
                .Select(c => new    
                {
                    CustomerName = c.Customer.Name,
                    CustomerEmail = c.Customer.Email,
                    CustomerPhone = c.Customer.Phone,
                    CustomerAddress = c.Customer.Address,
                    orderDate = c.OrderDate,
                    totalAmount = c.TotalAmount,

                    OrderDetails = c.OrderDetails.Select(od => new
                    {
                        od.Product,
                        od.Quantity,
                        od.UnitPrice
                    })
                })
                .ToList();
            foreach (var order in orderInfo)
            {
                Console.WriteLine($"Order beställt: {order.orderDate:d}, " +
                    $"Summa: {order.totalAmount:C} " +
                    $"Kund:\n{order.CustomerName}, " +
                    $"Mail: {order.CustomerEmail}, " +
                    $"Telefon: {order.CustomerPhone}, " +
                    $"Adress: {order.CustomerAddress}");
                
                foreach (var od in order.OrderDetails)
                {
                    Console.WriteLine($"Produkt: {od.Product.Name}, " +
                        $"Antal: {od.Quantity}, " +
                        $"Pris: {od.UnitPrice:C}");
                }
                Console.WriteLine();
            }
        }
    }
}
