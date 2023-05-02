using Ecommerce_API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_API.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MenuItem>().HasData(
                new MenuItem
                {
                    Id = 1,
                    Name = "Spring Roll",
                    Description = "Fusc tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                    Image = "https://dvecommerceimages.blob.core.windows.net/dvecommerce/spring roll.jpg",
                    Price = 7.99,
                    Category = "Appetizer",
                    SpecialTag = ""
                }, new MenuItem
                {
                    Id = 2,
                    Name = "Idli",
                    Description = "Fusc tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                    Image = "https://dvecommerceimages.blob.core.windows.net/dvecommerce/idli.jpg",
                    Price = 8.99,
                    Category = "Appetizer",
                    SpecialTag = ""
                }, new MenuItem
                {
                    Id = 3,
                    Name = "Panu Puri",
                    Description = "Fusc tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                    Image = "https://dvecommerceimages.blob.core.windows.net/dvecommerce/pani puri.jpg",
                    Price = 8.99,
                    Category = "Appetizer",
                    SpecialTag = "Best Seller"
                }, new MenuItem
                {
                    Id = 4,
                    Name = "Hakka Noodles",
                    Description = "Fusc tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                    Image = "https://dvecommerceimages.blob.core.windows.net/dvecommerce/hakka noodles.jpg",
                    Price = 10.99,
                    Category = "Entrée",
                    SpecialTag = ""
                }, new MenuItem
                {
                    Id = 5,
                    Name = "Malai Kofta",
                    Description = "Fusc tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                    Image = "https://dvecommerceimages.blob.core.windows.net/dvecommerce/malai kofta.jpg",
                    Price = 12.99,
                    Category = "Entrée",
                    SpecialTag = "Top Rated"
                }, new MenuItem
                {
                    Id = 6,
                    Name = "Paneer Pizza",
                    Description = "Fusc tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                    Image = "https://dvecommerceimages.blob.core.windows.net/dvecommerce/paneer pizza.jpg",
                    Price = 11.99,
                    Category = "Entrée",
                    SpecialTag = ""
                }, new MenuItem
                {
                    Id = 7,
                    Name = "Paneer Tikka",
                    Description = "Fusc tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                    Image = "https://dvecommerceimages.blob.core.windows.net/dvecommerce/paneer tikka.jpg",
                    Price = 13.99,
                    Category = "Entrée",
                    SpecialTag = "Chef's Special"
                }, new MenuItem
                {
                    Id = 8,
                    Name = "Carrot Love",
                    Description = "Fusc tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                    Image = "https://dvecommerceimages.blob.core.windows.net/dvecommerce/carrot love.jpg",
                    Price = 4.99,
                    Category = "Dessert",
                    SpecialTag = ""
                }, new MenuItem
                {
                    Id = 9,
                    Name = "Rasmalai",
                    Description = "Fusc tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                    Image = "https://dvecommerceimages.blob.core.windows.net/dvecommerce/rasmalai.jpg",
                    Price = 4.99,
                    Category = "Dessert",
                    SpecialTag = "Chef's Special"
                }, new MenuItem
                {
                    Id = 10,
                    Name = "Sweet Rolls",
                    Description = "Fusc tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                    Image = "https://dvecommerceimages.blob.core.windows.net/dvecommerce/sweet rolls.jpg",
                    Price = 3.99,
                    Category = "Dessert",
                    SpecialTag = "Top Rated"
                });
        }
    }
}
