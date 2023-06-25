using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Models._Account_;
using tyenda_backend.App.Models._Branches_;
using tyenda_backend.App.Models._Cart_;
using tyenda_backend.App.Models._Category_;
using tyenda_backend.App.Models._City_;
using tyenda_backend.App.Models._Country_;
using tyenda_backend.App.Models._Customer_;
using tyenda_backend.App.Models._Follower_;
using tyenda_backend.App.Models._Item_;
using tyenda_backend.App.Models._ItemColor_;
using tyenda_backend.App.Models._ItemImage_;
using tyenda_backend.App.Models._Like_;
using tyenda_backend.App.Models._Order_;
using tyenda_backend.App.Models._Role_;
using tyenda_backend.App.Models._Session_;
using tyenda_backend.App.Models._Size_;
using tyenda_backend.App.Models._Store_;
using tyenda_backend.App.Models._Store_Category_;
using tyenda_backend.App.Models._Token_;
using TyendaBackend.App.Models._Account_;
using Color = tyenda_backend.App.Models._Color_.Color;

namespace tyenda_backend.App.Context
{
    public class TyendaContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<StoreCategory> StoreCategories { get; set; }

        public DbSet<Color> Colors { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<ItemImage> ItemImages { get; set; }
        public DbSet<ItemColor> ItemColors { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Follower> Followers { get; set; }

        public DbSet<Cart> Carts { get; set; }
        
        public TyendaContext(DbContextOptions<TyendaContext> opt) : base(opt) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Account>()
                .HasOne(account => account.Role)
                .WithMany(role => role.Accounts)
                .HasForeignKey(account => account.RoleId);

            modelBuilder.Entity<Token>()
                .HasOne(token => token.Account)
                .WithMany(account => account.Tokens)
                .HasForeignKey(token => token.AccountId);
            
            modelBuilder.Entity<Session>()
                .HasOne(session => session.Account)
                .WithOne(account => account.Session)
                .HasForeignKey<Session>(session => session.AccountId);

            modelBuilder.Entity<City>()
                .HasOne(city => city.Country)
                .WithMany(country => country.Cities)
                .HasForeignKey(city => city.CountryId);

            modelBuilder.Entity<Customer>()
                .HasOne(customer => customer.Account)
                .WithOne(account => account.Customer)
                .HasForeignKey<Customer>(customer => customer.AccountId);

            modelBuilder.Entity<Store>()
                .HasOne(store => store.Account)
                .WithOne(account => account.Store)
                .HasForeignKey<Store>(store => store.AccountId);

            modelBuilder.Entity<Branch>().HasKey(branch => new {branch.StoreId, branch.CityId});
            modelBuilder.Entity<Branch>()
                .HasOne(branch => branch.Store)
                .WithMany(store => store.Branches)
                .HasForeignKey(branch => branch.StoreId);
            modelBuilder.Entity<Branch>()
                .HasOne(branch => branch.City)
                .WithMany(city => city.Branches)
                .HasForeignKey(branch => branch.CityId);

            modelBuilder.Entity<StoreCategory>().HasKey(prop => new {prop.StoreId, prop.CategoryId});
            modelBuilder.Entity<StoreCategory>()
                .HasOne(storeCategory => storeCategory.Store)
                .WithMany(store => store.Categories)
                .HasForeignKey(storeCategory => storeCategory.StoreId);
            modelBuilder.Entity<StoreCategory>()
                .HasOne(storeCategory => storeCategory.Category)
                .WithMany(category => category.Stores)
                .HasForeignKey(storeCategory => storeCategory.CategoryId);

            modelBuilder.Entity<Item>()
                .HasOne(item => item.Store)
                .WithMany(store => store.Items)
                .HasForeignKey(item => item.StoreId);

            modelBuilder.Entity<Size>()
                .HasOne(size => size.Item)
                .WithMany(item => item.Sizes)
                .HasForeignKey(size => size.ItemId);
            
            modelBuilder.Entity<ItemImage>()
                .HasOne(itemImage => itemImage.Item)
                .WithMany(item => item.ItemImages)
                .HasForeignKey(itemImage => itemImage.ItemId);

            modelBuilder.Entity<ItemColor>().HasKey(itemColor => new {itemColor.ItemId, itemColor.ColorId});
            modelBuilder.Entity<ItemColor>()
                .HasOne(itemColor => itemColor.Item)
                .WithMany(item => item.ItemColors)
                .HasForeignKey(itemColor => itemColor.ItemId);
            modelBuilder.Entity<ItemColor>()
                .HasOne(itemColor => itemColor.Color)
                .WithMany(color => color.ItemColors)
                .HasForeignKey(itemColor => itemColor.ColorId);

            modelBuilder.Entity<Order>()
                .HasOne(order => order.City)
                .WithMany(city => city.Orders)
                .HasForeignKey(order => order.CityId);
            modelBuilder.Entity<Order>()
                .HasOne(order => order.Customer)
                .WithMany(customer => customer.Orders)
                .HasForeignKey(order => order.CustomerId);
            modelBuilder.Entity<Order>()
                .HasOne(order => order.Item)
                .WithMany(customer => customer.Orders)
                .HasForeignKey(order => order.ItemId);

            modelBuilder.Entity<Like>().HasKey(like => new {like.ItemId, like.CustomerId});
            modelBuilder.Entity<Like>()
                .HasOne(like => like.Item)
                .WithMany(item => item.Likes)
                .HasForeignKey(like => like.ItemId);
            modelBuilder.Entity<Like>()
                .HasOne(like => like.Customer)
                .WithMany(item => item.Likes)
                .HasForeignKey(like => like.CustomerId);

            modelBuilder.Entity<Follower>().HasKey(follower => new {follower.StoreId, follower.CustomerId});
            modelBuilder.Entity<Follower>()
                .HasOne(follower => follower.Store)
                .WithMany(store => store.Followers)
                .HasForeignKey(follower => follower.StoreId);
            modelBuilder.Entity<Follower>()
                .HasOne(follower => follower.Customer)
                .WithMany(store => store.Followers)
                .HasForeignKey(follower => follower.CustomerId);

            modelBuilder.Entity<Cart>()
                .HasOne(cart => cart.Customer)
                .WithMany(customer => customer.Carts)
                .HasForeignKey(cart => cart.CustomerId);
            modelBuilder.Entity<Cart>()
                .HasOne(cart => cart.Item)
                .WithMany(customer => customer.Carts)
                .HasForeignKey(cart => cart.ItemId);
            modelBuilder.Entity<Cart>()
                .HasOne(cart => cart.Store)
                .WithMany(customer => customer.Carts)
                .HasForeignKey(cart => cart.StoreId);



        }
    }
}