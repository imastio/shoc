CREATE TABLE `clstr_clusters` (
    `Id` varchar(100) NOT NULL,
    `WorkspaceId` varchar(100) NOT NULL,
    `Name` varchar(256) NOT NULL,
    `Description` text NOT NULL,
    `Type` varchar(64) NOT NULL,
    `Status` varchar(64) NOT NULL,
    `Configuration` longtext NOT NULL,
    `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `Updated` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (`Id`),
    UNIQUE KEY `UNIQUE_ClusterWorkspaceName` (`WorkspaceId`,`Name`),
    CONSTRAINT `FK_Cluster_Workspace` FOREIGN KEY (`WorkspaceId`) REFERENCES `wspc_workspaces` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
