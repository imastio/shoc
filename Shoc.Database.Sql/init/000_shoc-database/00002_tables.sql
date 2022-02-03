USE shoc;

CREATE TABLE IF NOT EXISTS `data_protection_keys` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `FriendlyName` text,
  `Xml` longtext,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE IF NOT EXISTS `idp_persisted_grants` (
  `Key` varchar(200) NOT NULL,
  `ClientId` varchar(200) NOT NULL,
  `Type` varchar(50) NOT NULL,
  `SubjectId` varchar(100) DEFAULT NULL,
  `SessionId` varchar(50) DEFAULT NULL,
  `Description` text,
  `Data` longtext,
  `Expiration` datetime DEFAULT NULL,
  `ConsumedTime` datetime DEFAULT NULL,
  `CreationTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Key`),
  KEY `IDP_PG_Expiration` (`Expiration`),
  KEY `IDP_PG_Sub_Cli_Type` (`SubjectId`,`ClientId`,`Type`),
  KEY `IDP_PG_Sub_Sess_Type` (`SubjectId`,`SessionId`,`Type`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE IF NOT EXISTS `idp_users` (
  `Id` varchar(100) NOT NULL,
  `Email` varchar(200) NOT NULL,
  `EmailVerified` bit(1) NOT NULL DEFAULT b'0',
  `Username` varchar(100) NOT NULL,
  `PasswordHash` text,
  `Role` varchar(100) NOT NULL,
  `FirstName` text,
  `LastName` text,
  `FullName` text NOT NULL,
  `LastIp` text,
  `LastLogin` datetime DEFAULT NULL,
  `LockedUntil` datetime DEFAULT NULL,
  `FailedAttempts` int DEFAULT 0,
  `Deleted` bit(1) NOT NULL DEFAULT b'0',
  `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Updated` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Email_UNIQUE` (`Email`),
  UNIQUE KEY `Username_UNIQUE` (`Username`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE IF NOT EXISTS `idp_confirmation_codes` (
  `Id` varchar(100) NOT NULL,
  `UserId` varchar(100) NOT NULL,
  `Email` varchar(100) NOT NULL,
  `CodeHash` varchar(200) NOT NULL,
  `Link` varchar(200) NOT NULL,
  `ValidUntil` datetime NOT NULL,
  `ReturnUrl` text,
  `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  KEY `ConfirmCode_User_FK_idx` (`UserId`),
  CONSTRAINT `ConfirmCode_User_FK` FOREIGN KEY (`UserId`) REFERENCES `idp_users` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE IF NOT EXISTS `prj_projects` (
  `Id` varchar(100) NOT NULL,
  `Name` varchar(200) NOT NULL,
  `Directory` varchar(200) NOT NULL DEFAULT '/',
  `BuildSpec` longtext,
  `RunSpec` longtext,
  `OwnerId` varchar(100) NOT NULL,
  `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Updated` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Name_UNIQUE` (`Name`),
  KEY `Project_Owner_FK_idx` (`OwnerId`),
  CONSTRAINT `Project_Owner_FK` FOREIGN KEY (`OwnerId`) REFERENCES `idp_users` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
