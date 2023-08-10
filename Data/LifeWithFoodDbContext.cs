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

    public virtual DbSet<OwnedGrocery> OwnedGroceries { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<Recipe> Recipes { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=BARTK; Database=LifeWithFoodDB;Trusted_Connection=True;Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Grocery>(entity =>
        {
            entity.HasKey(e => e.IdFoodItem).HasName("PK__Grocerie__09949FB3B8610966");

            entity.Property(e => e.IdFoodItem)
                .ValueGeneratedNever()
                .HasColumnName("idFoodItem");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("price");
        });

        modelBuilder.Entity<OwnedGrocery>(entity =>
        {
            entity.HasKey(e => new { e.IdOwnedFoodItem, e.UsersIdUser, e.GroceriesIdFoodItem }).HasName("PK__OwnedGro__C940255CB8BD932F");

            entity.HasIndex(e => e.GroceriesIdFoodItem, "fk_OwnedGroceries_Groceries_idx");

            entity.HasIndex(e => e.UsersIdUser, "fk_OwnedGroceries_Users_idx");

            entity.Property(e => e.IdOwnedFoodItem).HasColumnName("idOwnedFoodItem");
            entity.Property(e => e.UsersIdUser).HasColumnName("Users_idUser");
            entity.Property(e => e.GroceriesIdFoodItem).HasColumnName("Groceries_idFoodItem");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("name");

            entity.HasOne(d => d.GroceriesIdFoodItemNavigation).WithMany(p => p.OwnedGroceries)
                .HasForeignKey(d => d.GroceriesIdFoodItem)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_OwnedGroceries_Groceries");

            entity.HasOne(d => d.UsersIdUserNavigation).WithMany(p => p.OwnedGroceries)
                .HasForeignKey(d => d.UsersIdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_OwnedGroceries_Users");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => new { e.IdRating, e.RecipesIdRecipe, e.UsersIdUser }).HasName("PK__Ratings__27120C5FA45ACAF6");

            entity.HasIndex(e => e.RecipesIdRecipe, "fk_Ratings_Recipes_idx");

            entity.HasIndex(e => e.UsersIdUser, "fk_Ratings_Users_idx");

            entity.Property(e => e.IdRating).HasColumnName("idRating");
            entity.Property(e => e.RecipesIdRecipe).HasColumnName("Recipes_idRecipe");
            entity.Property(e => e.UsersIdUser).HasColumnName("Users_idUser");
            entity.Property(e => e.Comment)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("comment");
            entity.Property(e => e.Score)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("score");

            entity.HasOne(d => d.RecipesIdRecipeNavigation).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.RecipesIdRecipe)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Ratings_Recipes");

            entity.HasOne(d => d.UsersIdUserNavigation).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.UsersIdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Ratings_Users");
        });

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.HasKey(e => e.IdRecipe).HasName("PK__Recipes__7BA2E083D59BDABC");

            entity.Property(e => e.IdRecipe)
                .ValueGeneratedNever()
                .HasColumnName("idRecipe");
            entity.Property(e => e.Description)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.PrepTime).HasColumnName("prepTime");

            entity.HasMany(d => d.GroceriesIdFoodItems).WithMany(p => p.RecipesIdRecipes)
                .UsingEntity<Dictionary<string, object>>(
                    "ListsOfIngredient",
                    r => r.HasOne<Grocery>().WithMany()
                        .HasForeignKey("GroceriesIdFoodItem")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_Ingredients_Groceries"),
                    l => l.HasOne<Recipe>().WithMany()
                        .HasForeignKey("RecipesIdRecipe")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_Ingredients_Recipes"),
                    j =>
                    {
                        j.HasKey("RecipesIdRecipe", "GroceriesIdFoodItem").HasName("PK__ListsOfI__06CDA0586FE5F04A");
                        j.ToTable("ListsOfIngredients");
                        j.HasIndex(new[] { "GroceriesIdFoodItem" }, "fk_Ingredients_Groceries_idx");
                        j.IndexerProperty<int>("RecipesIdRecipe").HasColumnName("Recipes_idRecipe");
                        j.IndexerProperty<int>("GroceriesIdFoodItem").HasColumnName("Groceries_idFoodItem");
                    });

            entity.HasMany(d => d.UsersIdUsers).WithMany(p => p.RecipesIdRecipes)
                .UsingEntity<Dictionary<string, object>>(
                    "FavoriteRecipe",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("UsersIdUser")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_FavoriteRecipes_Users"),
                    l => l.HasOne<Recipe>().WithMany()
                        .HasForeignKey("RecipesIdRecipe")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_FavoriteRecipes_Recipes"),
                    j =>
                    {
                        j.HasKey("RecipesIdRecipe", "UsersIdUser").HasName("PK__Favorite__DE2AF80B41D28B20");
                        j.ToTable("FavoriteRecipes");
                        j.HasIndex(new[] { "UsersIdUser" }, "fk_FavoriteRecipes_Users_idx");
                        j.IndexerProperty<int>("RecipesIdRecipe").HasColumnName("Recipes_idRecipe");
                        j.IndexerProperty<int>("UsersIdUser").HasColumnName("Users_idUser");
                    });
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.IdTag).HasName("PK__Tags__020FEDB8287E403F");

            entity.Property(e => e.IdTag)
                .ValueGeneratedNever()
                .HasColumnName("idTag");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("name");

            entity.HasMany(d => d.RecipesIdRecipes).WithMany(p => p.TagsIdTags)
                .UsingEntity<Dictionary<string, object>>(
                    "RecipesTag",
                    r => r.HasOne<Recipe>().WithMany()
                        .HasForeignKey("RecipesIdRecipe")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_RecipesTags_Recipes"),
                    l => l.HasOne<Tag>().WithMany()
                        .HasForeignKey("TagsIdTag")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_RecipesTags_Tags"),
                    j =>
                    {
                        j.HasKey("TagsIdTag", "RecipesIdRecipe").HasName("PK__RecipesT__002714D05F16B6DC");
                        j.ToTable("RecipesTags");
                        j.HasIndex(new[] { "RecipesIdRecipe" }, "fk_RecipesTags_Recipes_idx");
                        j.IndexerProperty<int>("TagsIdTag").HasColumnName("Tags_idTag");
                        j.IndexerProperty<int>("RecipesIdRecipe").HasColumnName("Recipes_idRecipe");
                    });
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("PK__Users__3717C982ABA5A283");

            entity.Property(e => e.IdUser)
                .ValueGeneratedNever()
                .HasColumnName("idUser");
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
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
