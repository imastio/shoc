
CREATE TABLE IF NOT EXISTS `exc_jobs` (
  `Id` varchar(100) NOT NULL,
  `ProjectId` varchar(100) NOT NULL,
  `PackageId` varchar(100) NOT NULL,
  `OwnerId` varchar(100) NOT NULL,
  `RunSpec` longtext,
  `RunInfo` longtext,
  `Status` varchar(25) NOT NULL,
  `Progress` int NOT NULL DEFAULT '0',
  `ProgressMessage` text,
  `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Updated` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  KEY `Job_Project_FK_idx` (`ProjectId`),
  KEY `Job_Package_FK_idx` (`PackageId`),
  KEY `Job_Owner_FK_idx` (`OwnerId`),
  CONSTRAINT `Job_Project_FK_idx` FOREIGN KEY (`ProjectId`) REFERENCES `prj_projects` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `Job_Package_FK_idx` FOREIGN KEY (`PackageId`) REFERENCES `prj_packages` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `Job_Owner_FK_idx` FOREIGN KEY (`OwnerId`) REFERENCES `idp_users` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
