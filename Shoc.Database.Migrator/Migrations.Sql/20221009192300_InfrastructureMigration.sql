
CREATE TABLE IF NOT EXISTS `data_protection_keys` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `FriendlyName` text,
  `Xml` longtext,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
