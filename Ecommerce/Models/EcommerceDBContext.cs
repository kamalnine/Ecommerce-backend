﻿using Microsoft.EntityFrameworkCore;


namespace Ecommerce.Models
{

    public partial class EcommerceDBContext : DbContext
    {
        public EcommerceDBContext()
        {
        }

        public EcommerceDBContext(DbContextOptions<EcommerceDBContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<OrderItems> OrderItems { get; set; }
      

        public virtual DbSet<Review> Review { get; set; }
        public virtual DbSet<Adress> Adresses { get; set; }

        public virtual DbSet<Signup> Signup { get; set; }

        public virtual DbSet<Wishlist> Wishlist { get; set; }

        public virtual DbSet<Cart> Cart { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Products");
                entity.HasKey(e => e.ProductID);
                entity.Property(e => e.ProductID).HasColumnName("ID");

                entity.Property(e => e.Isactive)
                    .HasColumnName("ISACTIVE")
                    .HasDefaultValueSql("((1))");
                entity.HasQueryFilter(t => t.Isactive == true);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("NAME")
                    .HasMaxLength(100)
                    .IsUnicode(false);


                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("Description")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Price)
                .IsRequired()
                .HasColumnName("Price")
                .HasMaxLength(100)
                .HasColumnType("decimal(18, 2)")
                .IsUnicode(false);

                entity.Property(e => e.Quantity)
                .IsRequired()
                .HasColumnName("Quantity")
                .HasMaxLength(100)
                .IsUnicode(false);

                entity.Property(e => e.Category)
                .IsRequired()
                .HasColumnName("Category")
                .HasMaxLength(100)
                .IsUnicode(false);

                entity.Property(e => e.ImageURL)
                .IsRequired()
                .HasColumnName("ImageURL")
                .HasMaxLength(1000)
                .IsUnicode(false);

                
               
            });

            modelBuilder.Entity<Wishlist>(entity =>
            {
                entity.ToTable("Wishlist");

                entity.HasKey(e => e.WishlistID);
                entity.Property(e => e.WishlistID).HasColumnName("ID");
                entity.Property(e => e.ProductID).IsRequired();
                entity.Property(e=>e.CustomerID).IsRequired();


                entity.Property(e => e.Isactive)
                    .HasColumnName("ISACTIVE")
                    .HasDefaultValueSql("((1))");
                entity.HasQueryFilter(t => t.Isactive == true);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("NAME")
                    .HasMaxLength(100)
                    .IsUnicode(false);


                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("Description")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Price)
                .IsRequired()
                .HasColumnName("Price")
                .HasMaxLength(100)
                .HasColumnType("decimal(18, 2)")
                .IsUnicode(false);

                entity.Property(e => e.Quantity)
                .IsRequired()
                .HasColumnName("Quantity")
                .HasMaxLength(100)
                .IsUnicode(false);

                entity.Property(e => e.Category)
                .IsRequired()
                .HasColumnName("Category")
                .HasMaxLength(100)
                .IsUnicode(false);

                entity.Property(e => e.ImageURL)
                .IsRequired()
                .HasColumnName("ImageURL")
                .HasMaxLength(1000)
                .IsUnicode(false);



            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.OrderID).HasColumnName("ID");

                entity.Property(e => e.CustomerID).HasColumnName("CUSTOMER_ID");

                entity.Property(e => e.OrderDate)
                    .HasColumnName("ORDERDATE")
                    .HasColumnType("date");

                entity.Property(e => e.ShipDate)
                    .HasColumnName("SHIPDATE")
                    .HasColumnType("date");

                entity.Property(e => e.Isactive)
                    .HasColumnName("ISACTIVE")
                    .HasDefaultValueSql("((1))");
                entity.HasQueryFilter(t => t.Isactive == true);

                entity.Property(e => e.TotalAmount).HasColumnName("TOTALAMOUNT").HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("NAME")
                    .HasMaxLength(100)
                    .IsUnicode(false);

              




            });

            modelBuilder.Entity<OrderItems>(entity =>
            {
                entity.Property(e => e.OrderItemID).HasColumnName("ORDERITEMID");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");
                entity.Property(e => e.signupId).HasColumnName("SignupId");

                entity.Property(e => e.ProductID).HasColumnName("PRODUCTID");

              

                entity.Property(e => e.Quantity).HasColumnName("Quantity");

                entity.Property(e => e.UnitPrice).HasColumnName("UNITPRICE").HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TotalPrice).HasColumnName("TOTALPRICE").HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ImageURL).HasColumnName("ImageURL");

                entity.Property(e => e.ProductName).HasColumnName("ProductName");

                entity.Property(e => e.Variant).HasColumnName("Variant");

                entity.Property(e => e.OrderDate)
                   .HasColumnName("ORDERDATE")
                   .HasColumnType("date");

                entity.Property(e => e.ShipDate)
                    .HasColumnName("SHIPDATE")
                    .HasColumnType("date");

                entity.Property(e => e.Isactive)
                .HasColumnName("ISACTIVE")
                .HasDefaultValueSql("((1))");
                entity.HasQueryFilter(t => t.Isactive == true);
            });

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.Property(e => e.CartID).HasColumnName("CARTID");

              
                entity.Property(e => e.CustomerID).HasColumnName("SignupId");

                entity.Property(e => e.ProductID).HasColumnName("PRODUCTID");
               
                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("Description")
                    .HasMaxLength(1000)
                    .IsUnicode(false);


                entity.Property(e => e.Quantity).HasColumnName("Quantity");

                entity.Property(e => e.Price).HasColumnName("UNITPRICE").HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TotalPrice).HasColumnName("TOTALPRICE").HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ImageURL).HasColumnName("ImageURL");

                entity.Property(e => e.Name).HasColumnName("ProductName");

                entity.Property(e => e.Variant).HasColumnName("Variant");
                 entity.Property(e => e.Category)
                .IsRequired()
                .HasColumnName("Category")
                .HasMaxLength(100)
                .IsUnicode(false);

                entity.Property(e => e.Isactive)
                .HasColumnName("ISACTIVE")
                .HasDefaultValueSql("((1))");
                entity.HasQueryFilter(t => t.Isactive == true);
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.Property(e => e.ReviewID).HasColumnName("ID");


                entity.Property(e => e.ProductID).HasColumnName("PRODUCTID");

                entity.Property(e => e.CustomerID).HasColumnName("CUSTOMERID");

                entity.Property(e => e.Isactive)
                    .HasColumnName("ISACTIVE")
                    .HasDefaultValueSql("((1))");
                entity.HasQueryFilter(t => t.Isactive == true);

                entity.Property(e => e.Comment)
                    .IsRequired()
                    .HasColumnName("Comment")
                    .HasMaxLength(1000)
                    .IsUnicode(false);


             

                entity.Property(e => e.Rating)
                .IsRequired()
                .HasColumnName("Rating")
                .HasMaxLength(100)
                .IsUnicode(false);

                



            });

            modelBuilder.Entity<Adress>(entity =>
            {
                entity.HasKey(e => e.AddressID);

                

                entity.Property(e => e.CustomerID).HasColumnName("CUSTOMERID");


              

                entity.Property(e => e.Isactive)
                    .HasColumnName("ISACTIVE")
                    .HasDefaultValueSql("((1))");
                entity.HasQueryFilter(t => t.Isactive == true);

                entity.Property(e => e.Street)
                    .IsRequired()
                    .HasColumnName("Street")
                    .HasMaxLength(1000)
                    .IsUnicode(false);




                entity.Property(e => e.City)
                .IsRequired()
                .HasColumnName("CITY")
                .HasMaxLength(100)
                .IsUnicode(false);

                entity.Property(e => e.State)
              .IsRequired()
              .HasColumnName("STATE")
              .HasMaxLength(100)
              .IsUnicode(false);

                entity.Property(e => e.Country)
           .IsRequired()
           .HasColumnName("COUNTRY")
           .HasMaxLength(100)
           .IsUnicode(false);


                entity.Property(e => e.ZipCode).HasColumnName("ZIPCODE");



            });

          

            modelBuilder.Entity<Signup>(entity =>
            {
                entity.ToTable("SIGNUP");

                entity.Property(e => e.Signupid).HasColumnName("SIGNUPID");

                entity.Property(e => e.ConfirmPassword)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("EMAIL")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Isactive)
                    .HasColumnName("ISACTIVE")
                    .HasDefaultValueSql("((1))");
                entity.HasQueryFilter(t => t.Isactive == true);

                entity.Property(e => e.Mobile).HasColumnName("MOBILE");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("NAME")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("PASSWORD")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });




            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
