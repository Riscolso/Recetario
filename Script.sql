-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';

-- -----------------------------------------------------
-- Schema Recetario
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema Recetario
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `Recetario` DEFAULT CHARACTER SET utf8 ;
USE `Recetario` ;

-- -----------------------------------------------------
-- Table `Recetario`.`Actor`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Recetario`.`Actor` (
  `idActor` INT NOT NULL AUTO_INCREMENT,
  `NombreActor` VARCHAR(55) NOT NULL,
  `FechaNac` DATE NOT NULL,
  `Tipo` TINYINT(1) NOT NULL,
  PRIMARY KEY (`idActor`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Recetario`.`Correo`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Recetario`.`Correo` (
  `IdEmail` INT NOT NULL,
  `Correo` VARCHAR(60) NOT NULL,
  `Actor_idActor` INT NOT NULL,
  PRIMARY KEY (`IdEmail`),
  INDEX `fk_Correo_Actor1_idx` (`Actor_idActor` ASC),
  CONSTRAINT `fk_Correo_Actor1`
    FOREIGN KEY (`Actor_idActor`)
    REFERENCES `Recetario`.`Actor` (`idActor`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Recetario`.`Receta`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Recetario`.`Receta` (
  `idReceta` INT NOT NULL AUTO_INCREMENT,
  `Nombre` VARCHAR(45) NOT NULL,
  `ProcentajePromedio` INT NOT NULL,
  `TiempoPrep` VARCHAR(10) NOT NULL,
  `Actor_idActor` INT NOT NULL,
  PRIMARY KEY (`idReceta`, `Actor_idActor`),
  INDEX `fk_Receta_Actor1_idx` (`Actor_idActor` ASC),
  CONSTRAINT `fk_Receta_Actor1`
    FOREIGN KEY (`Actor_idActor`)
    REFERENCES `Recetario`.`Actor` (`idActor`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Recetario`.`Etiqueta`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Recetario`.`Etiqueta` (
  `idEtiqueta` INT NOT NULL AUTO_INCREMENT,
  `Etiqueta` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`idEtiqueta`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Recetario`.`Paso`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Recetario`.`Paso` (
  `NoPaso` INT NOT NULL,
  `Texto` VARCHAR(600) NOT NULL,
  `TiempoTemporizador` INT NOT NULL,
  `Receta_idReceta` INT NOT NULL,
  PRIMARY KEY (`NoPaso`),
  INDEX `fk_Paso_Receta1_idx` (`Receta_idReceta` ASC),
  CONSTRAINT `fk_Paso_Receta1`
    FOREIGN KEY (`Receta_idReceta`)
    REFERENCES `Recetario`.`Receta` (`idReceta`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Recetario`.`Ingrediente`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Recetario`.`Ingrediente` (
  `idIngrediente` INT NOT NULL AUTO_INCREMENT,
  `Nombre` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`idIngrediente`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Recetario`.`Lleva`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Recetario`.`Lleva` (
  `Receta_idReceta` INT NOT NULL,
  `Receta_Actor_idActor` INT NOT NULL,
  `Ingrediente_idIngrediente` INT NOT NULL,
  PRIMARY KEY (`Receta_idReceta`, `Receta_Actor_idActor`, `Ingrediente_idIngrediente`),
  INDEX `fk_Receta_has_Ingrediente_Ingrediente1_idx` (`Ingrediente_idIngrediente` ASC),
  INDEX `fk_Receta_has_Ingrediente_Receta1_idx` (`Receta_idReceta` ASC, `Receta_Actor_idActor` ASC),
  CONSTRAINT `fk_Receta_has_Ingrediente_Receta1`
    FOREIGN KEY (`Receta_idReceta` , `Receta_Actor_idActor`)
    REFERENCES `Recetario`.`Receta` (`idReceta` , `Actor_idActor`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Receta_has_Ingrediente_Ingrediente1`
    FOREIGN KEY (`Ingrediente_idIngrediente`)
    REFERENCES `Recetario`.`Ingrediente` (`idIngrediente`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Recetario`.`Visualizacion`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Recetario`.`Visualizacion` (
  `Actor_idActor` INT NOT NULL,
  `Receta_idReceta` INT NOT NULL,
  `Receta_Actor_idActor` INT NOT NULL,
  `ProcentajeCompl` INT NULL,
  `Calificacion` TINYINT(1) NULL,
  PRIMARY KEY (`Actor_idActor`, `Receta_idReceta`, `Receta_Actor_idActor`),
  INDEX `fk_Actor_has_Receta_Receta1_idx` (`Receta_idReceta` ASC, `Receta_Actor_idActor` ASC),
  INDEX `fk_Actor_has_Receta_Actor1_idx` (`Actor_idActor` ASC),
  CONSTRAINT `fk_Actor_has_Receta_Actor1`
    FOREIGN KEY (`Actor_idActor`)
    REFERENCES `Recetario`.`Actor` (`idActor`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Actor_has_Receta_Receta1`
    FOREIGN KEY (`Receta_idReceta` , `Receta_Actor_idActor`)
    REFERENCES `Recetario`.`Receta` (`idReceta` , `Actor_idActor`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Recetario`.`Usa`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Recetario`.`Usa` (
  `Receta_idReceta` INT NOT NULL,
  `Receta_Actor_idActor` INT NOT NULL,
  `Etiqueta_idEtiqueta` INT NOT NULL,
  PRIMARY KEY (`Receta_idReceta`, `Receta_Actor_idActor`, `Etiqueta_idEtiqueta`),
  INDEX `fk_Receta_has_Etiqueta_Etiqueta1_idx` (`Etiqueta_idEtiqueta` ASC),
  INDEX `fk_Receta_has_Etiqueta_Receta1_idx` (`Receta_idReceta` ASC, `Receta_Actor_idActor` ASC),
  CONSTRAINT `fk_Receta_has_Etiqueta_Receta1`
    FOREIGN KEY (`Receta_idReceta` , `Receta_Actor_idActor`)
    REFERENCES `Recetario`.`Receta` (`idReceta` , `Actor_idActor`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Receta_has_Etiqueta_Etiqueta1`
    FOREIGN KEY (`Etiqueta_idEtiqueta`)
    REFERENCES `Recetario`.`Etiqueta` (`idEtiqueta`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
