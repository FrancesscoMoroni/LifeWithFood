using System;
using System.Collections.Generic;
using LifeWithFood.Models;
using Microsoft.EntityFrameworkCore;

namespace LifeWithFood.Data;

public partial class LifeWithFoodDbContext : DbContext
{
    public LifeWithFoodDbContext()
    {
    }

    public LifeWithFoodDbContext(DbContextOptions<LifeWithFoodDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Grocery> Groceries { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<ListsOfIngredient> ListsOfIngredients { get; set; }

    public virtual DbSet<OwnedGrocery> OwnedGroceries { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<Recipe> Recipes { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=BARTK; Database=LifeWithFoodDB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Grocery>(entity =>
        {
            entity.HasKey(e => e.IdFoodItem).HasName("PK__Grocerie__09949FB34AAB3941");

            entity.Property(e => e.IdFoodItem).HasColumnName("idFoodItem");
            entity.Property(e => e.Name)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Unit)
                .IsRequired()
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("unit");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => new { e.IdImage, e.RecipesIdRecipe }).HasName("PK__Images__10346276D35676A0");

            entity.HasIndex(e => e.RecipesIdRecipe, "fk_Images_Recipes1_idx");

            entity.Property(e => e.IdImage)
                .ValueGeneratedOnAdd()
                .HasColumnName("idImage");
            entity.Property(e => e.RecipesIdRecipe).HasColumnName("Recipes_idRecipe");
            entity.Property(e => e.Position)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("position");

            entity.HasOne(d => d.RecipesIdRecipeNavigation).WithMany(p => p.Images)
                .HasForeignKey(d => d.RecipesIdRecipe)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Images_Recipes1");
        });

        modelBuilder.Entity<ListsOfIngredient>(entity =>
        {
            entity.HasKey(e => new { e.GroceriesIdFoodItem, e.RecipesIdRecipe }).HasName("PK__ListsOfI__1A13F7DCE7A4DB55");

            entity.HasIndex(e => e.GroceriesIdFoodItem, "fk_ListsOfIngredients_Groceries1_idx");

            entity.HasIndex(e => e.RecipesIdRecipe, "fk_ListsOfIngredients_Recipes1_idx");

            entity.Property(e => e.GroceriesIdFoodItem).HasColumnName("Groceries_idFoodItem");
            entity.Property(e => e.RecipesIdRecipe).HasColumnName("Recipes_idRecipe");
            entity.Property(e => e.Quanity).HasColumnName("quanity");

            entity.HasOne(d => d.GroceriesIdFoodItemNavigation).WithMany(p => p.ListsOfIngredients)
                .HasForeignKey(d => d.GroceriesIdFoodItem)
                .HasConstraintName("fk_ListsOfIngredients_Groceries1");

            entity.HasOne(d => d.RecipesIdRecipeNavigation).WithMany(p => p.ListsOfIngredients)
                .HasForeignKey(d => d.RecipesIdRecipe)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ListsOfIngredients_Recipes1");
        });

        modelBuilder.Entity<OwnedGrocery>(entity =>
        {
            entity.HasKey(e => new { e.IdOwnedFoodItem, e.UsersIdUser, e.GroceriesIdFoodItem }).HasName("PK__OwnedGro__C940255C885E5D9E");

            entity.HasIndex(e => e.GroceriesIdFoodItem, "fk_OwnedGroceries_Groceries1_idx");

            entity.HasIndex(e => e.UsersIdUser, "fk_OwnedGroceries_Users1_idx");

            entity.Property(e => e.IdOwnedFoodItem)
                .ValueGeneratedOnAdd()
                .HasColumnName("idOwnedFoodItem");
            entity.Property(e => e.UsersIdUser).HasColumnName("Users_idUser");
            entity.Property(e => e.GroceriesIdFoodItem).HasColumnName("Groceries_idFoodItem");
            entity.Property(e => e.ExpirationDate)
                .HasColumnType("date")
                .HasColumnName("expirationDate");
            entity.Property(e => e.Location)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("location");
            entity.Property(e => e.Quanity).HasColumnName("quanity");

            entity.HasOne(d => d.GroceriesIdFoodItemNavigation).WithMany(p => p.OwnedGroceries)
                .HasForeignKey(d => d.GroceriesIdFoodItem)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_OwnedGroceries_Groceries1");

            entity.HasOne(d => d.UsersIdUserNavigation).WithMany(p => p.OwnedGroceries)
                .HasForeignKey(d => d.UsersIdUser)
                .HasConstraintName("fk_OwnedGroceries_Users1");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => new { e.IdRating, e.UsersIdUser, e.RecipesIdRecipe }).HasName("PK__Ratings__83B6C4F17ADA9D52");

            entity.HasIndex(e => e.RecipesIdRecipe, "fk_Ratings_Recipes1_idx");

            entity.HasIndex(e => e.UsersIdUser, "fk_Ratings_Users1_idx");

            entity.Property(e => e.IdRating)
                .ValueGeneratedOnAdd()
                .HasColumnName("idRating");
            entity.Property(e => e.UsersIdUser).HasColumnName("Users_idUser");
            entity.Property(e => e.RecipesIdRecipe).HasColumnName("Recipes_idRecipe");
            entity.Property(e => e.Comment)
                .IsUnicode(false)
                .HasColumnName("comment");
            entity.Property(e => e.Date)
                .HasColumnType("date")
                .HasColumnName("date");
            entity.Property(e => e.Score).HasColumnName("score");

            entity.HasOne(d => d.RecipesIdRecipeNavigation).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.RecipesIdRecipe)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Ratings_Recipes1");

            entity.HasOne(d => d.UsersIdUserNavigation).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.UsersIdUser)
                .HasConstraintName("fk_Ratings_Users1");
        });

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.HasKey(e => e.IdRecipe).HasName("PK__Recipes__7BA2E0835D4623E0");

            entity.HasIndex(e => e.UsersIdUser, "fk_Recipes_Users2_idx");

            entity.Property(e => e.IdRecipe).HasColumnName("idRecipe");
            entity.Property(e => e.CreateDate)
                .HasColumnType("date")
                .HasColumnName("createDate");
            entity.Property(e => e.Description)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.EditDate)
                .HasColumnType("date")
                .HasColumnName("editDate");
            entity.Property(e => e.Instruction)
                .IsUnicode(false)
                .HasColumnName("instruction");
            entity.Property(e => e.Name)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.PrepTime).HasColumnName("prepTime");
            entity.Property(e => e.UsersIdUser).HasColumnName("Users_idUser");

            entity.HasOne(d => d.UsersIdUserNavigation).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.UsersIdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Ratings_Users2");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.IdTag).HasName("PK__Tags__020FEDB855C5239A");

            entity.Property(e => e.IdTag).HasColumnName("idTag");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Priority).HasColumnName("priority");

            entity.HasMany(d => d.RecipesIdRecipes).WithMany(p => p.TagsIdTags)
                .UsingEntity<Dictionary<string, object>>(
                    "RecipesTag",
                    r => r.HasOne<Recipe>().WithMany()
                        .HasForeignKey("RecipesIdRecipe")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_RecipesTags_Recipes1"),
                    l => l.HasOne<Tag>().WithMany()
                        .HasForeignKey("TagsIdTag")
                        .HasConstraintName("fk_RecipesTags_Tags1"),
                    j =>
                    {
                        j.HasKey("TagsIdTag", "RecipesIdRecipe").HasName("PK__RecipesT__002714D0AC7121C6");
                        j.ToTable("RecipesTags");
                        j.HasIndex(new[] { "RecipesIdRecipe" }, "fk_RecipesTags_Recipes1_idx");
                        j.IndexerProperty<int>("TagsIdTag").HasColumnName("Tags_idTag");
                        j.IndexerProperty<int>("RecipesIdRecipe").HasColumnName("Recipes_idRecipe");
                    });
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("PK__Users__3717C982E7E7B17B");

            entity.Property(e => e.IdUser).HasColumnName("idUser");
            entity.Property(e => e.CreateDate)
                .HasColumnType("date")
                .HasColumnName("createDate");
            entity.Property(e => e.Login)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("login");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Role).HasColumnName("role");

            entity.HasMany(d => d.RecipesIdRecipes).WithMany(p => p.UsersIdUsers)
                .UsingEntity<Dictionary<string, object>>(
                    "FavoriteRecipe",
                    r => r.HasOne<Recipe>().WithMany()
                        .HasForeignKey("RecipesIdRecipe")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_Users_has_Recipes_Recipes1"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UsersIdUser")
                        .HasConstraintName("fk_Users_has_Recipes_Users1"),
                    j =>
                    {
                        j.HasKey("UsersIdUser", "RecipesIdRecipe").HasName("PK__Favorite__946672E1CFEE9A4E");
                        j.ToTable("FavoriteRecipes");
                        j.HasIndex(new[] { "RecipesIdRecipe" }, "fk_Users_has_Recipes_Recipes1_idx");
                        j.HasIndex(new[] { "UsersIdUser" }, "fk_Users_has_Recipes_Users1_idx");
                        j.IndexerProperty<int>("UsersIdUser").HasColumnName("Users_idUser");
                        j.IndexerProperty<int>("RecipesIdRecipe").HasColumnName("Recipes_idRecipe");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
