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

    public virtual DbSet<ListsOfIngredient> ListsOfIngredients { get; set; }

    public virtual DbSet<OwnedGrocery> OwnedGroceries { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<Recipe> Recipes { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Grocery>(entity =>
        {
            entity.HasKey(e => e.IdFoodItem).HasName("PK__Grocerie__09949FB32487AE67");

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

        modelBuilder.Entity<ListsOfIngredient>(entity =>
        {
            entity.HasKey(e => new { e.GroceriesIdFoodItem, e.RecipesIdRecipe }).HasName("PK__ListsOfI__1A13F7DC94610DB3");

            entity.HasIndex(e => e.GroceriesIdFoodItem, "fk_ListsOfIngredients_Groceries1_idx");

            entity.HasIndex(e => e.RecipesIdRecipe, "fk_ListsOfIngredients_Recipes1_idx");

            entity.Property(e => e.GroceriesIdFoodItem).HasColumnName("Groceries_idFoodItem");
            entity.Property(e => e.RecipesIdRecipe).HasColumnName("Recipes_idRecipe");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.GroceriesIdFoodItemNavigation).WithMany(p => p.ListsOfIngredients)
                .HasForeignKey(d => d.GroceriesIdFoodItem)
                .HasConstraintName("fk_ListsOfIngredients_Groceries1");

            entity.HasOne(d => d.RecipesIdRecipeNavigation).WithMany(p => p.ListsOfIngredients)
                .HasForeignKey(d => d.RecipesIdRecipe)
                .HasConstraintName("fk_ListsOfIngredients_Recipes1");
        });

        modelBuilder.Entity<OwnedGrocery>(entity =>
        {
            entity.HasKey(e => new { e.IdOwnedFoodItem, e.UsersIdUser, e.GroceriesIdFoodItem }).HasName("PK__OwnedGro__C940255C6A8DA0D8");

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
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.GroceriesIdFoodItemNavigation).WithMany(p => p.OwnedGroceries)
                .HasForeignKey(d => d.GroceriesIdFoodItem)
                .HasConstraintName("fk_OwnedGroceries_Groceries1");

            entity.HasOne(d => d.UsersIdUserNavigation).WithMany(p => p.OwnedGroceries)
                .HasForeignKey(d => d.UsersIdUser)
                .HasConstraintName("fk_OwnedGroceries_Users1");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => new { e.IdRating, e.UsersIdUser, e.RecipesIdRecipe }).HasName("PK__Ratings__83B6C4F1678DAFA6");

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
                .HasConstraintName("fk_Ratings_Recipes1");

            entity.HasOne(d => d.UsersIdUserNavigation).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.UsersIdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Ratings_Users1");
        });

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.HasKey(e => e.IdRecipe).HasName("PK__Recipes__7BA2E083D612495F");

            entity.HasIndex(e => e.UsersIdUser, "fk_Recipes_Users1_idx");

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
                .HasConstraintName("fk_Recipes_User1");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.IdTag).HasName("PK__Tags__020FEDB8970568BC");

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
                        .HasConstraintName("fk_RecipesTags_Recipes1"),
                    l => l.HasOne<Tag>().WithMany()
                        .HasForeignKey("TagsIdTag")
                        .HasConstraintName("fk_RecipesTags_Tags1"),
                    j =>
                    {
                        j.HasKey("TagsIdTag", "RecipesIdRecipe").HasName("PK__RecipesT__002714D0D84B06E8");
                        j.ToTable("RecipesTags");
                        j.HasIndex(new[] { "RecipesIdRecipe" }, "fk_RecipesTags_Recipes1_idx");
                        j.IndexerProperty<int>("TagsIdTag").HasColumnName("Tags_idTag");
                        j.IndexerProperty<int>("RecipesIdRecipe").HasColumnName("Recipes_idRecipe");
                    });
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("PK__Users__3717C98268E0D36B");

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
                        .HasConstraintName("fk_Users_has_Recipes_Recipes1"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UsersIdUser")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_Users_has_Recipes_Users1"),
                    j =>
                    {
                        j.HasKey("UsersIdUser", "RecipesIdRecipe").HasName("PK__Favorite__946672E1190DC3DB");
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
