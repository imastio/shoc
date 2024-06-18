CREATE TABLE `idp_users` (
    `Id` varchar(100) NOT NULL,
    `Email` varchar(200) NOT NULL,
    `EmailVerified` bit(1) NOT NULL DEFAULT b'0',
    `Username` varchar(100) NOT NULL,
    `Type` varchar(50) NOT NULL DEFAULT 'external',
    `UserState` varchar(50) NOT NULL DEFAULT 'active',
    `PasswordHash` text,
    `Phone` varchar(100) DEFAULT NULL,
    `PhoneVerified` bit(1) NOT NULL DEFAULT b'0',
    `FirstName` text,
    `LastName` text,
    `FullName` text NOT NULL,
    `PictureUri` longtext,
    `Gender` text,
    `Timezone` text,
    `BirthDate` date DEFAULT NULL,
    `Country` text,
    `State` text,
    `City` text,
    `Postal` text,
    `Address1` text,
    `Address2` text,
    `LastIp` text,
    `LastLogin` datetime DEFAULT NULL,
    `MultiFactor` bit(1) NOT NULL DEFAULT b'0',
    `LockedUntil` datetime DEFAULT NULL,
    `FailedAttempts` int DEFAULT '0',
    `Metadata` longtext,
    `Deleted` bit(1) NOT NULL DEFAULT b'0',
    `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `Updated` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (`Id`),
    UNIQUE KEY `Email_UNIQUE` (`Email`),
    UNIQUE KEY `Username_UNIQUE` (`Username`),
    KEY `User_Created_IDX` (`Created` DESC)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `idp_users_accesses` (
    `Id` varchar(128) NOT NULL,
    `UserId` varchar(128) NOT NULL,
    `Access` varchar(128) NOT NULL,
    `Updated` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (`Id`),
    UNIQUE KEY `User_Accesses_UNIQUE` (`UserId`,`Access`),
    KEY `User_Accesses_FK_idx` (`UserId`),
    CONSTRAINT `User_Accesses_FK` FOREIGN KEY (`UserId`) REFERENCES `idp_users` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `idp_signin_history` (
    `Id` varchar(100) NOT NULL,
    `SessionId` text,
    `UserId` varchar(100) NOT NULL,
    `Ip` varchar(20) DEFAULT NULL,
    `Provider` varchar(100) DEFAULT NULL,
    `MethodType` varchar(100) DEFAULT NULL,
    `MiltiFactorType` varchar(100) DEFAULT NULL,
    `UserAgent` text,
    `Time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `Refreshed` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (`Id`),
    KEY `SignInHist_User_FK_idx` (`UserId`),
    KEY `SignInHist_Time_IDX` (`Time` DESC),
    CONSTRAINT `SignInHist_User_FK` FOREIGN KEY (`UserId`) REFERENCES `idp_users` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `idp_confirmation_codes` (
    `Id` varchar(100) NOT NULL,
    `UserId` varchar(100) NOT NULL,
    `Target` varchar(100) NOT NULL,
    `TargetType` varchar(15) NOT NULL,
    `CodeHash` varchar(200) NOT NULL,
    `Link` varchar(200) NOT NULL,
    `Lang` varchar(10) NOT NULL,
    `ValidUntil` datetime NOT NULL,
    `ReturnUrl` text,
    `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (`Id`),
    KEY `ConfirmCode_User_FK_idx` (`UserId`),
    CONSTRAINT `ConfirmCode_User_FK` FOREIGN KEY (`UserId`) REFERENCES `idp_users` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `idp_one_time_passwords` (
    `Id` varchar(100) NOT NULL,
    `UserId` varchar(100) NOT NULL,
    `Target` varchar(100) DEFAULT NULL,
    `TargetType` varchar(15) NOT NULL,
    `DeliveryMethod` varchar(50) NOT NULL,
    `PasswordHash` varchar(150) NOT NULL,
    `Link` varchar(200) NOT NULL,
    `Lang` varchar(20) NOT NULL,
    `ValidUntil` datetime NOT NULL,
    `ReturnUrl` text,
    `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (`Id`),
    KEY `OneTimePass_User_FK_idx` (`UserId`),
    CONSTRAINT `OneTimePass_User_FK` FOREIGN KEY (`UserId`) REFERENCES `idp_users` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `idp_user_groups` (
    `Id` varchar(100) NOT NULL,
    `Name` varchar(150) NOT NULL,
    `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `Updated` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (`Id`),
    UNIQUE KEY `Name_UNIQUE` (`Name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `idp_user_groups_accesses` (
    `Id` varchar(128) NOT NULL,
    `GroupId` varchar(128) NOT NULL,
    `Access` varchar(128) NOT NULL,
    `Updated` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (`Id`),
    UNIQUE KEY `UserGroup_Accesses_UNIQUE` (`GroupId`,`Access`),
    KEY `UserGroup_Accesses_FK_idx` (`GroupId`),
    CONSTRAINT `UserGroup_Accesses_FK` FOREIGN KEY (`GroupId`) REFERENCES `idp_user_groups` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `idp_user_group_membership` (
    `GroupId` varchar(100) NOT NULL,
    `UserId` varchar(100) NOT NULL,
    UNIQUE KEY `FK_UserGroup_Unique` (`GroupId`,`UserId`),
    KEY `FK_UserGroup_Group_idx` (`GroupId`),
    KEY `FK_UserGroup_User_idx` (`UserId`),
    CONSTRAINT `FK_UserGroup_Group` FOREIGN KEY (`GroupId`) REFERENCES `idp_user_groups` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT `FK_UserGroup_User` FOREIGN KEY (`UserId`) REFERENCES `idp_users` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `idp_roles` (
    `Id` varchar(128) NOT NULL,
    `Name` varchar(256) NOT NULL,
    `Description` longtext,
    `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `Updated` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (`Id`),
    UNIQUE KEY `Name_UNIQUE` (`Name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `idp_role_membership` (
    `RoleId` varchar(128) NOT NULL,
    `UserId` varchar(128) NOT NULL,
    UNIQUE KEY `Role_Privilege_UNIQUE` (`RoleId`,`UserId`),
    KEY `FK_RolePrivileges_Role_idx` (`RoleId`),
    KEY `FK_RolePrivileges_Privilege_idx` (`UserId`),
    CONSTRAINT `FK_RoleMembers_Role` FOREIGN KEY (`RoleId`) REFERENCES `idp_roles` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT `FK_RoleMembers_User` FOREIGN KEY (`UserId`) REFERENCES `idp_users` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `idp_privileges` (
    `Id` varchar(128) NOT NULL,
    `Name` varchar(256) NOT NULL,
    `Description` longtext,
    `Category` varchar(256) NOT NULL,
    `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `Updated` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (`Id`),
    UNIQUE KEY `Name_UNIQUE` (`Name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `idp_role_privileges` (
    `RoleId` varchar(128) NOT NULL,
    `PrivilegeId` varchar(128) NOT NULL,
    UNIQUE KEY `Role_Privilege_UNIQUE` (`RoleId`,`PrivilegeId`),
    KEY `FK_RolePrivileges_Role_idx` (`RoleId`),
    KEY `FK_RolePrivileges_Privilege_idx` (`PrivilegeId`),
    CONSTRAINT `FK_RolePrivileges_Privilege` FOREIGN KEY (`PrivilegeId`) REFERENCES `idp_privileges` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT `FK_RolePrivileges_Role` FOREIGN KEY (`RoleId`) REFERENCES `idp_roles` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `idp_privileges_accesses` (
    `Id` varchar(128) NOT NULL,
    `PrivilegeId` varchar(128) NOT NULL,
    `Access` varchar(128) NOT NULL,
    `Updated` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (`Id`),
    UNIQUE KEY `Privilege_Access_UNIQUE` (`PrivilegeId`,`Access`),
    KEY `Privilege_Access_FK_idx` (`PrivilegeId`),
    CONSTRAINT `Privilege_Access_FK` FOREIGN KEY (`PrivilegeId`) REFERENCES `idp_privileges` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;













