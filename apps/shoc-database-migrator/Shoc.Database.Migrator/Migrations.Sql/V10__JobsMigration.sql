﻿CREATE TABLE `job_labels` (
    `Id` varchar(100) NOT NULL,
    `WorkspaceId` varchar(100) NOT NULL,
    `Name` varchar(256) NOT NULL,
    `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `Updated` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (`Id`),
    UNIQUE KEY `UNIQUE_Label_Workspace` (`WorkspaceId`,`Name`),
    KEY `FK_Label_Workspace_idx` (`WorkspaceId`),
    CONSTRAINT `FK_Label_Workspace` FOREIGN KEY (`WorkspaceId`) REFERENCES `wspc_workspaces` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `job_git_repositories` (
    `Id` varchar(100) NOT NULL,
    `WorkspaceId` varchar(100) NOT NULL,
    `Name` varchar(256) NOT NULL,
    `Owner` varchar(256) NOT NULL,
    `Source` varchar(256) NOT NULL,
    `Repository` varchar(512) NOT NULL,
    `RemoteUrl` varchar(768) NOT NULL,
    `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `Updated` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (`Id`),
    UNIQUE KEY `UNIQUE_Source_Repository` (`Source`,`Repository`),
    KEY `FK_GitRepository_Workspace_idx` (`WorkspaceId`),
    CONSTRAINT `FK_GitRepository_Workspace` FOREIGN KEY (`WorkspaceId`) REFERENCES `wspc_workspaces` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
