CREATE TABLE `stgs_configuration` (
    `Id` varchar(100) NOT NULL,
    `Group` varchar(256) NOT NULL,
    `Key` varchar(256) NOT NULL,
    `IsEncrypted` bit(1) NOT NULL,
    `Value` longtext,
    `ValueEncrypted` longtext,
    `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `Updated` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (`Id`),
    UNIQUE KEY `Config_Group_Key_UNIQUE` (`Group`,`Key`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `stgs_mailing_profiles` (
    `Id` varchar(100) NOT NULL,
    `Code` varchar(100) NOT NULL,
    `Provider` varchar(100) NOT NULL,
    `Server` text,
    `Port` int DEFAULT NULL,
    `Username` text,
    `PasswordEncrypted` longtext,
    `EncryptionType` varchar(100) DEFAULT NULL,
    `ApiUrl` text,
    `ApplicationId` text,
    `ApiSecretEncrypted` longtext,
    `DefaultFromEmail` text,
    `DefaultFromSender` text,
    `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `Updated` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (`Id`),
    UNIQUE KEY `Code_UNIQUE` (`Code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
