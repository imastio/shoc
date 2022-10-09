
CREATE TABLE IF NOT EXISTS `bld_docker_registries` (
  `Id` varchar(100) NOT NULL,
  `Name` varchar(100) NOT NULL,
  `OwnerId` varchar(100) DEFAULT NULL,
  `RegistryUri` varchar(500) NOT NULL,
  `Shared` bit(1) NOT NULL DEFAULT b'0',
  `Email` text,
  `Username` text,
  `EncryptedPassword` blob,
  `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Updated` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `DockerRegistry_Name_Unique` (`Name`,`OwnerId`),
  KEY `DockerRegistry_Owner_FK_idx` (`OwnerId`),
  CONSTRAINT `DockerRegistry_Owner_FK` FOREIGN KEY (`OwnerId`) REFERENCES `idp_users` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
