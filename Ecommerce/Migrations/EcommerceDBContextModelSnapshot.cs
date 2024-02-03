﻿// <auto-generated />
using System;
using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Ecommerce.Migrations
{
    [DbContext(typeof(EcommerceDBContext))]
    partial class EcommerceDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Ecommerce.Models.Adress", b =>
                {
                    b.Property<int>("AddressID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AddressID"));

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("CITY");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("COUNTRY");

                    b.Property<int>("CustomerID")
                        .HasColumnType("int")
                        .HasColumnName("CUSTOMERID");

                    b.Property<bool?>("Isactive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasColumnName("ISACTIVE")
                        .HasDefaultValueSql("((1))");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("STATE");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .IsUnicode(false)
                        .HasColumnType("varchar(1000)")
                        .HasColumnName("Street");

                    b.Property<string>("ZipCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("ZIPCODE");

                    b.HasKey("AddressID");

                    b.ToTable("Adresses");
                });

            modelBuilder.Entity("Ecommerce.Models.Cart", b =>
                {
                    b.Property<int>("CartID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CartID"));

                    b.Property<int>("CustomerID")
                        .HasColumnType("int")
                        .HasColumnName("CustomerId");

                    b.Property<int>("ProductID")
                        .HasColumnType("int")
                        .HasColumnName("ProductId");

                    b.Property<int>("Quantity")
                        .HasColumnType("int")
                        .HasColumnName("Quantity");

                    b.HasKey("CartID");

                    b.ToTable("Cart");
                });

            modelBuilder.Entity("Ecommerce.Models.Order", b =>
                {
                    b.Property<int>("OrderID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderID"));

                    b.Property<int>("CustomerID")
                        .HasColumnType("int")
                        .HasColumnName("CUSTOMER_ID");

                    b.Property<bool?>("Isactive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasColumnName("ISACTIVE")
                        .HasDefaultValueSql("((1))");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("date")
                        .HasColumnName("ORDERDATE");

                    b.Property<DateTime>("ShipDate")
                        .HasColumnType("date")
                        .HasColumnName("SHIPDATE");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("NAME");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("decimal(18, 2)")
                        .HasColumnName("TOTALAMOUNT");

                    b.HasKey("OrderID");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("Ecommerce.Models.OrderItem", b =>
                {
                    b.Property<int>("OrderItemID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ORDERITEMID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderItemID"));

                    b.Property<string>("ImageURL")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("ImageURL");

                    b.Property<bool?>("Isactive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasColumnName("ISACTIVE")
                        .HasDefaultValueSql("((1))");

                    b.Property<int>("OrderId")
                        .HasColumnType("int")
                        .HasColumnName("OrderID");

                    b.Property<int>("ProductID")
                        .HasColumnType("int")
                        .HasColumnName("PRODUCTID");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("ProductName");

                    b.Property<int>("Quantity")
                        .HasColumnType("int")
                        .HasColumnName("Quantity");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(18, 2)")
                        .HasColumnName("TOTALPRICE");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("decimal(18, 2)")
                        .HasColumnName("UNITPRICE");

                    b.Property<int>("signupId")
                        .HasColumnType("int")
                        .HasColumnName("SignupId");

                    b.HasKey("OrderItemID");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("Ecommerce.Models.Product", b =>
                {
                    b.Property<int>("ProductID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductID"));

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("Category");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .IsUnicode(false)
                        .HasColumnType("varchar(1000)")
                        .HasColumnName("Description");

                    b.Property<string>("ImageURL")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .IsUnicode(false)
                        .HasColumnType("varchar(1000)")
                        .HasColumnName("ImageURL");

                    b.Property<bool?>("Isactive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasColumnName("ISACTIVE")
                        .HasDefaultValueSql("((1))");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("NAME");

                    b.Property<decimal>("Price")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("decimal(18, 2)")
                        .HasColumnName("Price");

                    b.Property<int>("Quantity")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("int")
                        .HasColumnName("Quantity");

                    b.HasKey("ProductID");

                    b.ToTable("Products", (string)null);
                });

            modelBuilder.Entity("Ecommerce.Models.Review", b =>
                {
                    b.Property<int>("ReviewID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ReviewID"));

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .IsUnicode(false)
                        .HasColumnType("varchar(1000)")
                        .HasColumnName("Comment");

                    b.Property<int>("CustomerID")
                        .HasColumnType("int")
                        .HasColumnName("CUSTOMERID");

                    b.Property<bool?>("Isactive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasColumnName("ISACTIVE")
                        .HasDefaultValueSql("((1))");

                    b.Property<int>("ProductID")
                        .HasColumnType("int")
                        .HasColumnName("PRODUCTID");

                    b.Property<int>("Rating")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("int")
                        .HasColumnName("Rating");

                    b.HasKey("ReviewID");

                    b.ToTable("Review");
                });

            modelBuilder.Entity("Ecommerce.Models.Signup", b =>
                {
                    b.Property<int>("Signupid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("SIGNUPID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Signupid"));

                    b.Property<string>("ConfirmPassword")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("EMAIL");

                    b.Property<bool?>("Isactive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasColumnName("ISACTIVE")
                        .HasDefaultValueSql("((1))");

                    b.Property<long>("Mobile")
                        .HasColumnType("bigint")
                        .HasColumnName("MOBILE");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("NAME");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("PASSWORD");

                    b.HasKey("Signupid");

                    b.ToTable("SIGNUP", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
