CREATE TABLE `sec_secrets` (
    `Id` varchar(100) NOT NULL,
    `WorkspaceId` varchar(100) NOT NULL,
    `Name` varchar(256) NOT NULL,
    `Description` text NOT NULL,
    `Disabled` bit(1) NOT NULL,
    `Encrypted` bit(1) NOT NULL,
    `Value` text NOT NULL,
    `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `Updated` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (`Id`),
    UNIQUE KEY `UNIQUE_Workspace_SecretName` (`WorkspaceId`,`Name`),
    KEY `FK_Secrets_Workspaces_idx` (`WorkspaceId`),
    CONSTRAINT `FK_Secrets_Workspaces` FOREIGN KEY (`WorkspaceId`) REFERENCES `wspc_workspaces` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `sec_user_secrets` (
    `Id` varchar(100) NOT NULL,
    `WorkspaceId` varchar(100) NOT NULL,
    `UserId` varchar(100) NOT NULL,
    `Name` varchar(256) NOT NULL,
    `Description` text NOT NULL,
    `Disabled` bit(1) NOT NULL,
    `Encrypted` bit(1) NOT NULL,
    `Value` text NOT NULL,
    `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `Updated` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (`Id`),
    UNIQUE KEY `UNIQUE_Workspace_User_UserSecretName` (`WorkspaceId`,`UserId`,`Name`),
    KEY `FK_UserSecrets_Workspaces_idx` (`WorkspaceId`),
    KEY `FK_UserSecrets_Users_idx` (`UserId`),
    CONSTRAINT `FK_UserSecrets_Users` FOREIGN KEY (`UserId`) REFERENCES `idp_users` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT `FK_UserSecrets_Workspaces` FOREIGN KEY (`WorkspaceId`) REFERENCES `wspc_workspaces` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT `FK_UserSecrets_WorkspaceUsers` FOREIGN KEY (`WorkspaceId`, `UserId`) REFERENCES `wspc_workspace_members` (`WorkspaceId`, `UserId`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

