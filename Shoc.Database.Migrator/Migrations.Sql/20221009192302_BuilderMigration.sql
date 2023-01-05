
CREATE TABLE IF NOT EXISTS `bld_docker_registries` (
  `Id` varchar(100) NOT NULL,
  `Name` varchar(100) NOT NULL,
  `RegistryUri` varchar(500) NOT NULL,
  `Repository` varchar(500) NOT NULL,
  `AllowNesting` bit(1) NOT NULL DEFAULT b'0',
  `Email` text,
  `Username` text,
  `EncryptedPassword` text,
  `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Updated` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `DockerRegistry_Name_Unique` (`Name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE IF NOT EXISTS `bld_kubernetes_clusters` (
  `Id` varchar(100) NOT NULL,
  `Name` varchar(100) NOT NULL,
  `ApiServerUri` varchar(500) NOT NULL,
  `EncryptedKubeConfig` text NOT NULL,
  `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Updated` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `KubernetesCluster_Name_Unique` (`Name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

