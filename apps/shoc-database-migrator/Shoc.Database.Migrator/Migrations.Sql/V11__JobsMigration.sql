CREATE TABLE `job_identity_states` (
    `WorkspaceId` varchar(100) NOT NULL,
    `ObjectType` varchar(64) NOT NULL,
    `Identity` bigint NOT NULL,
    PRIMARY KEY (`WorkspaceId`,`ObjectType`),
    CONSTRAINT `FK_IdentityState_Workspace` FOREIGN KEY (`WorkspaceId`) REFERENCES `wspc_workspaces` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `job_labels` (
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

CREATE TABLE `job_jobs` (
    `Id` varchar(100) NOT NULL,
    `WorkspaceId` varchar(100) NOT NULL,
    `LocalId` bigint NOT NULL,
    `ClusterId` varchar(100) NOT NULL,
    `UserId` varchar(100) NOT NULL,
    `Scope` varchar(64) NOT NULL,
    `Manifest` longtext NOT NULL,
    `ClusterConfigEncrypted` text NOT NULL,
    `Namespace` varchar(256) DEFAULT NULL,
    `TotalTasks` bigint NOT NULL,
    `SucceededTasks` bigint NOT NULL,
    `FailedTasks` bigint NOT NULL,
    `CancelledTasks` bigint NOT NULL,
    `CompletedTasks` bigint NOT NULL,
    `Status` varchar(64) NOT NULL,
    `Message` text NOT NULL,
    `PendingAt` datetime DEFAULT NULL,
    `RunningAt` datetime DEFAULT NULL,
    `CompletedAt` datetime DEFAULT NULL,
    `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `Updated` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (`Id`),
    UNIQUE KEY `UNIQUE_WorkspaceId_LocalId` (`WorkspaceId`,`LocalId`),
    KEY `FK_Job_Workspace_idx` (`WorkspaceId`),
    KEY `FK_Job_User_idx` (`UserId`),
    KEY `FK_Job_Cluster_idx` (`ClusterId`),
    CONSTRAINT `FK_Job_Cluster` FOREIGN KEY (`ClusterId`) REFERENCES `clstr_clusters` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT `FK_Job_User` FOREIGN KEY (`UserId`) REFERENCES `idp_users` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT `FK_Job_Workspace` FOREIGN KEY (`WorkspaceId`) REFERENCES `wspc_workspaces` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `job_job_tasks` (
    `Id` varchar(100) NOT NULL,
    `WorkspaceId` varchar(100) NOT NULL,
    `JobId` varchar(100) NOT NULL,
    `Sequence` bigint NOT NULL,
    `ClusterId` varchar(100) NOT NULL,
    `PackageId` varchar(100) NOT NULL,
    `UserId` varchar(100) NOT NULL,
    `Type` varchar(64) NOT NULL,
    `Runtime` longtext NOT NULL,
    `Args` longtext NOT NULL,
    `PackageReferenceEncrypted` text NOT NULL,
    `ArrayReplicas` bigint NOT NULL,
    `ArrayIndexer` text NOT NULL,
    `ArrayCounter` text NOT NULL,
    `ResolvedEnvEncrypted` longtext NOT NULL,
    `MemoryRequested` bigint DEFAULT NULL,
    `CpuRequested` bigint DEFAULT NULL,
    `NvidiaGpuRequested` bigint DEFAULT NULL,
    `AmdGpuRequested` bigint DEFAULT NULL,
    `Spec` longtext NOT NULL,
    `Status` varchar(64) NOT NULL,
    `Message` text NOT NULL,
    `PendingAt` datetime DEFAULT NULL,
    `RunningAt` datetime DEFAULT NULL,
    `CompletedAt` datetime DEFAULT NULL,
    `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `Updated` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (`Id`),
    UNIQUE KEY `UNIQUE_JobId_Sequence` (`JobId`,`Sequence`),
    KEY `FK_JobTask_Workspace_idx` (`WorkspaceId`),
    KEY `FK_JobTask_Job_idx` (`JobId`),
    KEY `FK_JobTask_User_idx` (`UserId`),
    KEY `FK_JobTask_Cluster_idx` (`ClusterId`),
    KEY `FK_JobTask_Package_idx` (`PackageId`),
    CONSTRAINT `FK_JobTask_Cluster` FOREIGN KEY (`ClusterId`) REFERENCES `clstr_clusters` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
    CONSTRAINT `FK_JobTask_Job` FOREIGN KEY (`JobId`) REFERENCES `job_jobs` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT `FK_JobTask_Package` FOREIGN KEY (`PackageId`) REFERENCES `pkg_packages` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
    CONSTRAINT `FK_JobTask_User` FOREIGN KEY (`UserId`) REFERENCES `idp_users` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT `FK_JobTask_Workspace` FOREIGN KEY (`WorkspaceId`) REFERENCES `wspc_workspaces` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


CREATE TABLE `job_job_labels` (
    `Id` varchar(100) NOT NULL,
    `WorkspaceId` varchar(100) NOT NULL,
    `JobId` varchar(100) NOT NULL,
    `LabelId` varchar(100) NOT NULL,
    PRIMARY KEY (`Id`),
    UNIQUE KEY `UNIQUE_JobId_LabelId` (`JobId`,`LabelId`),
    KEY `FK_JobLabel_Workspace_idx` (`WorkspaceId`),
    KEY `FK_JobLabel_Job_idx` (`JobId`),
    KEY `FK_JobLabel_Label_idx` (`LabelId`),
    CONSTRAINT `FK_JobLabel_Job` FOREIGN KEY (`JobId`) REFERENCES `job_jobs` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT `FK_JobLabel_Label` FOREIGN KEY (`LabelId`) REFERENCES `job_labels` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT `FK_JobLabel_Workspace` FOREIGN KEY (`WorkspaceId`) REFERENCES `wspc_workspaces` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `job_job_git_repositories` (
    `Id` varchar(100) NOT NULL,
    `WorkspaceId` varchar(100) NOT NULL,
    `JobId` varchar(100) NOT NULL,
    `GitRepoId` varchar(100) NOT NULL,
    PRIMARY KEY (`Id`),
    UNIQUE KEY `UNIQUE_JobId` (`JobId`),
    KEY `FK_JobGitRepo_Workspace_idx` (`WorkspaceId`),
    KEY `FK_JobGitRepo_Job_idx` (`JobId`),
    KEY `FK_JobGitRepo_GitRepo_idx` (`GitRepoId`),
    CONSTRAINT `FK_JobGitRepo_GitRepo` FOREIGN KEY (`GitRepoId`) REFERENCES `job_git_repositories` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT `FK_JobGitRepo_Job` FOREIGN KEY (`JobId`) REFERENCES `job_jobs` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT `FK_JobGitRepo_Workspace` FOREIGN KEY (`WorkspaceId`) REFERENCES `wspc_workspaces` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
