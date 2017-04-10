using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using TestDatabaseSeed;

namespace TestDatabaseSeed.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20170410222954_InitialCreation")]
    partial class InitialCreation
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TestDatabaseSeed.Models.EntityTable", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<int?>("Occur")
                        .HasColumnName("occur");

                    b.HasKey("Id")
                        .HasName("PK_EntityTable");

                    b.ToTable("EntityTable");
                });
        }
    }
}
