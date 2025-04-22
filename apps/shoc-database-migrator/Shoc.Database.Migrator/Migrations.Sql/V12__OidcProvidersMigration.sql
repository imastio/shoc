CREATE TABLE `idp_oidc_providers` (
    `Id` varchar(100) NOT NULL,
    `Code` varchar(100) NOT NULL,
    `Type` varchar(64) NOT NULL,
    `Name` text NOT NULL,
    `IconUrl` text NOT NULL,
    `Authority` text NOT NULL,
    `ResponseType` text NOT NULL,
    `ClientId` text NOT NULL,
    `ClientSecretEncrypted` text NOT NULL,
    `Scope` text NOT NULL,
    `FetchUserInfo` bit(1) NOT NULL,
    `Pkce` bit(1) NOT NULL,
    `Disabled` bit(1) NOT NULL,
    `Trusted` bit(1) NOT NULL,
    `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `Updated` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (`Id`),
    UNIQUE KEY `Code_UNIQUE` (`Code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `idp_oidc_provider_domains` (
    `Id` varchar(100) NOT NULL,
    `ProviderId` varchar(100) NOT NULL,
    `DomainName` varchar(1024) NOT NULL,
    `Verified` bit(1) NOT NULL,
    `Created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `Updated` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (`Id`),
    KEY `FK_OidcProvider_OidcProviderDomain_idx` (`ProviderId`),
    CONSTRAINT `FK_OidcProvider_OidcProviderDomain` FOREIGN KEY (`ProviderId`) REFERENCES `idp_oidc_providers` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;



