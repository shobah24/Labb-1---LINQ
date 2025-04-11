using Labb_1___LINQ.Data;
using Labb_1___LINQ.Modals;

namespace Labb_1___LINQ
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var context = new ECommerceContext())
            {
                var showAllData = new ShowProductData(context);
                while (true)
                {
                    Console.Write("1. Visa alla elektronik produkter(hög-låg pris): " +
                        "\n2. Visa alla leverantörer med ett lagersaldo under 10 enheter: " +
                        "\n3. Visa totala summan för alla ordrar under den senaste månaden:" +
                        "\n4. Visa de 3 mest sålda produkterna:" +
                        "\n5. Visa alla kategorier och antalet produkter i dem:" +
                        "\n6. Visa ordrar med ett belopp över 1000kr:" +
                        "\n7. Avsluta programmet!\n");

                    var userChoice = Console.ReadLine();
                    switch (userChoice)
                    {
                        case "1":
                            Console.WriteLine("Elektronik produkter:");
                            showAllData.ShowProducts();
                            Console.WriteLine();
                            break;
                        case "2":
                            showAllData.ShowLowStockSuppliers();
                            Console.WriteLine();
                            break;
                        case "3":
                            showAllData.ShowTotalOrderValue();
                            Console.WriteLine();
                            break;
                        case "4":
                            Console.WriteLine("De 3 mest sålda produkterna:");
                            showAllData.ShowTopSellingProducts();
                            Console.WriteLine();
                            break;
                        case "5":
                            Console.WriteLine("Kategorier och antal produkter i dem:");
                            showAllData.ShowCategoriesAndProductCount();
                            Console.WriteLine();
                            break;
                        case "6":
                            Console.WriteLine("Alla ordrar med belopp över 1000kr:");
                            showAllData.ShowOrdersOver1000();
                            Console.WriteLine();
                            break;
                        case "7":
                            return;
                        default:
                            Console.WriteLine("fel val, försök igen.");
                            break;
                    }
                }
            }
        }
    }
}
