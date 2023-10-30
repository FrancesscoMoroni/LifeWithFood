USE LifeWithFoodDB ;

CREATE TABLE Recipes (
  idRecipe INT NOT NULL IDENTITY,
  name TEXT NOT NULL,
  description TEXT NULL,
  instruction TEXT NULL,
  prepTime INT NULL,
  createDate DATE NOT NULL,
  editDate DATE NULL,
  PRIMARY KEY (idRecipe));


CREATE TABLE Users (
  idUser INT NOT NULL IDENTITY,
  login VARCHAR(45) NULL,
  password VARCHAR(45) NULL,
  name VARCHAR(45) NULL,
  role INT NULL,
  createDate DATE NULL,
  PRIMARY KEY (idUser));


CREATE TABLE Ratings (
  idRating INT NOT NULL IDENTITY,
  score INT NULL,
  comment TEXT NULL,
  date DATE NOT NULL,
  Users_idUser INT NOT NULL,
  Recipes_idRecipe INT NOT NULL,
  PRIMARY KEY (idRating, Users_idUser, Recipes_idRecipe),
  INDEX fk_Ratings_Users1_idx (Users_idUser ASC) ,
  INDEX fk_Ratings_Recipes1_idx (Recipes_idRecipe ASC),
  CONSTRAINT fk_Ratings_Users1
    FOREIGN KEY (Users_idUser)
    REFERENCES Users (idUser)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT fk_Ratings_Recipes1
    FOREIGN KEY (Recipes_idRecipe)
    REFERENCES Recipes (idRecipe)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION);


CREATE TABLE Groceries (
  idFoodItem INT NOT NULL IDENTITY,
  name TEXT NOT NULL,
  PRIMARY KEY (idFoodItem));


CREATE TABLE ListsOfIngredients (
  Groceries_idFoodItem INT NOT NULL,
  Recipes_idRecipe INT NOT NULL,
  PRIMARY KEY (Groceries_idFoodItem, Recipes_idRecipe),
  INDEX fk_ListsOfIngredients_Groceries1_idx (Groceries_idFoodItem ASC),
  INDEX fk_ListsOfIngredients_Recipes1_idx (Recipes_idRecipe ASC),
  CONSTRAINT fk_ListsOfIngredients_Groceries1
    FOREIGN KEY (Groceries_idFoodItem)
    REFERENCES Groceries (idFoodItem)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT fk_ListsOfIngredients_Recipes1
    FOREIGN KEY (Recipes_idRecipe)
    REFERENCES Recipes (idRecipe)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION);


CREATE TABLE OwnedGroceries (
  idOwnedFoodItem INT NOT NULL IDENTITY,
  name TEXT NOT NULL,
  expirationDate DATE NULL,
  Users_idUser INT NOT NULL,
  Groceries_idFoodItem INT NOT NULL,
  PRIMARY KEY (idOwnedFoodItem, Users_idUser, Groceries_idFoodItem),
  INDEX fk_OwnedGroceries_Users1_idx (Users_idUser ASC) ,
  INDEX fk_OwnedGroceries_Groceries1_idx (Groceries_idFoodItem ASC),
  CONSTRAINT fk_OwnedGroceries_Users1
    FOREIGN KEY (Users_idUser)
    REFERENCES Users (idUser)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT fk_OwnedGroceries_Groceries1
    FOREIGN KEY (Groceries_idFoodItem)
    REFERENCES Groceries (idFoodItem)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION);


CREATE TABLE Tags (
  idTag INT NOT NULL IDENTITY,
  name VARCHAR(45) NULL,
  priority INT NULL,
  PRIMARY KEY (idTag));


CREATE TABLE RecipesTags (
  Tags_idTag INT NOT NULL,
  Recipes_idRecipe INT NOT NULL,
  PRIMARY KEY (Tags_idTag, Recipes_idRecipe),
  INDEX fk_RecipesTags_Recipes1_idx (Recipes_idRecipe ASC),
  CONSTRAINT fk_RecipesTags_Tags1
    FOREIGN KEY (Tags_idTag)
    REFERENCES Tags (idTag)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT fk_RecipesTags_Recipes1
    FOREIGN KEY (Recipes_idRecipe)
    REFERENCES Recipes (idRecipe)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION);


CREATE TABLE Images (
  idImage INT NOT NULL IDENTITY,
  position VARCHAR(45) NULL,
  Recipes_idRecipe INT NOT NULL,
  PRIMARY KEY (idImage, Recipes_idRecipe),
  INDEX fk_Images_Recipes1_idx (Recipes_idRecipe ASC),
  CONSTRAINT fk_Images_Recipes1
    FOREIGN KEY (Recipes_idRecipe)
    REFERENCES Recipes (idRecipe)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION);


CREATE TABLE FavoriteRecipes (
  Users_idUser INT NOT NULL,
  Recipes_idRecipe INT NOT NULL,
  PRIMARY KEY (Users_idUser, Recipes_idRecipe),
  INDEX fk_Users_has_Recipes_Recipes1_idx (Recipes_idRecipe ASC),
  INDEX fk_Users_has_Recipes_Users1_idx (Users_idUser ASC),
  CONSTRAINT fk_Users_has_Recipes_Users1
    FOREIGN KEY (Users_idUser)
    REFERENCES Users (idUser)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT fk_Users_has_Recipes_Recipes1
    FOREIGN KEY (Recipes_idRecipe)
    REFERENCES Recipes (idRecipe)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION);
