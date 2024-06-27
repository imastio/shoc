CREATE TABLE `wspc_workspaces` (
    `Id` varchar(100) NOT NULL,
    `Name` varchar(100) NOT NULL,
    `Description` text NOT NULL,
    `Type` varchar(64) NOT NULL,
    `Status` varchar(64) NOT NULL,
    `CreatedBy` varchar(100) NOT NULL,
    `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `Updated` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (`Id`),
    UNIQUE KEY `Name_UNIQUE` (`Name`),
    KEY `FK_Workspaces_User_idx` (`CreatedBy`),
CONSTRAINT `FK_Workspaces_Users` FOREIGN KEY (`CreatedBy`) REFERENCES `idp_users` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `wspc_workspace_members` (
    `Id` varchar(100) NOT NULL,
    `WorkspaceId` varchar(100) NOT NULL,
    `UserId` varchar(100) NOT NULL,
    `Role` varchar(64) NOT NULL,
    `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `Updated` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (`Id`),
    UNIQUE KEY `Workspace_User_UNIQUE` (`WorkspaceId`,`UserId`),
    KEY `FK_WorkspaceMember_User_idx` (`UserId`),
    CONSTRAINT `FK_WorkspaceMember_User` FOREIGN KEY (`UserId`) REFERENCES `idp_users` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT `FK_WorkspaceMember_Workspace` FOREIGN KEY (`WorkspaceId`) REFERENCES `wspc_workspaces` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
