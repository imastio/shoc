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
