CREATE TABLE `reg_registries` (
    `Id` varchar(100) NOT NULL,
    `WorkspaceId` varchar(100) DEFAULT NULL,
    `Name` varchar(64) NOT NULL,
    `DisplayName` varchar(128) NOT NULL,
    `Status` varchar(64) NOT NULL,
    `Provider` varchar(64) NOT NULL,
    `UsageScope` varchar(64) NOT NULL,
    `Registry` text NOT NULL,
    `Namespace` text NOT NULL,
    `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `Updated` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (`Id`),
    UNIQUE KEY `WorkspaceId_Name_UNIQUE` (`WorkspaceId`,`Name`),
CONSTRAINT `Registries_Workspace_FK` FOREIGN KEY (`WorkspaceId`) REFERENCES `wspc_workspaces` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `reg_registries_credentials` (
    `Id` varchar(100) NOT NULL,
    `RegistryId` varchar(100) NOT NULL,
    `WorkspaceId` varchar(100) DEFAULT NULL,
    `UserId` varchar(100) DEFAULT NULL,
    `Source` varchar(64) NOT NULL,
    `Username` varchar(256) NOT NULL,
    `PasswordEncrypted` text NOT NULL,
    `Email` varchar(256) DEFAULT NULL,
    `PullAllowed` bit(1) NOT NULL,
    `PushAllowed` bit(1) NOT NULL,
    `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `Updated` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (`Id`),
    KEY `FK_RegistryCredentials_Registries_idx` (`RegistryId`),
    KEY `FK_RegistryCredentials_Users_idx` (`UserId`),
    KEY `FK_RegistryCredentials_Workspaces_idx` (`WorkspaceId`),
    KEY `FK_RegistryCredentials_WorkspaceMembers_idx` (`WorkspaceId`,`UserId`),
    CONSTRAINT `FK_RegistryCredentials_Registries` FOREIGN KEY (`RegistryId`) REFERENCES `reg_registries` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT `FK_RegistryCredentials_Users` FOREIGN KEY (`UserId`) REFERENCES `idp_users` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT `FK_RegistryCredentials_WorkspaceMembers` FOREIGN KEY (`WorkspaceId`, `UserId`) REFERENCES `wspc_workspace_members` (`WorkspaceId`, `UserId`) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT `FK_RegistryCredentials_Workspaces` FOREIGN KEY (`WorkspaceId`) REFERENCES `wspc_workspaces` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

