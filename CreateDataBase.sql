USE LifeWithFoodDB ;

CREATE TABLE Recipes (
  idRecipe INT NOT NULL AUTO_INCREMENT,
  name VARCHAR(45) NULL,
  description VARCHAR(45) NULL,
  prepTime INT NULL,
  PRIMARY KEY (idRecipe));

CREATE TABLE Users (
  idUser INT NOT NULL IDENTITY,
  login VARCHAR(45) NULL,
  password VARCHAR(45) NULL,
  name VARCHAR(45) NULL,
  role INT NULL,
  PRIMARY KEY (idUser));

CREATE TABLE Ratings (
  idRating INT NOT NULL IDENTITY,
  score VARCHAR(45) NULL,
  comment VARCHAR(45) NULL,
  Recipes_idRecipe INT NOT NULL,
  Users_idUser INT NOT NULL,
  PRIMARY KEY (idRating, Recipes_idRecipe, Users_idUser),
  INDEX fk_Ratings_Recipes_idx (Recipes_idRecipe ASC) ,
  INDEX fk_Ratings_Users_idx (Users_idUser ASC) ,
  CONSTRAINT fk_Ratings_Recipes
    FOREIGN KEY (Recipes_idRecipe)
    REFERENCES Recipes(idRecipe)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT fk_Ratings_Users
    FOREIGN KEY (Users_idUser)
    REFERENCES Users(idUser)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION);

CREATE TABLE FavoriteRecipes (
  Recipes_idRecipe INT NOT NULL,
  Users_idUser INT NOT NULL,
  PRIMARY KEY (Recipes_idRecipe, Users_idUser),
  INDEX fk_FavoriteRecipes_Users_idx (Users_idUser ASC) ,
  CONSTRAINT fk_FavoriteRecipes_Recipes
    FOREIGN KEY (Recipes_idRecipe)
    REFERENCES Recipes(idRecipe)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT fk_FavoriteRecipes_Users
    FOREIGN KEY (Users_idUser)
    REFERENCES Users(idUser)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION);

CREATE TABLE Groceries (
  idFoodItem INT NOT NULL IDENTITY,
  name VARCHAR(45) NULL,
  price VARCHAR(45) NULL,
  PRIMARY KEY (idFoodItem));

CREATE TABLE ListsOfIngredients (
  Recipes_idRecipe INT NOT NULL,
  Groceries_idFoodItem INT NOT NULL,
  PRIMARY KEY (Recipes_idRecipe, Groceries_idFoodItem),
  INDEX fk_Ingredients_Groceries_idx (Groceries_idFoodItem ASC) ,
  CONSTRAINT fk_Ingredients_Recipes
    FOREIGN KEY (Recipes_idRecipe)
    REFERENCES Recipes(idRecipe)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT fk_Ingredients_Groceries
    FOREIGN KEY (Groceries_idFoodItem)
    REFERENCES Groceries(idFoodItem)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION);

CREATE TABLE OwnedGroceries (
  idOwnedFoodItem INT NOT NULL IDENTITY,
  name VARCHAR(45) NULL,
  Users_idUser INT NOT NULL,
  Groceries_idFoodItem INT NOT NULL,
  PRIMARY KEY (idOwnedFoodItem, Users_idUser, Groceries_idFoodItem),
  INDEX fk_OwnedGroceries_Users_idx (Users_idUser ASC) ,
  INDEX fk_OwnedGroceries_Groceries_idx (Groceries_idFoodItem ASC) ,
  CONSTRAINT fk_OwnedGroceries_Users
    FOREIGN KEY (Users_idUser)
    REFERENCES Users(idUser)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT fk_OwnedGroceries_Groceries
    FOREIGN KEY (Groceries_idFoodItem)
    REFERENCES Groceries(idFoodItem)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION);


CREATE TABLE Tags (
  idTag INT NOT NULL IDENTITY,
  name VARCHAR(45) NULL,
  PRIMARY KEY (idTag));

CREATE TABLE RecipesTags (
  Tags_idTag INT NOT NULL,
  Recipes_idRecipe INT NOT NULL,
  PRIMARY KEY (Tags_idTag, Recipes_idRecipe),
  INDEX fk_RecipesTags_Recipes_idx (Recipes_idRecipe ASC) ,
  CONSTRAINT fk_RecipesTags_Tags
    FOREIGN KEY (Tags_idTag)
    REFERENCES Tags (idTag)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT fk_RecipesTags_Recipes
    FOREIGN KEY (Recipes_idRecipe)
    REFERENCES Recipes (idRecipe)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION);
