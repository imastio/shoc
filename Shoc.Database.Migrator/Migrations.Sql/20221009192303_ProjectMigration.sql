
CREATE TABLE IF NOT EXISTS `prj_projects` (
  `Id` varchar(100) NOT NULL,
  `Name` varchar(200) NOT NULL,
  `OwnerId` varchar(100) NOT NULL,
  `Type` varchar(100) NOT NULL,
  `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Updated` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Project_Name_Dir_Unique_idx` (`Name`, `OwnerId`),
  KEY `Project_Owner_FK_idx` (`OwnerId`),
  CONSTRAINT `Project_Owner_FK` FOREIGN KEY (`OwnerId`) REFERENCES `idp_users` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE IF NOT EXISTS `prj_packages` (
  `Id` varchar(100) NOT NULL,
  `ProjectId` varchar(100) NOT NULL,
  `Status` varchar(25) NOT NULL,
  `RegistryId` varchar(100) DEFAULT NULL,
  `ImageUri` text,
  `BuildSpec` longtext,
  `ImageRecipe` longtext,
  `ListingChecksum` text,
  `ImageChecksum` varchar(250) DEFAULT NULL,
  `Progress` int NOT NULL DEFAULT '0',
  `ProgressMessage` text,
  `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Updated` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  KEY `Package_Project_FK_idx` (`ProjectId`),
  KEY `Package_Registry_FK_idx` (`RegistryId`),
  CONSTRAINT `Package_Project_FK` FOREIGN KEY (`ProjectId`) REFERENCES `prj_projects` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `Package_Registry_FK` FOREIGN KEY (`RegistryId`) REFERENCES `bld_docker_registries` (`Id`) ON DELETE SET NULL ON UPDATE SET NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE IF NOT EXISTS `prj_package_bundles` (
  `Id` varchar(100) NOT NULL,
  `PackageId` varchar(100) NOT NULL,
  `BundleRoot` varchar(1024) NOT NULL,
  `Created` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  KEY `Bundle_Package_FK_idx` (`PackageId`),
  CONSTRAINT `Bundle_Package_FK` FOREIGN KEY (`PackageId`) REFERENCES `prj_packages` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


CREATE TABLE IF NOT EXISTS `prj_project_versions` (
  `ProjectId` varchar(100) NOT NULL,
  `Version` varchar(150) NOT NULL,
  `PackageId` varchar(100) NOT NULL,
  PRIMARY KEY (`ProjectId`, `Version`),
  KEY `Package_Version_FK_idx` (`PackageId`),
  CONSTRAINT `Package_Version_FK` FOREIGN KEY (`PackageId`) REFERENCES `prj_packages` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `Project_Version_FK` FOREIGN KEY (`ProjectId`) REFERENCES `prj_projects` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
